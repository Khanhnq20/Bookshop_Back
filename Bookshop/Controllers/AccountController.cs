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
using AutoMapper;
using Bookshop.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace Bookshop.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
      
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly CookieOptions _cookieOptions;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            DataContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _cookieOptions = new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddDays(1),
                HttpOnly = false,
                Secure = true,
                IsEssential = true,
                Path = "/",
                Domain = _configuration.GetSection("localhost").Value,
                SameSite = SameSiteMode.None,
            };
        }

        [HttpGet("getPersonal")]
        public async Task<IActionResult> GetPersonal(string id)
        {
            var f_user = await _userManager.FindByIdAsync(id);
            return Ok(f_user);
        }

        [HttpGet("getRole")]
        public async Task<IActionResult> GetRole(string id)
        {
            var f_user = await _userManager.FindByIdAsync(id);

            string type = f_user.GetType().Name;
            if (!string.IsNullOrEmpty(type))
            {
                return Ok(type);
            }
            return Ok("");
        }
        //[HttpPost("register")]
        //public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        //{
        //    var user = new ApplicationUser { UserName = model.EmailAddress, Email = model.EmailAddress };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Errors.Count() != 0)
        //    {
        //        return StatusCode(403,result.Errors);
        //    }
        //    return Ok("Registered");
        //}


        [Authorize(Roles = "admin")]
        [HttpPost("register/admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminCreateDTO model)
        {
            var user = _mapper.Map<Admin>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }
            if (result.Errors.Count() != 0)
            {
                return StatusCode(403, result.Errors);
            }
            return Ok("Registered");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("register/staff")]
        public async Task<IActionResult> CreateStaff([FromBody] StaffCreateDTO model)
        {
            var user = _mapper.Map<Staff>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "staff");
            }
            if (result.Errors.Count() != 0)
            {
                return StatusCode(403, result.Errors);
            }
            return Ok("Registered");
        }

        [HttpPost("register/user")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,"user");
            }
            if (result.Errors.Count() != 0)
            {
                return StatusCode(403, result.Errors);
            }
            return Ok("Registered");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, 
                model.Password
                , isPersistent: false, lockoutOnFailure: false
                );

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                var role = await _roleManager.FindByIdAsync(userRole.RoleId);

      
                var tokenResponse = await BuildToken(user.Id,model.Email, role.Name);
                Response.Cookies.Append("jwt", tokenResponse.Token,_cookieOptions);
                Response.Cookies.Append("r_jwt", tokenResponse.RefreshToken, _cookieOptions);
                
                return Ok(tokenResponse);
            }
            else
            {
                return BadRequest(result);
            }

        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("jwt", _cookieOptions);
            Response.Cookies.Delete("r_jwt", _cookieOptions);
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
            
            if(verifiedToken.GetType().GetProperty("Errors") != null  && verifiedToken?.Errors[0] == "We cannot refresh this since the token has not expired")
            {
                var userID = GetClaimValue(token, JwtRegisteredClaimNames.NameId);
                var role = GetClaimValue(token,ClaimTypes.Role );
                return Ok(new { isLogged= true,userId = userID,identity = role});
            }

            return BadRequest(verifiedToken);
        }

        private async Task<AuthResult> BuildToken(string userId,string emailAdress,string role)
        {
            try
            {
                var claims = new List<Claim>()
                {    
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim(JwtRegisteredClaimNames.Email,emailAdress),
                    new Claim(ClaimTypes.Role,role),
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
                var userRole = _context.UserRoles.FirstOrDefault(p => p.UserId == dbUser.Id);
                var role = await _roleManager.FindByIdAsync(userRole.RoleId);

                return await BuildToken(dbUser.Id,dbUser.Email,role.Name);
            }
            catch (Exception ex)
            {
                return new AuthResult {
                    Errors = new List<string> {ex.Message},
                    Success = false
                };
            }
        }

        private string GetClaimValue(string token, string claimType)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            
            try
            {
                var securityToken = jwtTokenHandler.ReadJwtToken(token) as JwtSecurityToken;
                return securityToken.Claims.First(claim => claim.Type == claimType).Value;

            }
            catch (Exception e)
            {

                throw e;
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
