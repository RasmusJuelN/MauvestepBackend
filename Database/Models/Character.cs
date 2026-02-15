using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Character
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        [Required]
        public string CharacterImageUrl { get; set; } = null!;

        // Navigation property
        public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();
    }
}
