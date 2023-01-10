using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entreprise_Projet.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Pseudo { get; set; } = "";

        [StringLength(100)]
        public string Pass { get; set; } = "";
    }
}
