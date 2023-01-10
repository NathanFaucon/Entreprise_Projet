using Microsoft.EntityFrameworkCore;
using Entreprise_Projet.Models;
namespace Entreprise_Projet.Datas
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Salarie> Salaries { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
