using QuizManager.DataAccess.Interfaces;
using QuizManager.Models;

namespace QuizManager.DataAccess.Data
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly QuizManagerDbContext _dbContext;

        public AnswerRepository(QuizManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Answer GetAnswerById(int answerId) =>
            _dbContext.Answers
                .Where(answer => answer.AnswerId == answerId)
                .FirstOrDefault();

        public IEnumerable<Answer> GetAnswersToReindexForQuestion(string answerToDeleteIndex, int questionId) =>
            _dbContext.Answers
                .Where(answer => (answer.QuestionId == questionId && answer.Index != answerToDeleteIndex));

        public void AddAnswer(Answer answer) =>
            _dbContext.Answers
                .Add(answer);

        public void UpdateAnswer(Answer answer) =>
            _dbContext.Answers
                .Update(answer);

        public void DeleteAnswer(Answer answer) =>
            _dbContext.Answers
                .Remove(answer);

        public void SaveChanges() =>
            _dbContext
                .SaveChanges();
    }
}