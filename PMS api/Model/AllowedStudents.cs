using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_api.Model
{
    public class AllowedStudents
    {
        public Student Student { get; set; }
        public Companies Companies { get; set; }
        
        [Required]
        [ForeignKey("Companies")]
        public long company_id { get; set; }

        [Required]
        [ForeignKey("Student")]
        public long student_roll_no { get; set; }
    }
}
