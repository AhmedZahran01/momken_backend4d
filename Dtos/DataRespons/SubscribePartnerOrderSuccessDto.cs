namespace momken_backend.Dtos.DataRespons
{
    public class SubscribePartnerOrderSuccessDto
    {
        public int countOfDayBeforExpiry {  get; set; }
        public string paymentGateway { get; set; } 

   
        public string currency { get; set; } 


        public int MonthCount { get; set; }



        public DateOnly StartFrom { get; set; } 

        public int OrderId { get; set; }

        public string amount { get; set; }

        public DateTime CreatedAt { get; set; } 
        public DateTime ExpierAt { get; set; }

        public string Type { get; set; } = "الاسر المنتجة";
    }


    public class ClientOrderSuccessDto
    {
         public string paymentGateway { get; set; } 
        public  string  currency { get; set; }   
        public  int     OrderId { get; set; } 
        public string   amount { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public string Type { get; set; } = " المشتري ";
    }

}


