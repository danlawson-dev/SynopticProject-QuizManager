namespace QuizManager.Client.ViewModels
{
    public class QuizDeleteViewModel
    {
        public int QuizId { get; set; }

        public string QuizTitle { get; set; }

        public string CancelledReturnUrl { get; set; }
    }
}