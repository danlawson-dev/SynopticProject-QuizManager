namespace QuizManager.Client.ViewModels
{
    public class QuestionDeleteViewModel
    {
        public int QuizId { get; set; }

        public int QuestionId { get; set; }

        public string QuestionTitle { get; set; }

        public string CancelledReturnUrl { get; set; }
    }
}