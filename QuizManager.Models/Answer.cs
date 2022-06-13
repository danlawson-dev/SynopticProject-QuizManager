using System.ComponentModel.DataAnnotations.Schema;

namespace QuizManager.Models
{
    [Table("Answers")]
    public class Answer
    {
        public int AnswerId { get; set; }
        public string Index { get; set; }
        public string Text { get; set; }
        public bool IsCorrectAnswer { get; set; }

        // Parent navigation
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}