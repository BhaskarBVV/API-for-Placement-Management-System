using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace PMS_api.Model
{
    public class Student
    {
        [Key]
        [Required]
        public long roll_number { get; set; }

        [Required]
        public string name { get; set; }
    }
}
