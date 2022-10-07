using System.ComponentModel.DataAnnotations;

namespace Lab_2.Models
{
    public class Team
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name="Established Date")]
        [DataType(DataType.Date)]
        public string EstablishedDate { get; set; }
    }
}
