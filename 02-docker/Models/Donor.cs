namespace server.Models
{
    public class Donor

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<GiftDescription> GiftDescriptions { get; set; } = new List<GiftDescription>();
    }

}
