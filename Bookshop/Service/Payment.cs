using Bookshop.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Service
{
    public class Payment : IPayment
    {
        private readonly VNPayPaymentSetup ConfigurationManager;
        private IHttpContextAccessor _httpContextAccessor;
        public Payment(IOptions<VNPayPaymentSetup> configurationManager, IHttpContextAccessor httpContextAccessor)
        {
            ConfigurationManager = configurationManager.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        public PurchaseHistory Comfirm()
        {
            throw new NotImplementedException();
        }

        public string VNPayment(PurchaseHistory purchaseHistory, string returnUrl)
        {
            //Get Config Info
            string vnp_Returnurl = !String.IsNullOrEmpty(returnUrl) ? returnUrl : ConfigurationManager.ReturnUrl; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.Url; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.TmnCode; //Ma website
            string vnp_HashSecret = ConfigurationManager.HashSecret; //Chuoi bi mat

            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                throw new Exception("Pls Provide Config");
            }
            string locale = "vn";
            //Build URL for VNPAY
            VnpayConfig vnpay = new VnpayConfig();
            string host = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}";
            var hasOrigin = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Origin", out var origin);
            if (hasOrigin)
            {
                host = origin;
            }

            vnpay.AddRequestData("vnp_Version", VnpayConfig.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((purchaseHistory.PurchasedProducts.Sum(p=>p.TotalProductsFee) + purchaseHistory.DeliveryFee) * 100).ToString()); 
            vnpay.AddRequestData("vnp_BankCode", "NCB");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            vnpay.AddRequestData("vnp_Locale", "vn");
            string orderInfoString = "payment for boko shop";
            vnpay.AddRequestData("vnp_OrderInfo", "Paid :");

            vnpay.AddRequestData("vnp_ReturnUrl", host + vnp_Returnurl);
            string uniqueRef = $"{Guid.NewGuid().ToString()}-{Helper.RandomString(49)}";
            vnpay.AddRequestData("vnp_TxnRef", uniqueRef);
            purchaseHistory.UniqueCode = uniqueRef;
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddHours(1).ToString("yyyyMMddHHmmss"));


            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }
    }
}
