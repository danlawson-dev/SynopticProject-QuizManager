namespace QuizManager.Client.ViewModels
{
    public class QuestionDetailsAnswersViewModel
    {
        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

        public string AnswerIndex { get; set; }

        public bool IsCorrectAnswer { get; set; }
    }
}