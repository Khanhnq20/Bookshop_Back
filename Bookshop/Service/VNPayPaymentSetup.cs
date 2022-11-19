namespace Bookshop.Service
{
    public class VNPayPaymentSetup
    {
        public string HashSecret { get; set; }
        public string ReturnUrl { get; set; }
        public string Url { get; set; }
        public string TmnCode { get; set; }
        public string APIReturn { get; set; }
    }
}