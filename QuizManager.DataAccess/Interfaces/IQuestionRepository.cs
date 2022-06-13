using QuizManager.Models;

namespace QuizManager.DataAccess.Interfaces
{
    public interface IQuestionRepository
    {
        Question GetQuestionById(int questionId);
        IEnumerable<Question> GetQuestionsToResequenceForQuiz(int questionSequence, int quizId);
        void AddQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(Question question);
        void SaveChanges();
    }
}