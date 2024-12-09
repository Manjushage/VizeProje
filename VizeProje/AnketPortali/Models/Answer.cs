namespace AnketPortali.Models
{
    public enum AnswerType
    {
        Kötü = 1,
        Orta = 2,
        İyi = 3,
        Çokİyi = 4
    }

    public class Answer : BaseEntity
    {
        public AnswerType AnswerValue { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
} 