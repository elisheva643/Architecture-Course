

namespace server.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public ICollection<GiftDescription> GiftDescriptions { get; set; } = new List<GiftDescription>();
   
    public ICollection<Purchase> Purchases { get; set; }
        public string? ImageUrl { get; set; }
    }
}
