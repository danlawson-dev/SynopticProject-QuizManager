using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Client.ViewModels;
using QuizManager.DataAccess.Interfaces;
using QuizManager.Models;
using QuizManager.Models.Helpers;

namespace QuizManager.Client.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepo;

        public QuestionController(IQuestionRepository questionRepo)
        {
            _questionRepo = questionRepo;
        }

        [HttpGet]
        [Authorize(Policy = UserPolicyConstants.ViewPolicy)]
        public IActionResult Details(int questionId)
        {
            Question question = _questionRepo.GetQuestionById(questionId);

            if (question == null)
                return View("NotFound");

            bool userHasFullRights = User.HasClaim(UserPermissionLevelClaimConstants.ClaimType, UserPermissionLevelClaimConstants.Edit);

            var viewModel = new QuestionDetailsViewModel()
            {
                QuizId = question.QuizId,
                QuestionId = question.QuestionId,
                QuestionTitle = question.Title,
                UserHasFullRights = userHasFullRights,
                InvalidQuestionErrors = new List<string>()
            };

            if (question.Answers != null)
            {
                // Question must have atleast 3 answers to be valid
                if (question.Answers.Count < 3)
                    viewModel.InvalidQuestionErrors.Add(QuestionConstants.NumberOfAnswersRequired);

                viewModel.Answers = new List<QuestionDetailsAnswersViewModel>();

                // We now need to check there is at least 1 correct answer
                int correctAnswers = 0;

                foreach (Answer answer in question.Answers)
                {
                    if (answer.IsCorrectAnswer)
                        correctAnswers++;

                    viewModel.Answers.Add(new QuestionDetailsAnswersViewModel()
                    {
                        QuestionId = answer.QuestionId,
                        AnswerId = answer.AnswerId,
                        AnswerText = answer.Text,
                        AnswerIndex = answer.Index,
                        IsCorrectAnswer = answer.IsCorrectAnswer
                    });
                }

                // Question is only valid if there is at least 1 correct answer for it
                if (correctAnswers < 1)
                    viewModel.InvalidQuestionErrors.Add(QuestionConstants.AtleastOneCorrectAnswerRequired);
            }
            else
            {
                // We don't have answers for this question, so add it to the errors list
                viewModel.InvalidQuestionErrors.Add(QuestionConstants.NumberOfAnswersRequired);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Update(int quizId, int questionId, int questionSequence)
        {
            // We want to keep the page that the user was on if they select 'Cancel'
            var viewModel = new QuestionUpdateViewModel()
            {
                QuizId = quizId,
                QuestionSequence = questionSequence,
                CancelledReturnUrl = Request.Headers["Referer"].ToString()
            };

            if (questionId > 0)
            {
                Question question = _questionRepo.GetQuestionById(questionId);

                if (question == null)
                    return View("NotFound");

                viewModel.QuizId = question.QuizId;
                viewModel.QuestionId = question.QuizId;
                viewModel.QuestionTitle = question.Title;
                viewModel.IsExistingQuestion = true;
                viewModel.QuestionSequence = question.Sequence;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        [ValidateAntiForgeryToken]
        public IActionResult Update(QuestionUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            Question question = new Question()
            {
                QuizId = viewModel.QuizId,
                Title = viewModel.QuestionTitle,
                Sequence = viewModel.QuestionSequence
            };

            if (viewModel.QuestionId > 0)
            {
                // Retrieve the question and update it
                question = _questionRepo.GetQuestionById(viewModel.QuestionId);

                if (question == null)
                    return View("NotFound");

                // We only update the title
                question.Title = viewModel.QuestionTitle;

                _questionRepo.UpdateQuestion(question);
            }
            else
            {
                // Add new question to the database
                _questionRepo.AddQuestion(question);
            }

            _questionRepo.SaveChanges();

            // Go to the quiz details page
            return RedirectToAction("Details", new { questionId = question.QuestionId });
        }

        [HttpGet]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Delete(int questionId)
        {
            Question question = null;

            if (questionId > 0)
                question = _questionRepo.GetQuestionById(questionId);

            if (question == null)
                return View("NotFound");

            // We want to keep the page that the user was on if they select 'No'
            var viewModel = new QuestionDeleteViewModel()
            {
                QuizId = question.QuizId,
                QuestionId = question.QuestionId,
                QuestionTitle = question.Title,
                CancelledReturnUrl = Request.Headers["Referer"].ToString()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Delete(QuestionDeleteViewModel viewModel)
        {
            Question question = _questionRepo.GetQuestionById(viewModel.QuestionId);

            if (question == null)
                return View("NotFound");

            _questionRepo.DeleteQuestion(question);

            // After the question is deleted, re-sequence quiz questions
            ResequenceQuizQuestions(question.Sequence, question.QuizId);

            _questionRepo.SaveChanges();

            // Go to the quiz details page
            return RedirectToAction("Details", "Quiz", new { quizId = viewModel.QuizId });
        }

        private void ResequenceQuizQuestions(int questionSequence, int quizId)
        {
            // This only returns the questions for the quiz where their sequence is greater than the question being deleted
            var questions = _questionRepo.GetQuestionsToResequenceForQuiz(questionSequence, quizId);

            if (questions != null)
            {
                foreach (Question question in questions)
                {
                    // -1 from the sequence
                    question.Sequence--;
                    _questionRepo.UpdateQuestion(question);
                }
            }
        }
    }
}