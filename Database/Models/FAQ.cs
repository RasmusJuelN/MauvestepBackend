using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class FAQ
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Question { get; set; } = null!;

        [Required]
        public string Answer { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;
    }
}
