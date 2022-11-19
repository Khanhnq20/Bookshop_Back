using Bookshop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Service
{
    public interface IPayment
    {
        string VNPayment(PurchaseHistory purchaseHistory, string returnUrl);
        PurchaseHistory Comfirm();
    }
}
