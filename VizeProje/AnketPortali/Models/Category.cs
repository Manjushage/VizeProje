namespace AnketPortali.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Survey> Surveys { get; set; }
    }
} 