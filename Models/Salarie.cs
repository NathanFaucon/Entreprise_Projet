using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entreprise_Projet.Models
{
    public class Salarie
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string NomSalarie { get; set; } = "";
        [StringLength(50)]
        public string PrenomSalarie { get; set; } = "";
        [StringLength(15)]
        public string PhoneFixe { get; set; } = "";
        [StringLength(15)]
        public string PhonePortable { get; set; } = "";
        [StringLength(100)]
        public string Mail { get; set; } = "";
        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public Site Site { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

    }
}
