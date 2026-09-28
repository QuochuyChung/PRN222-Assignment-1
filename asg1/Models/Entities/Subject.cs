using System.ComponentModel.DataAnnotations;

namespace asg1.Models.Entities
{
    public class Subject
    {
        public int SubjectId { get; set; }

        [Required, MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
