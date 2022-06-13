namespace QuizManager.Client.ViewModels
{
    public class QuizListViewModel
    {
        public string PageTitle =>
            "Quizzes";

        public bool UserHasFullRights { get; set; }

        public List<QuizListItemViewModel> Quizzes { get; set; }

        public bool HasQuizzes =>
            (Quizzes != null && Quizzes.Count > 0);
    }
}