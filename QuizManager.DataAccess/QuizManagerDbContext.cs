using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizManager.Models;

namespace QuizManager.DataAccess
{
    public class QuizManagerDbContext : IdentityDbContext<User>
    {
        public QuizManagerDbContext(DbContextOptions<QuizManagerDbContext> options) : base(options)
        {

        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}