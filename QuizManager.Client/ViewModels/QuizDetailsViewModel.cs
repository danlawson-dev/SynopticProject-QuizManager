namespace QuizManager.Client.ViewModels
{
    public class QuizDetailsViewModel
    {
        public int QuizId { get; set; }

        public string QuizTitle { get; set; }

        public bool UserHasFullRights { get; set; }

        public bool CanViewQuestionDetails { get; set; }

        public List<QuizDetailsQuestionsViewModel> Questions { get; set; }

        public bool HasQuestions => 
            (Questions != null && Questions.Count > 0);

        public int NextQuestionSequence =>
            (HasQuestions) ? Questions.Count + 1 : 1;
    }
}