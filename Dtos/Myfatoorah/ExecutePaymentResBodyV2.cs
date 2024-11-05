namespace momken_backend.Dtos.Myfatoorah
{
    public class ExecutePaymentResBodyV2
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object ValidationErrors { get; set; }
        public InvoiceData Data { get; set; }
    }
    public class InvoiceData
    {
        public int InvoiceId { get; set; }
        public bool IsDirectPayment { get; set; }
        public string PaymentURL { get; set; }
        public string RecurringId { get; set; }
    }
}
