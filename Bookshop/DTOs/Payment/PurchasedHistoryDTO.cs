using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Payment
{
    public class PurchasedHistoryDTO
    {
        public string UserId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
        public bool confirmStatus { get; set; }
        public int DeliveryFee { get; set; }
        public string Date { get; set; }
        public List<PurchasedProductDTO> PurchasedProducts { get; set; }
    }
}
