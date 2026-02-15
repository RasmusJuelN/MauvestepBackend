using System;

namespace API.DTOs
{
    public class FAQDto
    {
        public Guid Id { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string Category { get; set; } = null!;
    }

    public class CreateFAQDto
    {
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string Category { get; set; } = null!;
    }

    public class UpdateFAQDto
    {
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}