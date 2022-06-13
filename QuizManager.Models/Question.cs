using System.ComponentModel.DataAnnotations.Schema;

namespace QuizManager.Models
{
    [Table("Questions")]
    public class Question
    {
        public int QuestionId { get; set; }
        public int Sequence { get; set; }
        public string Title { get; set; }

        // Parent navigation
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        // Child navigation
        public ICollection<Answer> Answers { get; set; }
    }
}