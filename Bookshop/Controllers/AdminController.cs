using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bookshop.DTOs.User;
using Bookshop.Entity;
using Bookshop.SQLContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bookshop.Controllers
{   
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AdminController(IMapper mapper, UserManager<ApplicationUser> userManager, IConfiguration configuration, DataContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }
        
        [AllowAnonymous]
        [HttpGet("getUser")]
        public async Task<IActionResult> UserManagement(string searchString)
        {
            var users = await _context.Users.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                var f_user = await _context.Users.Where(s => (s.Name.Contains(searchString) || s.Email.Contains(searchString))).ToListAsync();
                return Ok(f_user);
            }
            return Ok(users);
        }


        [HttpGet("getStaff")]
        public async Task<IActionResult> StaffManagement(string searchString)
        {
            var staffs = await _context.Staffs.ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                var f_user = await _context.Staffs.Where(s => (s.Name.Contains(searchString) || s.Email.Contains(searchString))).ToListAsync();
                return Ok(f_user);
            }
            return Ok(staffs);
        }

        [HttpGet("getPurchaseHistory")]
        public async Task<IActionResult> GetPurchaseHistory(string searchString)
        {
            var history = await _context.PurchaseHistories.Include(u => u.User).ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                var f_history = await _context.PurchaseHistories.Where(s => (s.Date.Contains(searchString) || s.User.Email.Contains(searchString))).ToListAsync();
                return Ok(f_history);
            }
            return Ok(history);
        }

        [AllowAnonymous]
        [HttpGet("getSingleUser")]
        public async Task<IActionResult> getSingleUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("getSinglePurchaseHistory")]
        public async Task<IActionResult> GetSinglePurchaseHistory(int id)
        {
            var history = await _context.PurchaseHistories
                .Include(u => u.User)
                .Include(t => t.PurchasedProducts)
                    .ThenInclude(p => p.products)
                   .FirstOrDefaultAsync(p => p.Id == id);
            return Ok(history);
        }

        
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(string id,string password)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(f => f.Id == id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, password);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
        [AllowAnonymous]
        [HttpPost("userUpdate")]
        public async Task<IActionResult> UserUpdate([FromBody]UserUpdateDTO request, string id)
        {
                var user = await _userManager.Users.FirstOrDefaultAsync(f => f.Id == id);
                var update_user = _mapper.Map(request, user);
                await _userManager.UpdateAsync(update_user);
                return Ok();
        }

        [HttpGet("payment/verify")]
        public async Task<IActionResult> Verify(int id)
        {   
            var history = await _context.PurchaseHistories.FirstOrDefaultAsync(f => f.Id == id);
            int number = id;
            int numberTwo = id + 2;
            history.Verify = true;
            _context.SaveChanges();
            return Ok();
        }
    }
}
