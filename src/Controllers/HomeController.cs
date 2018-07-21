using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestApplicationForMorzanoth.Models;

namespace TestApplicationForMorzanoth.Controllers
{
    public class HomeController : Controller
    {
        private Database _database = Database.Intiliaze();
        private const int numberOfQuestions = 3;
        private string loggedInUserName = "Mr. Freeze";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Category()
        {
            var user = _database.Users
              .Find(u => u.UserName == loggedInUserName);

            var categories = new List<CategoryItem>();

            foreach (Category category in _database.Categories.ToList())
            {
                var hasStarted = _database.UserQuestions
                        .Where(uq => uq.UserId == user.Id
                            && uq.Question.CategoryId == category.Id)
                        .Any();

                var categoryItem = new CategoryItem
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    HasCompleted = !HasOpenQuestions(user.Id, category.Id),
                    HasStarted = hasStarted
                };

                categories.Add(categoryItem);
            }

            return View(categories);
        }

        public ActionResult SurveyCompleted(int categoryId)
        {

            var category = _database.Categories
                .Find(c => c.Id == categoryId);

            return View(new SurveyCompletedView { CategoryName = category.Name });
        }

        public ActionResult Survey(int categoryId)
        {
            int questionCounter = 0;

            var category = _database.Categories
                .Find(c => c.Id == categoryId);

            var user = _database.Users
                .Find(u => u.UserName == loggedInUserName);

            if (!HasOpenQuestions(user.Id, category.Id))
            {
                return RedirectToAction("SurveyCompleted", new { categoryId = category.Id });
            }

            if (category != null)
            {
                var questions = _database.Questions
                    .Where(q => q.Category.Id == categoryId)
                    .Take(numberOfQuestions)
                    .Select(q => new UserQuestionView
                    {
                        QuestionDescription = q.Description,
                        QuestionId = q.Id,
                        Counter = ++questionCounter
                    })
                    .ToList();

                var survey = new SurveyView
                {
                    CategoryName = category.Name,
                    CategoryId = category.Id,
                    Questions = questions,
                    UserId = user.Id,
                };

                return View(survey);
            }
            else
            {
                return HttpNotFound("Unable to find a categroy with matching identifier.");
            }
        }

        [HttpPost]
        public ActionResult Next(SurveyView model)
        {
            if (!ModelState.IsValid)
            {
                return View("Survey", model);
            }

            foreach (var userQuestionView in model.Questions)
            {
                UserQuestion userQuestion = new UserQuestion
                {
                    Answer = userQuestionView.Answer,
                    QuestionId = userQuestionView.QuestionId,
                    UserId = model.UserId
                };

                _database.Add(userQuestion);
            }

            if (!HasOpenQuestions(model.UserId, model.CategoryId))
            {
                return RedirectToAction("SurveyCompleted", new { categoryId = model.CategoryId });
            }

            var counter = _database.UserQuestions
                .Where(uq => uq.Question.CategoryId == model.CategoryId
                             && uq.UserId == model.UserId)
                .Count();

            var remainingQuestions = _database.Questions
                .Where(nq => nq.Category.Id == model.CategoryId
                    && !_database.UserQuestions
                        .Where(uq => uq.UserId == model.UserId)
                        .Select(q => q.QuestionId)
                        .Contains(nq.Id))
                .Take(numberOfQuestions)
                .Select(q => new UserQuestionView
                {
                    QuestionId = q.Id,
                    QuestionDescription = q.Description,
                    Counter = ++counter
                })
                .ToList();

            var survey = new SurveyView
            {
                CategoryName = model.CategoryName,
                CategoryId = model.CategoryId,
                Questions = remainingQuestions,
                UserId = model.UserId,
            };

            ModelState.Clear();

            return View("Survey", survey);
        }

        [HttpGet]
        public ActionResult Load(int categoryId)
        {
            var user = _database.Users
              .Find(u => u.UserName == loggedInUserName);

            var category = _database.Categories
                .Find(c => c.Id == categoryId);

            if (!HasOpenQuestions(user.Id, category.Id))
            {
                return RedirectToAction("SurveyCompleted", new { categoryId = category.Id });
            }

            var answeredQuestions = _database.UserQuestions
                .Where(uq => uq.Question.Category.Id == categoryId
                    && uq.UserId == user.Id);


            int counter = answeredQuestions.Count();

            var survey = new SurveyView
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                UserId = user.Id
            };


            survey.Questions = _database.Questions
                .Where(nq => nq.Category.Id == categoryId
                    && !answeredQuestions
                        .Select(q => q.Question.Id)
                        .Contains(nq.Id))
                .Take(numberOfQuestions)
                .Select(q => new UserQuestionView
                {
                    Counter = ++counter,
                    QuestionDescription = q.Description,
                    QuestionId = q.Id,
                })
                .ToList();

            ModelState.Clear();

            return View("Survey", survey);
        }

        private bool HasOpenQuestions(int userId, int categoryId)
        {
            return GetNumberOfOpenQuestions(userId, categoryId) > 0;
        }

        private int GetNumberOfOpenQuestions(int userId, int categoryId)
        {
            var numberOfQuestions = _database.Questions
                .Where(q => q.CategoryId == categoryId)
                .Count();

            var numberOfAnswered = _database.UserQuestions
                .Where(uq => uq.Question.CategoryId == categoryId
                    && uq.UserId == userId)
                .Count();

            return numberOfQuestions - numberOfAnswered;

        }
    }
}