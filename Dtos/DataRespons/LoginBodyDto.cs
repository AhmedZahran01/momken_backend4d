namespace momken_backend.Dtos.DataRespons
{
    public class LoginBodyDto
    {
        public PartnerBodyResDto Partner {  get; set; }
        public string Token { get; set; }
    }

    public class LoginClientBodyDto
    {
        public ClientBodyResDto client { get; set; }
        public string Token { get; set; }
    }
}
