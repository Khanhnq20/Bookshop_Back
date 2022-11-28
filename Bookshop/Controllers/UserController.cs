using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bookshop.DTOs.Payment;
using Bookshop.DTOs.Product;
using Bookshop.Entity;
using Bookshop.Service;
using Bookshop.SQLContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Controllers
{
    [ApiController]
    [Authorize(Roles = "user,admin,staff")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IPayment _payment;

        public UserController(DataContext context, IMapper mapper, IPayment payment)
        {
            this._context = context;
            this._mapper = mapper;
            this._payment = payment;
        }
        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody]PurchasedHistoryDTO request,string returnURL)
        {
            var purchaseHistory = _mapper.Map<PurchaseHistory>(request);
            string link = _payment.VNPayment(purchaseHistory, returnURL);
            _context.PurchaseHistories.Add(purchaseHistory);
            _context.SaveChanges();
            return Ok(link);
        }
        [HttpGet("payment/confirm")]
        public async Task<IActionResult> ConfirmPayment(string refCode)
        {
            var purchaseHistory = _context.PurchaseHistories.Include(p => p.PurchasedProducts).FirstOrDefault(f => f.UniqueCode == refCode);
            purchaseHistory.confirmStatus = true;

            _context.SaveChanges();
            return Ok(purchaseHistory);
        }
        [AllowAnonymous]
        [HttpPost("comment")]
        public async Task<IActionResult> Comment([FromBody]CommentCreationDTO request)
        {
            var comment = _mapper.Map<Comment>(request);
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return Ok("Success");
        }

        [AllowAnonymous]
        [HttpGet("getComment")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _context.Comments.Where(p =>p.ProductId == id).ToListAsync();

            return Ok(comment);
        }

        [HttpGet("getPurchased")]
        public async Task<IActionResult> GetPurchased(string id)
        {
            var history = await _context.PurchaseHistories
                .Include(u => u.User)
                .Include(t => t.PurchasedProducts)
                    .ThenInclude(p => p.products)
                   .Where(p => p.UserId == id).ToListAsync();
            return Ok(history);
        }

    }
}
