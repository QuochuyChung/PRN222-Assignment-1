using System.ComponentModel.DataAnnotations;

namespace asg1.Common.Enums
{
    public enum QuestionSource
    {
        [Display(Name = "Manual")]
        Manual = 1,

        [Display(Name = "AI generated")]
        AIGenerated = 2
    }
}
