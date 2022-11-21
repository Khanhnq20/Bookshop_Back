using Bookshop.SQLContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity
{
    public class PurchaseHistory
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string UniqueCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool confirmStatus { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public int DeliveryFee { get; set; }
        public string Date { get; set; }
        public List<PurchasedProduct> PurchasedProducts { get; set; }
    }

    public class PurchasedProduct
    {
        public int Id { get; set; }

        [ForeignKey(nameof(products))]
        public int ProductId { get; set; }
        public Product products { get; set; }

        public string Type { get; set; }
        public int Quantity { get; set; }
        public int TotalProductsFee { get; set; }


        [ForeignKey(nameof(PurchaseHistory))]
        public int HistoryId { get; set; }
        public PurchaseHistory PurchaseHistory { get; set; }
    }
}
