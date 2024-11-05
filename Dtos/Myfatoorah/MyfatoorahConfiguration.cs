namespace momken_backend.Dtos.Myfatoorah
{
    public class MyfatoorahConfiguration
    {
        public string BaseUrl { get; set; }
        public string Language { get; set; } = "AR";
        public string DisplayCurrencyIso { get; set; } = "SAR";
        public string Token { get; set; }
        public string SingningKeyOrder { get; set; }
    }
}
