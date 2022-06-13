using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizManager.Client.ViewModels;
using QuizManager.DataAccess.Interfaces;
using QuizManager.Models;
using QuizManager.Models.Helpers;

namespace QuizManager.Client.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IAnswerRepository _answerRepo;
        private readonly IQuestionRepository _questionRepo;

        public AnswerController(IAnswerRepository answerRepo, IQuestionRepository questionRepo)
        {
            _answerRepo = answerRepo;
            _questionRepo = questionRepo;
        }

        [HttpGet]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Update(int questionId, int answerId, string answerIndex)
        {
            bool updateOnly = (answerId > 0);

            if (questionId > 0)
            {
                Question question = _questionRepo.GetQuestionById(questionId);

                if (question == null)
                    return View("NotFound");

                // We cannot add anymore answers if there are already 5
                if (question.Answers.Count >= 5 && !updateOnly)
                    return View("AnswerLimitReached", new AnswerLimitReachedViewModel() { QuestionId = questionId });
            }

            var viewModel = new AnswerUpdateViewModel()
            {
                QuestionId = questionId,
                AnswerIndex = answerIndex
            };

            if (updateOnly)
            {
                Answer answer = _answerRepo.GetAnswerById(answerId);

                if (answer == null)
                    return View("NotFound");

                viewModel.QuestionId = answer.QuestionId;
                viewModel.AnswerId = answer.AnswerId;
                viewModel.AnswerText = answer.Text;
                viewModel.IsCorrectAnswer = answer.IsCorrectAnswer;
                viewModel.IsExistingAnswer = true;
                viewModel.AnswerIndex = answer.Index;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        [ValidateAntiForgeryToken]
        public IActionResult Update(AnswerUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            Answer answer = new Answer()
            {
                QuestionId =viewModel.QuestionId,
                Text = viewModel.AnswerText,
                IsCorrectAnswer = viewModel.IsCorrectAnswer,
                Index = viewModel.AnswerIndex
            };

            if (viewModel.AnswerId > 0)
            {
                // Retrieve the answer and update it
                answer = _answerRepo.GetAnswerById(viewModel.AnswerId);

                if (answer == null)
                    return View("NotFound");

                // We only update the text and correct answer values
                answer.Text = viewModel.AnswerText;
                answer.IsCorrectAnswer = viewModel.IsCorrectAnswer;

                _answerRepo.UpdateAnswer(answer);
            }
            else
            {
                // Add new answer to the database
                _answerRepo.AddAnswer(answer);
            }

            _answerRepo.SaveChanges();

            // Go to the question details page
            return RedirectToAction("Details", "Question", new { questionId = answer.QuestionId });
        }

        [HttpGet]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Delete(int answerId)
        {
            Answer answer = null;

            if (answerId > 0)
                answer = _answerRepo.GetAnswerById(answerId);

            if (answer == null)
                return View("NotFound");

            var viewModel = new AnswerDeleteViewModel()
            {
                QuestionId = answer.QuestionId,
                AnswerId = answer.AnswerId,
                AnswerText = answer.Text
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = UserPolicyConstants.EditPolicy)]
        public IActionResult Delete(AnswerDeleteViewModel viewModel)
        {
            Answer answer = _answerRepo.GetAnswerById(viewModel.AnswerId);

            if (answer == null)
                return View("NotFound");

            _answerRepo.DeleteAnswer(answer);

            // After the answer is deleted, re-index question answers
            ReindexQuestionAnswers(answer.Index, answer.QuestionId);

            _answerRepo.SaveChanges();

            // Go to the question details page
            return RedirectToAction("Details", "Question", new { questionId = viewModel.QuestionId });
        }

        private void ReindexQuestionAnswers(string answerToDeleteIndex, int questionId)
        {
            // Returns the answers for this question, excluding the answer being deleted
            var answers = _answerRepo.GetAnswersToReindexForQuestion(answerToDeleteIndex, questionId);

            if (answers != null)
            {
                foreach (Answer answer in answers)
                {
                    // Get the index value for the current answer
                    int currentAnswerIndex = AnswerIndexerHelper.GetIndex(answer.Index);

                    // If this question is a greater index to the one being deleted, we want to update its index
                    if (currentAnswerIndex > AnswerIndexerHelper.GetIndex(answerToDeleteIndex))
                    {
                        // The new index for this question is 1 less than the current index value (--currentAnswerIndex)
                        answer.Index = AnswerIndexerHelper.AnswerIndexes[--currentAnswerIndex];
                        _answerRepo.UpdateAnswer(answer);
                    }
                }
            }
        }
    }
}