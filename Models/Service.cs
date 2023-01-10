using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entreprise_Projet.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string NomService { get; set; } = "";
    }
}
