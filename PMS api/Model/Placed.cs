using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace PMS_api.Model
{
    public class Placed
    {
        public Student Student { get; set; }
        public Companies Companies { get; set; }

        [Key]
        [Required]
        [ForeignKey("Student")]
        public long roll_number { get; set; }
        

        [Required]
        [ForeignKey("Companies")]
        public long company_id { get; set; }


    }
}
