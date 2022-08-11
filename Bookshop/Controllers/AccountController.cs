using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Bookshop.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Bookshop.SQLContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Bookshop.Entity;
using System.ComponentModel.DataAnnotations;

namespace Bookshop.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            DataContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.EmailAddress, Email = model.EmailAddress };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Errors.Count() != 0)
            {
                return StatusCode(403,result.Errors);
            }
            return Ok("Registered");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password
                , isPersistent: false, lockoutOnFailure: false
                );
            
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
      
                var tokenResponse = await BuildToken(user.Id,model.EmailAddress);
                Response.Cookies.Append("jwt", tokenResponse.Token,new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Domain = Request.Host.Host,
                    Path="/",
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                    HttpOnly=true,
                    Secure=true
                });
                Response.Cookies.Append("r_jwt", tokenResponse.RefreshToken, new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Domain = Request.Host.Host,
                    Path = "/",
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                    HttpOnly=true,
                    Secure=true

                });
                return Ok(tokenResponse);
            }
            else
            {
                return BadRequest(result);
            }

        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            if (ModelState.IsValid)
            {
                string accessToken =Request.Cookies["jwt"];
                string refreshToken = Request.Cookies["r_jwt"];

                var res = await VerifyToken(new TokenRequest()
                {
                    Token = accessToken,
                    RefreshToken = refreshToken
                });

                if (res == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {"Invalid tokens"},
                        Success = false
                    });
                }

                return Ok(res);
            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>() {"Invalid payload"},
                Success = false
            });
        }

        [HttpGet("userPersistence")]
        public async Task<IActionResult> GetAccessCertificate()
        {
            var token = Request.Cookies.FirstOrDefault(key => key.Key == "jwt").Value;
            var refreshToken = Request.Cookies.FirstOrDefault(key => key.Key == "r_jwt").Value;
            if(String.IsNullOrEmpty(token) || String.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Cookies null");
            }

            var verifiedToken = await VerifyToken(new TokenRequest() { Token = token, RefreshToken = refreshToken });

            if(verifiedToken.Errors[0] == "We cannot refresh this since the token has not expired")
            {
                return Ok(new { isLogged= true });
            }

            return BadRequest(verifiedToken);
        }

        private async Task<AuthResult> BuildToken(string userId,string emailAdress)
        {
            try
            {
                var claims = new List<Claim>()
                {    
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim(JwtRegisteredClaimNames.Email,emailAdress),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.UtcNow.AddHours(6);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: creds
                 );
                var tokenHandler = new JwtSecurityTokenHandler();

                string tokenStr = tokenHandler.WriteToken(token);

                var refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsUsed = false,
                    AddedDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    Token = RandomString(25) + Guid.NewGuid(),
                    UserId = userId
                };

                _context.RefreshTokens.Update(refreshToken);
                await _context.SaveChangesAsync();

                return new AuthResult()
                {
                    Token = tokenStr,
                    RefreshToken = refreshToken.Token,
                    Expriration = expiration,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new AuthResult()
                {
                    Errors = new List<string>
                    {
                        e.Message
                    },
                    Success= false
                };
            }
        }
        private async Task<AuthResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // This validation function will make sure that the token meets the validation parameters
                // and its an actual jwt token not just a random string
                var _tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("jwt:key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.Zero
                }; 
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
                _tokenValidationParameters.ValidateLifetime = true;
                // Now we need to check if the token has a valid security algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Will get the time stamp in unix time
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                // we convert the expiry date from seconds to the date
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                        Success = false
                    };
                }

                // Check the token we got if its saved in the db
                var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "refresh token doesnt exist" },
                        Success = false
                    };
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has expired, user needs to relogin" },
                        Success = false
                    };
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been used" },
                        Success = false
                    };
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been revoked" },
                        Success = false
                    };
                }

                // we are getting here the jwt token id
                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // check the id that the recieved token has against the id saved in the db
                if (storedRefreshToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "the token doenst mateched the saved token" },
                        Success = false
                    };
                }

                storedRefreshToken.IsUsed = true;
                _context.RefreshTokens.Update(storedRefreshToken);
                await _context.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
                return await BuildToken(dbUser.Id,dbUser.Email);
            }
            catch (Exception ex)
            {
                return new AuthResult {
                    Errors = new List<string> {ex.Message},
                    Success = false
                };
            }
        }


        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }


    }
}
