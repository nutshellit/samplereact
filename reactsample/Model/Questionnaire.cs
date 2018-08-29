using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reactsample.Model
{
    public class Questionnaire
    {
        public string Name { get; set; }
        public List<QuestionnaireSection> Sections { get; set; } = new List<QuestionnaireSection>();
    }

    public class QuestionnaireSection
    {
        public string SectionName { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }

    public enum QuestionType
    {
        Text = 1,
        Radio = 2
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int? ParentQuestionId { get; set; }
        public int? ParentQuestionOptionId { get; set; }
        public List<QuestionOption> Options = new List<QuestionOption>();
    }

    public class QuestionOption
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
    }


    public class QuestionnaireResponse
    {
        public string QuestionnaireName { get; set; }
        public Int64 SubmitTimeStamp { get; set; }
        public List<QuestionnaireResponseItem> Responses { get; set; } = new List<QuestionnaireResponseItem>();
        
    }

    public class QuestionnaireResponseItem
    {
        public int QuestionId { get; set; }
        public string Response { get; set; }
    }

}
