using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Ability
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Character")]
        public Guid CharacterId { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int Damage { get; set; }

        [Required]
        public int ApCost { get; set; }

        [Required]
        public int ApGain { get; set; }

        [Required]
        public int HpGain { get; set; }

        [Required]
        public string AbilityImageUrl { get; set; } = null!;

        // Navigation property
        public virtual Character Character { get; set; } = null!;
    }
}
