using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace server.Models.DTO
{
    public class PurchaseDTO
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GiftId { get; set; }

        [Required]
        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "כמות חייבת להיות לפחות 1")]
        public int Quantity { get; set; } 

        public bool IsDraft { get; set; }

       

    }
}
