using QuizManager.Models;

namespace QuizManager.DataAccess.Interfaces
{
    public interface IQuizRepository
    {
        IEnumerable<Quiz> GetAllQuizzes();
        Quiz GetQuizById(int quizId);
        bool CheckQuizExistsWithTitle(string quizTitle);
        void AddQuiz(Quiz quiz);
        void UpdateQuiz(Quiz quiz);
        void DeleteQuiz(Quiz quiz);
        void SaveChanges();
    }
}