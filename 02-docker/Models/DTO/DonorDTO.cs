using System.ComponentModel.DataAnnotations;

namespace server.Models.DTO
{
    public class DonorDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "חובה להזין שם תורם")]
        [MinLength(2, ErrorMessage = "שם חייב להכיל לפחות 2 תווים")]
        public string Name { get; set; }

        [Required(ErrorMessage = "חובה להזין מייל")]
        [EmailAddress(ErrorMessage = "כתובת מייל לא תקינה")]
        public string Email { get; set; }
    }
}
