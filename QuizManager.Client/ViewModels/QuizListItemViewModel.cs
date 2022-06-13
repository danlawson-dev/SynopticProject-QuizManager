namespace QuizManager.Client.ViewModels
{
    public class QuizListItemViewModel
    {
        public int QuizId { get; set; }

        public string QuizTitle { get; set; }

        public string QuizTitleNoSpaces =>
            QuizTitle.Replace(" ", "");
    }
}