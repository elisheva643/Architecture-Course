using System.ComponentModel.DataAnnotations;

namespace server.Models.DTO
{
    public class RegisterDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "חובה להזין שם")]
        [MinLength(2, ErrorMessage = "שם חייב להכיל לפחות 2 תווים")]
        public string Name { get; set; }

        [Required(ErrorMessage = "חובה להזין אימייל")]
        [EmailAddress(ErrorMessage = "אימייל לא תקין")]
        public string Email { get; set; }

        [Required(ErrorMessage = "חובה להזין סיסמה")]
        [MinLength(6, ErrorMessage = "סיסמה חייבת להכיל לפחות 6 תווים")]
        public string Password { get; set; }
    }
}
