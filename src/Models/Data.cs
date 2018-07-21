using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApplicationForMorzanoth.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }

    public class UserQuestion
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? Answer { get; set; }
        public int QuestionId { get; set; }

        public Question Question { get; set; }
        public User User { get; set; }
    }

    public class Database
    {
        public List<User> Users = new List<User>();
        public List<Category> Categories = new List<Category>();
        public List<Question> Questions = new List<Question>();
        public List<UserQuestion> UserQuestions = new List<UserQuestion>();

        public int UserCounter = 0;
        public int CategoryCounter = 0;
        public int QuestionCounter = 0;
        public int UserQuestionCounter = 0;

        private static Database Instance;

        private Database()
        {
            var category1 = new Category { Name = "School" };
            var category2 = new Category { Name = "Sports" };

            var question1 = new Question { Description = "How difficult is your math class?", CategoryId = 1 };
            var question2 = new Question { Description = "How delicious is your lunch?", CategoryId = 1 };
            var question3 = new Question { Description = "How good are your teachers?", CategoryId = 1 };
            var question4 = new Question { Description = "How much do you get bullied?", CategoryId = 1 };
            var question5 = new Question { Description = "How much do you enjoy spanish class?", CategoryId = 1 };
            var question6 = new Question { Description = "How much do you enjoy french class?", CategoryId = 1 };
            var question7 = new Question { Description = "How much do you enjoy shona class?", CategoryId = 1 };
            var question8 = new Question { Description = "How much do you enjoy music class??", CategoryId = 1 };
            var question9 = new Question { Description = "How challenging do you think your school is?", CategoryId = 1 };
            var question10 = new Question { Description = "How delicious is your breakfast?", CategoryId = 1 };
            var question11 = new Question { Description = "How do you rate your teachers?", CategoryId = 1 };
            var question12 = new Question { Description = "How many hours of classes do you have?", CategoryId = 1 };
            var question13 = new Question { Description = "How much do you enjoy sports?", CategoryId = 1 };
            var question14 = new Question { Description = "How are things in your life?", CategoryId = 1 };
            var question15 = new Question { Description = "Are you happy?", CategoryId = 1 };
            var question16 = new Question { Description = "Are you happy again?", CategoryId = 1 };

            var question17 = new Question { Description = "How much do you like football?", CategoryId = 2 };
            var question18 = new Question { Description = "How much do you like squash?", CategoryId = 2 };

            Add(category1);
            Add(category2);

            Add(question1);
            Add(question2);
            Add(question3);
            Add(question4);
            Add(question5);
            Add(question6);
            Add(question7);
            Add(question8);
            Add(question9);
            Add(question10);
            Add(question11);
            Add(question12);
            Add(question13);
            Add(question14);
            Add(question15);
            Add(question16);
            Add(question17);
            Add(question18);

            Add(new User
            {
                UserName = "Mr. Freeze"
            });
        }

        public static Database Intiliaze()
        {
            if (Instance == null)
            {
                Instance = new Database();
            }

            return Instance;
        }

        public void Add(UserQuestion userQuestion)
        {
            this.UserQuestions.Add(new UserQuestion
            {
                Id = ++UserQuestionCounter,
                Answer = userQuestion.Answer,
                QuestionId = userQuestion.QuestionId,
                UserId = userQuestion.UserId,
                User = Users.Find(u => u.Id == userQuestion.UserId),
                Question = Questions.Find(q => q.Id == userQuestion.QuestionId)
            });
        }

        public void Add(Category category)
        {
            this.Categories.Add(new Category
            {
                Id = ++CategoryCounter,
                Name = category.Name
            });
        }

        public void Add(Question question)
        {
            this.Questions.Add(new Question
            {
                Id = ++QuestionCounter,
                Description = question.Description,
                CategoryId = question.CategoryId,
                Category = Categories.Find(c => c.Id == question.CategoryId)
            });
        }

        public void Add(User user)
        {
            this.Users.Add(new User
            {
                Id = ++UserCounter,
                UserName = user.UserName
            });
        }

    }
}