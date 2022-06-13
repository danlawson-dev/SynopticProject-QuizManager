using System.ComponentModel.DataAnnotations;

namespace QuizManager.Client.ViewModels
{
    public class QuizUpdateViewModel
    {
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Quiz title is required")]
        [Display(Name = "Quiz title")]
        public string QuizTitle { get; set; }

        public bool IsExistingQuiz { get; set; }

        public string TitleText =>
            (IsExistingQuiz) ? "Update Quiz" : "Add Quiz";

        public string ButtonSubmitText =>
            (IsExistingQuiz) ? "Update" : "Create";

        public string CancelledReturnUrl { get; set; }
    }
}