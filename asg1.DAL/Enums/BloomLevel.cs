using System.ComponentModel.DataAnnotations;

namespace asg1.DAL.Enums
{
    public enum BloomLevel
    {
        [Display(Name = "Nhớ")]
        Remember = 1,

        [Display(Name = "Hiểu")]
        Understand = 2,

        [Display(Name = "Vận dụng")]
        Apply = 3,

        [Display(Name = "Phân tích")]
        Analyze = 4
    }
}
