namespace AnketPortali.Models
{
    public class Question : BaseEntity
    {
        public string QuestionText { get; set; }
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
} 