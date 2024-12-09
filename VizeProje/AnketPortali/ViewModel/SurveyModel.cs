namespace AnketPortali.ViewModel
{
    public class SurveyModel
    {
        public int? Id { get; set; } // Survey
    
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } // Survey
        public int CategoryId { get; set; }
    }
}
