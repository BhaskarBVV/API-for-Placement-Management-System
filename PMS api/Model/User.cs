using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_api.Model
{
    public class User
    {
        [Key]
        [Required]
        public string userName { get; set; }

        [Required]
        public string pass { get; set; }
    }
}
