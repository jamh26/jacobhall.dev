using System.ComponentModel.DataAnnotations;

namespace jacobhall.dev.Models
{
    public class ContactFormModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Message { get; set; }
    }
}