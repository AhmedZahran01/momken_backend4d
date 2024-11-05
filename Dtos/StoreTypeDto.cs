namespace momken_backend.Dtos
{
    public class StoreTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubTypeDto> SubTypes { get; set; }
    }
}
