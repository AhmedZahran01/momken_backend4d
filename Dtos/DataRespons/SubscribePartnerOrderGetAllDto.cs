namespace momken_backend.Dtos.DataRespons
{
    public class SubscribePartnerOrderGetAllDto
    {
        public Guid Id { get; set; }
        public int OrderId { get; set; }

        public string amount { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
