using Entreprise_Projet.Datas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Entreprise_Projet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Entreprise_Projet.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;

        public AccountController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            User? findUser = context.Users.FirstOrDefault(x => x.Pseudo == user.Pseudo && x.Pass == user.Pass);

            if (findUser != null)
            {
                List<Salarie> Salaries = context.Salaries.ToList();
                ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
                ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
                HttpContext.Session.SetString("IsLogged", user.Pseudo);
                return Redirect("../Salarie/Index");
            }
            else
            {
                HttpContext.Session.SetString("IsLogged", "");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetString("IsLogged", "");
            //HttpContext.Session.Remove("isLogged");
            return Redirect("../Salarie/Index");
        }
    }
}
