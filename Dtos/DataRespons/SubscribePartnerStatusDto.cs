namespace momken_backend.Dtos.DataRespons
{
    public class SubscribePartnerStatusDto
    {
        public int DayCountBeforExpiry { get; set; }
        public string DateExpiry { get; set; }
        public string Status { get; set; }

        public int MonthPrice { get; set; } = 10;

        public string Type { get; set; } = "الاسر المنتجة";
    }
}
