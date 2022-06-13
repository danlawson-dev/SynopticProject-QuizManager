using System.ComponentModel.DataAnnotations;

namespace QuizManager.Client.ViewModels
{
    public class QuestionUpdateViewModel
    {
        public int QuizId { get; set; }

        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Question title is required")]
        [Display(Name = "Question title")]
        public string QuestionTitle { get; set; }

        public bool IsExistingQuestion { get; set; }

        public string TitleText =>
            (IsExistingQuestion) ? "Update Question" : "Add Question";

        public string ButtonSubmitText =>
            (IsExistingQuestion) ? "Update" : "Create";

        public int QuestionSequence { get; set; }

        public string CancelledReturnUrl { get; set; }
    }
}