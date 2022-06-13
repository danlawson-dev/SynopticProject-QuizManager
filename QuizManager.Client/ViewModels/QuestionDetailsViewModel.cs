using QuizManager.Models.Helpers;

namespace QuizManager.Client.ViewModels
{
    public class QuestionDetailsViewModel
    {
        public int QuizId { get; set; }

        public int QuestionId { get; set; }

        public string QuestionTitle { get; set; }

        public bool UserHasFullRights { get; set; }

        public List<QuestionDetailsAnswersViewModel> Answers { get; set; }

        public bool HasAnswers =>
            (Answers != null && Answers.Count > 0);

        public bool CanHaveMoreAnswersAdded =>
            (Answers == null || Answers.Count < 5);

        public string NextAnswerIndex
        {
            get
            {
                int index = (HasAnswers) ? Answers.Count : 0;

                return AnswerIndexerHelper.AnswerIndexes[index];
            }
        }

        public List<string> InvalidQuestionErrors { get; set; }
    }
}