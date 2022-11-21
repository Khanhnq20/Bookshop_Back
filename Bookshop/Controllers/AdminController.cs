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
    }
}
