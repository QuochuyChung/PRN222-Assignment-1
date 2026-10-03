using System.ComponentModel.DataAnnotations;

namespace asg1.Common.Enums
{
    public enum BloomLevel
    {
        [Display(Name = "Remember")]
        Remember = 1,

        [Display(Name = "Understand")]
        Understand = 2,

        [Display(Name = "Apply")]
        Apply = 3,

        [Display(Name = "Analyze")]
        Analyze = 4
    }
}
