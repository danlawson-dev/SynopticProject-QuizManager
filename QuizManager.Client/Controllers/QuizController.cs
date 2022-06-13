using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Client.ViewModels;
using QuizManager.DataAccess.Interfaces;
using QuizManager.Models;
using QuizManager.Models.Helpers;

namespace QuizManager.Client.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepo;

        public QuizController(IQuizRepository quizRepo)
        {
            _quizRepo = quizRepo;
        }

        [HttpGet]
        public IActionResult List()
        {
            var quizzes = _quizRepo.GetAllQuizzes();

            bool userHasFullRights = User.HasClaim(UserPermissionLevelClaimConstants.ClaimType, UserPermissionLevelClaimConstants.Edit);

            var viewModel = new QuizListViewModel()
            {
                UserHasFullRights = userHasFullRights,
                Quizzes = quizzes.Select(quiz => new QuizListItemViewModel()
                {
                    QuizId = quiz.QuizId,
                    QuizTitle = quiz.Title
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int quizId)
        {
            Quiz quiz = _quizRepo.GetQuizById(quizId);

            if (quiz == null)
                return View("NotFound");

            bool userHasFullRights = User.HasClaim(UserPermissionLevelClaimConstants.ClaimType, UserPermissionLevelClaimConstants.Edit);
            bool userHasViewRights = User.HasClaim(UserPermissionLevelClaimConstants.ClaimType, UserPermissionLevelClaimConstants.View);

            var viewModel = new QuizDetailsViewModel()
            {
                QuizId = quiz.QuizId,
                QuizTitle = quiz.Title,
                UserHasFullRights = userHasFullRights,
                CanViewQuestionDetails = userHasViewRights
            };

            if (quiz.Questions != null)
            {
                viewModel.Questions = new List<QuizDetailsQuestionsViewModel>();

                foreach (Question question in quiz.Questions)
                {
                    bool questionIsValid = false;

                    // Question must have at least 3 answers to be valid
                    if (question.Answers != null && question.Answers.Count >= 3)
                    {
                        questionIsValid = true;

                        // We now need to check that there is at least 1 correct answer
                        int correctAnswers = 0;
                        foreach (Answer answer in question.Answers)
                            if (answer.IsCorrectAnswer)
                                correctAnswers++;

                        // Question is only valid if there is at least 1 correct answer for it
                        questionIsValid = (correctAnswers >= 1);
                    }

                    viewModel.Questions.Add(new QuizDetailsQuestionsViewModel()
                    {
                        QuizId = question.QuizId,
                        QuestionId = question.QuestionId,
                        QuestionTitle = question.Title,
                        QuestionSequence = question.Sequence,
                        IsValid = questionIsValid
                    });
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Update(int quizId)
        {
            // We want to keep the page that the user was on if they select 'Cancel'
            var viewModel = new QuizUpdateViewModel()
            {
                CancelledReturnUrl = Request.Headers["Referer"].ToString()
            };

            if (quizId > 0)
            {
                Quiz quiz = _quizRepo.GetQuizById(quizId);

                if (quiz == null)
                    return View("NotFound");

                viewModel.QuizId = quiz.QuizId;
                viewModel.QuizTitle = quiz.Title;
                viewModel.IsExistingQuiz = true;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        [ValidateAntiForgeryToken]
        public IActionResult Update(QuizUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Check to see if a quiz exists with this title
            bool quizAlreadyExistsWithTitle = _quizRepo.CheckQuizExistsWithTitle(viewModel.QuizTitle);
            if (quizAlreadyExistsWithTitle)
            {
                ModelState.AddModelError(string.Empty, QuizConstants.QuizAlreadyExistsWithTitle);
                return View(viewModel);
            }

            Quiz quiz = new Quiz()
            {
                Title = viewModel.QuizTitle
            };

            if (viewModel.QuizId > 0)
            {
                // Retrieve the quiz and update it
                quiz = _quizRepo.GetQuizById(viewModel.QuizId);

                if (quiz == null)
                    return View("NotFound");

                // We only update the title
                quiz.Title = viewModel.QuizTitle;
                    
                _quizRepo.UpdateQuiz(quiz);
            }
            else
            {
                // Add new quiz to the database
                _quizRepo.AddQuiz(quiz);
            }

            _quizRepo.SaveChanges();

            // Go to the quiz details page
            return RedirectToAction("Details", new { quizId = quiz.QuizId });
        }

        [HttpGet]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Delete(int quizId)
        {
            Quiz quiz = null;

            if (quizId > 0)
                quiz = _quizRepo.GetQuizById(quizId);

            if (quiz == null)
                return View("NotFound");

            // We want to keep the page that the user was on if they select 'No'
            var viewModel = new QuizDeleteViewModel()
            {
                QuizId = quiz.QuizId,
                QuizTitle = quiz.Title,
                CancelledReturnUrl = Request.Headers["Referer"].ToString()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Delete(QuizDeleteViewModel viewModel)
        {
            Quiz quiz = _quizRepo.GetQuizById(viewModel.QuizId);

            if (quiz == null)
                return View("NotFound");

            _quizRepo.DeleteQuiz(quiz);
            _quizRepo.SaveChanges();

            // Go to the quiz list page
            return RedirectToAction("List");
        }
    }
}