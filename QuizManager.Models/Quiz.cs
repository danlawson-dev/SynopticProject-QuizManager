using System.ComponentModel.DataAnnotations.Schema;

namespace QuizManager.Models
{
    [Table("Quizzes")]
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; }

        // Child navigation
        public ICollection<Question> Questions { get; set; }
    }
}