namespace momken_backend.Dtos.DataRespons
{
    public class LoginBodyDto
    {
        public PartnerBodyResDto Partner {  get; set; }
        public string Token { get; set; }
    }
}
