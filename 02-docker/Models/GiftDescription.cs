using System.Text.Json.Serialization;

namespace server.Models
{
    public class GiftDescription
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
        public int DonerId { get; set; }
        [JsonIgnore]
        public Donor Doner { get; set; }
        public int Quantity { get; set; }
    }
}
