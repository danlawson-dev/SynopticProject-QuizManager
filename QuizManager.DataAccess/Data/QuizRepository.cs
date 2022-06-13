using Microsoft.EntityFrameworkCore;
using QuizManager.DataAccess.Interfaces;
using QuizManager.Models;

namespace QuizManager.DataAccess.Data
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizManagerDbContext _dbContext;

        public QuizRepository(QuizManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Quiz> GetAllQuizzes() =>
            _dbContext.Quizzes;

        public Quiz GetQuizById(int quizId) =>
            _dbContext.Quizzes
                .Where(quiz => quiz.QuizId == quizId)
                .Include(quiz => quiz.Questions)
                .ThenInclude(question => question.Answers)
                .FirstOrDefault();

        public bool CheckQuizExistsWithTitle(string quizTitle)
        {
            // Check each quiz title for a match
            foreach (Quiz quiz in _dbContext.Quizzes)
                if (quiz.Title == quizTitle)
                    return true;

            return false;
        }

        public void AddQuiz(Quiz quiz) => 
            _dbContext.Quizzes
                .Add(quiz);

        public void UpdateQuiz(Quiz quiz) => 
            _dbContext.Quizzes
                .Update(quiz);

        public void DeleteQuiz(Quiz quiz) => 
            _dbContext.Quizzes
                .Remove(quiz);

        public void SaveChanges() =>
            _dbContext
                .SaveChanges();
    }
}