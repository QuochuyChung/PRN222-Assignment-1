using System.ComponentModel.DataAnnotations;

namespace asg1.DAL.Enums
{
    public enum QuestionSource
    {
        [Display(Name = "Thủ công")]
        Manual = 1,

        [Display(Name = "AI sinh")]
        AIGenerated = 2
    }
}
