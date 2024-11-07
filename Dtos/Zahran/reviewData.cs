namespace momken_backend.Dtos.Zahran
{
    public class reviewDataDto
    {
        public Guid clientId { get; set; }
        public Guid partnerStoreId { get; set; }

        public string ReviewMessage { get; set; }

        public int evaluationNumber { get; set; }

    }
}
