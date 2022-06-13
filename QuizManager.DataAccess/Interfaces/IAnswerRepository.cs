using QuizManager.Models;

namespace QuizManager.DataAccess.Interfaces
{
    public interface IAnswerRepository
    {
        Answer GetAnswerById(int answerId);
        IEnumerable<Answer> GetAnswersToReindexForQuestion(string answerToDeleteIndex, int questionId);
        void AddAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
        void SaveChanges();
    }
}