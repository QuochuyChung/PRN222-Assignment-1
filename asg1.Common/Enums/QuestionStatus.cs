using System.ComponentModel.DataAnnotations;

namespace asg1.Common.Enums
{
    public enum QuestionStatus
    {
        [Display(Name = "Draft")]
        Draft = 1,

        [Display(Name = "Approved")]
        Approved = 2,

        [Display(Name = "Rejected")]
        Rejected = 3
    }
}
