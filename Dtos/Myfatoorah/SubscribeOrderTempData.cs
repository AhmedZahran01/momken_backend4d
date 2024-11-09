namespace momken_backend.Dtos.Myfatoorah
{
    public class SubscribeOrderTempData
    {
        public string InvoiceId { get; set; }
        public Guid userId { get; set; }
        public int MonthCount { get; set; }

    }

    public class PaidClientOrderTempData
    {
        public string InvoiceId { get; set; }
        public Guid userId { get; set; }
    }
}
