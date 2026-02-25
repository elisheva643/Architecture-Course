using System.ComponentModel.DataAnnotations;

namespace server.Models.DTO
{
    public class GiftDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "חובה להזין שם מתנה")]
        [MinLength(2, ErrorMessage = "שם מתנה חייב להכיל לפחות 2 תווים")]
        public string Name { get; set; }

        [Required(ErrorMessage = "חובה להזין תיאור")]
        [MinLength(5, ErrorMessage = "תיאור קצר מדי")]
        public string Description { get; set; }
        [Required(ErrorMessage = "חובה להזין קטגוריה")] 
        public string Category { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "מחיר חייב להיות גדול מ־0")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
