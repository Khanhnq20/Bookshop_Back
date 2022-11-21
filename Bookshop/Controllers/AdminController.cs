using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bookshop.SQLContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bookshop.Controllers
{   
    [ApiController]
    [Route("api/admin")]
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

        [HttpGet("getUser")]
        public async Task<IActionResult> UserManagement()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("getStaff")]
        public async Task<IActionResult> StaffManagement()
        {
            var staffs = await _context.Staffs.ToListAsync();
            return Ok(staffs);
        }

        [HttpGet("getPurchaseHistory")]
        public async Task<IActionResult> GetPurchaseHistory()
        {
            var history = await _context.PurchaseHistories.Include(u => u.User).ToListAsync();
            return Ok(history);
        }

        [HttpGet("getSingleUser")]
        public async Task<IActionResult> getSingleUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return Ok(user);
        }

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
            var user = await _userManager.Users.FirstOrDefaultAsync(f => f.Id == id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            return Ok();
        }
    }
}
