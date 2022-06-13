using QuizManager.Models.Helpers;

namespace QuizManager.Client.ViewModels
{
    public class QuizDetailsQuestionsViewModel
    {
        public int QuizId { get; set; }

        public int QuestionId { get; set; }

        public string QuestionTitle { get; set; }

        public int QuestionSequence { get; set; }

        public bool IsValid { get; set; }
    }
}