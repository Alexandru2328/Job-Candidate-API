using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Job_Candidate_API.Models
{
    public class Candidate
    {
        [Key]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string PreferredContactTime { get; set; }
        public string PhoneNumber { get; set; }

        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public string Comments { get; set; }


    }
}
