namespace QuizManager.Models.Helpers
{
    public class ApplicationConstants
    {
        public static string CopyrightStatement = $"Copyright © {@DateTime.Today.Year} WebbiSkools Ltd";
    }

    public class UserPolicyConstants
    {
        public const string RestrictedPolicy = "Restricted";
        public const string ViewPolicy = "View";
        public const string EditPolicy = "Edit";
    }

    public class UserPermissionLevelClaimConstants
    {
        public const string ClaimType = "PermissionLevel";
        public const string Restricted = "Restricted";
        public const string View = "View";
        public const string Edit = "Edit";
    }

    public class AccountConstants
    {
        public const string UserNotFound = "The user cannot be found";
        public const string IncorrectPassword = "Incorrect password entered";
        public const string AccessDenied = "Access Denied";
        public const string AccessDeniedMessage = "You do not have rights to view this page";
    }

    public class QuizConstants
    {
        public const string NoneToShow = "No quizzes to show";
        public const string QuizAlreadyExistsWithTitle = "A quiz already exists with this title";
    }

    public class QuestionConstants
    {
        public const string NoneToShow = "No questions to show";
        public const string QuestionDetailsNoRights = "You do not have rights to view the details of this question";
        public const string Invalid = "(Invalid)";
        public const string NumberOfAnswersRequired = "There must be between 3 and 5 answers";
        public const string AtleastOneCorrectAnswerRequired = "There must be at least 1 correct answer";
    }

    public class AnswerConstants
    {
        public const string NoneToShow = "No answers to show";
        public const string AnswerModityNoRights = "You do not have rights to modify this answer";
        public const string IsCorrectAnswer = "(Correct answer)";
        public const string AnswerLimitReached = "Answer Limit Reached";
        public const string AnswerLimitReachedMessage = "Questions can only have a maximum of 5 answers. This has already been reached for this question";
    }

    public class NotFoundConstants
    {
        public const string NotFound = "Not Found";
        public const string NotFoundMessage = "The item you are looking for cannot be found";
    }
}