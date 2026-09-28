using System.ComponentModel.DataAnnotations;

namespace asg1.DAL.Enums
{
    public enum QuestionStatus
    {
        [Display(Name = "Nháp")]
        Draft = 1,

        [Display(Name = "Đã duyệt")]
        Approved = 2,

        [Display(Name = "Bị loại")]
        Rejected = 3
    }
}
