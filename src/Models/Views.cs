using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestApplicationForMorzanoth.Models
{
    public class SurveyView
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public IList<UserQuestionView> Questions { get; set; }
    }

    public class UserQuestionView
    {
        [Required]
        public int? Answer { get; set; }
        public int QuestionId { get; set; }
        public string QuestionDescription { get; set; }
        public int Counter { get; set; }
    }

    public class SurveyCompletedView
    {
        public string CategoryName { get; set; }
    }

    public class CategoryItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool HasCompleted { get; set; }
        public bool HasStarted { get; set; }
    }
}