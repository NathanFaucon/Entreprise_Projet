using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entreprise_Projet.Models
{
    public class Site
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string NomSite { get; set; } = "";
    }
}
