namespace AnketPortali.ViewModel
{
    public class AddQuestionViewModel
    {
        public int SurveyId { get; set; }
        public string QuestionText { get; set; }
        public List<QuestionViewModel>? Questions { get; set; }
    }
}
