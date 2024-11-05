namespace momken_backend.Dtos.Myfatoorah
{
    public class ExecutePaymentV2
    {
        public int PaymentMethodId { get; set; }
        public decimal InvoiceValue { get; set; }
        public string Language { get; set; } = "AR";
        public string CallBackUrl { get; set; }
        public string ErrorUrl { get; set; }
        public string ExpiryDate { get; set; } = DateTime.UtcNow.AddHours(3).ToString("yyyy-MM-ddTHH:mm:ssZ");
        public string DisplayCurrencyIso { get; set; } = "SAR";
        public List<InvoiceItem> InvoiceItems { get; set; }
    }

    public class InvoiceItem
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
    
