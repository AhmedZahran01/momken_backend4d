namespace momken_backend.Dtos.Myfatoorah
{
    public class GetPaymentStatusV2ResDataDto
    {
        
    public bool IsSuccess { get; set; }               
        public string Message { get; set; }             
        public List<ValidationError> ValidationErrors { get; set; } 
        public GetPaymentStatusResponse Data { get; set; }  
    }
    public class ValidationError
    {
        public string Name { get; set; }                    // Example: "FieldName"
        public string Error { get; set; }                   // Example: "Error message"
    }

    // GetPaymentStatusResponse class already defined
    public class GetPaymentStatusResponse
    {
        public int InvoiceId { get; set; }                  // 0
        public string InvoiceStatus { get; set; }           // "string"
        public string InvoiceReference { get; set; }        // "string"
        public string CustomerReference { get; set; }       // "string"
        public DateTime CreatedDate { get; set; }           // "2024-10-10T04:45:40.969Z"
        public string ExpiryDate { get; set; }              // "string"
        public string ExpiryTime { get; set; }              // "string"
        public decimal InvoiceValue { get; set; }           // 0
        public string Comments { get; set; }                // "string"
        public string CustomerName { get; set; }            // "string"
        public string CustomerMobile { get; set; }          // "string"
        public string CustomerEmail { get; set; }           // "string"
        public string UserDefinedField { get; set; }        // "string"
        public string InvoiceDisplayValue { get; set; }     // "string"
        public decimal DueDeposit { get; set; }             // 0
        public string DepositStatus { get; set; }           // "string"
        public List<InvoiceItemModel> InvoiceItems { get; set; }  // Array of InvoiceItemModel
        public List<TransactionModel> InvoiceTransactions { get; set; }  // Array of TransactionModel
        public List<SupplierModel> Suppliers { get; set; }  // Array of SupplierModel
    }

    // InvoiceItemModel class already defined
    public class InvoiceItemModel
    {
        public string ItemName { get; set; }                // "string"
        public int Quantity { get; set; }                   // 0
        public decimal UnitPrice { get; set; }              // 0
        public decimal? Weight { get; set; }                // 0 (nullable)
        public decimal? Width { get; set; }                 // 0 (nullable)
        public decimal? Height { get; set; }                // 0 (nullable)
        public decimal? Depth { get; set; }                 // 0 (nullable)
    }

    // TransactionModel class
    public class TransactionModel
    {
        public string TransactionDate { get; set; }         // "2024-10-10T07:42:58.5966667"
        public string PaymentGateway { get; set; }          // "VISA/MASTER"
        public string ReferenceId { get; set; }             // "07074494060226289072"
        public string TrackId { get; set; }                 // "10-10-2024_2262890"
        public string TransactionId { get; set; }           // "07074494060226289072"
        public string PaymentId { get; set; }               // "07074494060226289072"
        public string AuthorizationId { get; set; }         // "07074494060226289072"
        public string TransactionStatus { get; set; }       // "Failed"
        public string TransationValue { get; set; }         // "0.810"
        public string CustomerServiceCharge { get; set; }   // "0.000"
        public string TotalServiceCharge { get; set; }      // "0.101"
        public string DueValue { get; set; }                // "0.810"
        public string PaidCurrency { get; set; }            // "KD"
        public string PaidCurrencyValue { get; set; }       // "0.810"
        public float VatAmount { get; set; }               // "0.015"
        public string IpAddress { get; set; }               // "156.197.175.173"
        public string Country { get; set; }                 // "Egypt"
        public string Currency { get; set; }                // "KD"
        public string Error { get; set; }                   // "DECLINED : Do not honour"
        public string CardNumber { get; set; }              // "545454xxxxxx5454"
        public string ErrorCode { get; set; }               // "MF002"
    }

    // SupplierModel class
    public class SupplierModel
    {
        public int? SupplierCode { get; set; }              // Example: null
        public string SupplierName { get; set; }            // Example: "string"
        public decimal? InvoiceShare { get; set; }          // Example: 0
        public decimal? ProposedShare { get; set; }         // Example: 0
        public decimal? DepositShare { get; set; }          // Example: 0
    }
}
