using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bookshop.DTOs.Payment;
using Bookshop.Entity;
using Bookshop.Service;
using Bookshop.SQLContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Controllers
{
    [ApiController]
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
        
    }
}
