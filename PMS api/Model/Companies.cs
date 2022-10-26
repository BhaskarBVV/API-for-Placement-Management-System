using System.ComponentModel.DataAnnotations;

namespace PMS_api.Model
{
    public class Companies
    {
        [Key]
        [Required]
        public long company_id { get; set; }
        [Required]
        public string company_name { get; set; }
        [Required]
        public float package { get; set; }
    }
}
