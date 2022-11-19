using Bookshop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Payment
{
    public class PurchasedProductDTO
    {
        public int ProductId { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public int TotalProductsFee { get; set; }
        public int HistoryId { get; set; }
    }
}
