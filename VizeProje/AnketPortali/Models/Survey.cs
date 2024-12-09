namespace AnketPortali.Models
{
    public class Survey : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public bool IsActive { get; set; } // Survey

        public Category Category { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
} 