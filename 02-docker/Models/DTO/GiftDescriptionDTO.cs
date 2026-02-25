using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace server.Models.DTO
{
    public class GiftDescriptionDTO
    {
        [Required]
        public int GiftId { get; set; }

        [Required]
        public int DonerId { get; set; }
        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "כמות חייבת להיות גדולה מ-0")]
        public int Quantity { get; set; }
      
    }
}
