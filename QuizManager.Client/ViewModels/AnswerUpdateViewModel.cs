using System.ComponentModel.DataAnnotations;

namespace QuizManager.Client.ViewModels
{
    public class AnswerUpdateViewModel
    {
        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        [Required(ErrorMessage = "Answer is required")]
        [Display(Name = "Answer")]
        public string AnswerText { get; set; }

        [Display(Name = "Is correct answer")]
        public bool IsCorrectAnswer { get; set; }

        public bool IsExistingAnswer { get; set; }

        public string TitleText =>
            (IsExistingAnswer) ? "Update Answer" : "Add Answer";

        public string ButtonSubmitText =>
            (IsExistingAnswer) ? "Update" : "Create";

        public string AnswerIndex { get; set; }
    }
}