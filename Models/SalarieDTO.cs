namespace Entreprise_Projet.Models
{
    public class SalarieDTO
    {
        public int? Id { get; set; }
        public string NomSalarie { get; set; } = "";
        public string PrenomSalarie { get; set; } = "";
        public string PhoneFixe { get; set; } = "";
        public string PhonePortable { get; set; } = "";
        public string Mail { get; set; } = "";
        public Service Service { get; set; }
        public Site Site { get; set; }
    }
}
