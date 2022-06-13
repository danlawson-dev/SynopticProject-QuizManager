using Microsoft.EntityFrameworkCore;
using QuizManager.DataAccess.Interfaces;
using QuizManager.Models;

namespace QuizManager.DataAccess.Data
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizManagerDbContext _dbContext;

        public QuestionRepository(QuizManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Question GetQuestionById(int questionId) =>
            _dbContext.Questions
                .Where(question => question.QuestionId == questionId)
                .Include(question => question.Answers)
                .FirstOrDefault();

        public IEnumerable<Question> GetQuestionsToResequenceForQuiz(int questionSequence, int quizId) =>
            _dbContext.Questions
                .Where(question => (question.QuizId == quizId && question.Sequence > questionSequence));

        public void AddQuestion(Question question) =>
            _dbContext.Questions
                .Add(question);

        public void UpdateQuestion(Question question) =>
            _dbContext.Questions
                .Update(question);

        public void DeleteQuestion(Question question) =>
            _dbContext.Questions
                .Remove(question);

        public void SaveChanges() =>
            _dbContext
                .SaveChanges();
    }
}