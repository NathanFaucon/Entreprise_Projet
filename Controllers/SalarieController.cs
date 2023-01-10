using Entreprise_Projet.Datas;
using Entreprise_Projet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Entreprise_Projet.Controllers
{
    public class SalarieController : Controller
    {
        private readonly ApplicationDbContext context;

        public SalarieController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(string sortOrder)
        {
            List<Salarie> Salaries = context.Salaries.ToList();
            ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
            ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
            ViewData["NomSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nom_desc" : "";
            ViewData["PrenomSortParm"] = (sortOrder=="prenom") ? "prenom_desc" : "prenom";
            //ViewData["SiteSortParm"] = sortOrder == "Site" ? "site_desc" : "site";
            //ViewData["ServiceSortParm"] = sortOrder == "Service" ? "service_desc" : "service";
            var salaries = from s in Salaries
                           select s;
            switch (sortOrder)
            {
                case "nom_desc":
                    salaries = salaries.OrderByDescending(s => s.NomSalarie);
                    break;
                case "prenom":
                    salaries = salaries.OrderBy(s => s.PrenomSalarie);
                    break;
                case "prenom_desc":
                    salaries = salaries.OrderByDescending(s => s.PrenomSalarie);
                    break;
                /*case "site":
                    salaries = salaries.OrderBy(s => s.SiteId);
                    break;
                case "site_desc":
                    salaries = salaries.OrderByDescending(s => s.SiteId);
                    break;
                case "service":
                    salaries = salaries.OrderBy(s => s.ServiceId);
                    break;
                case "service_desc":
                    salaries = salaries.OrderByDescending(s => s.ServiceId);
                    break;*/
                default:
                    salaries = salaries.OrderBy(s => s.NomSalarie);
                    break;
            }
            //return View(Salaries);
            return View(salaries);
        }
        public IActionResult Create()
        {
            ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
            ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
            return View();
        }

        public IActionResult Edit(int id)
        {
            Salarie Salarie = GetSalarieById(id);
            ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
            ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
            return View(Salarie);
        }
        public IActionResult Details(int id)
        {
            Salarie Salarie = GetSalarieById(id);
            ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
            ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
            return View(Salarie);
        }
        public IActionResult Filter(string searchString, string searchSite, string searchService)
        {
            ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
            ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
            List<Salarie> filteredList=null;
            var salaries = context.Salaries.ToList();
            if ((string.IsNullOrEmpty(searchString)) && (searchSite == "Site") && (searchService == "Service")) return View("Index", salaries);
            if (!string.IsNullOrEmpty(searchString))
            {
               filteredList = salaries.Where((x => x.NomSalarie.ToUpper().Contains(searchString.ToUpper()) || x.PrenomSalarie.ToUpper().Contains(searchString.ToUpper()))).ToList();
            }
            if (searchSite != "Site")
            {
                if (filteredList == null)
                {
                    filteredList = salaries.Where(x => x.Site.Id == Int32.Parse(searchSite)).ToList();
                }
                else
                {
                    filteredList = filteredList.Where(x => x.Site.Id == Int32.Parse(searchSite)).ToList();
                }
                
            }
            if (searchService != "Service")
            {
                if (filteredList == null)
                {
                    filteredList = salaries.Where(x => x.Service.Id == Int32.Parse(searchService)).ToList();
                }
                else
                {
                    filteredList = filteredList.Where(x => x.Service.Id == Int32.Parse(searchService)).ToList();
                }

            }
            return View("Index", filteredList);
        }

        [HttpGet("salaries")]
        public IActionResult GetSalaries()
        {
            List<Salarie> mySalaries = context.Salaries.ToList();

            if (mySalaries.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Salaries:",
                    Salarie = mySalaries
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun salarie dans la base de données !"
                });
            }
        }
        [HttpGet("salaries/{salarieId}")]
        public IActionResult GetSalariesById(int salarieId)
        {
            Salarie? findSalarie = context.Salaries.FirstOrDefault(x => x.Id == salarieId);

            if (findSalarie == null)
            {
                return NotFound(new
                {
                    Message = "Aucun salarie trouvé avec cet ID !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Salarie trouvé !",
                    Salarie = new SalarieDTO() { Id = findSalarie.Id, NomSalarie = findSalarie.NomSalarie }
                });
            }
        }

        public Salarie GetSalarieById(int salarieId)
        {
            return context.Salaries.FirstOrDefault(x => x.Id == salarieId);
        }

        [HttpPost("salaries")]
        public IActionResult AddSalaries(SalarieDTO newSalarie)
        {
            Salarie addSalarie = new Salarie()
            {
                NomSalarie = newSalarie.NomSalarie,
                PrenomSalarie = newSalarie.PrenomSalarie,
                PhoneFixe = newSalarie.PhoneFixe,
                PhonePortable = newSalarie.PhonePortable,
                Mail = newSalarie.Mail,
                Service = newSalarie.Service,
                Site = newSalarie.Site
            };

            Service? findService = context.Services.FirstOrDefault(x => x.Id == newSalarie.Service.Id);

            if (findService == null)
            {
                return NotFound(new
                {
                    Message = "Aucun service trouvé avec cet ID !"
                });
            }
            else
            {
                addSalarie.Service = findService;
            }

            Site? findSite = context.Sites.FirstOrDefault(x => x.Id == newSalarie.Site.Id);

            if (findSite == null)
            {
                return NotFound(new
                {
                    Message = "Aucun site trouvé avec cet ID !"
                });
            }
            else
            {
                addSalarie.Site = findSite;
            }

            context.Salaries.Add(addSalarie);
            if (context.SaveChanges() > 0)
            {
                List<Salarie> Salaries = context.Salaries.ToList();
                ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
                ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
                return View("Index", Salaries);
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }

        public IActionResult EditSalarie(SalarieDTO newInfos)
        {
            Salarie? findSalarie = context.Salaries.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findSalarie != null)
            {

                findSalarie.NomSalarie = newInfos.NomSalarie;
                findSalarie.PrenomSalarie = newInfos.PrenomSalarie;
                findSalarie.PhoneFixe = newInfos.PhoneFixe;
                findSalarie.PhonePortable = newInfos.PhonePortable;
                findSalarie.Mail = newInfos.Mail;
                findSalarie.Service = newInfos.Service;
                findSalarie.Site = newInfos.Site;

                Service? findService = context.Services.FirstOrDefault(x => x.Id == findSalarie.Service.Id);

                if (findService == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucun service trouvé avec cet ID !"
                    });
                }
                else
                {
                    findSalarie.Service.NomService = findService.NomService;
                }

                Site? findSite = context.Sites.FirstOrDefault(x => x.Id == findSalarie.Site.Id);

                if (findSite == null)
                {
                    return NotFound(new
                    {
                        Message = "Aucun site trouvé avec cet ID !"
                    });
                }
                else
                {
                    findSalarie.Site.NomSite = findSite.NomSite;
                }

                //context.Salaries.Update(findSalarie);
                if (findSite != null && findSite.Id!=findSalarie.SiteId)
                {
                    context.Entry(findSite).State = EntityState.Detached;
                }
                if (findService != null)
                {

                    context.Entry(findService).State = EntityState.Detached;
                }
                context.Salaries.Attach(findSalarie);
                context.Entry(findSalarie).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                if (context.SaveChanges() > 0)
                {
                    List<Salarie> Salaries = context.Salaries.ToList();
                    ViewBag.Sites = new SelectList(context.Sites.ToList(), "Id", "NomSite");
                    ViewBag.Services = new SelectList(context.Services.ToList(), "Id", "NomService");
                    return View("Index", Salaries);
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur a eu lieu durant la modification..."
                    });
                }
            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun salarie n'a été trouvé avec cet ID !"
                });
            }
        }

        public IActionResult DeleteSalarie(int id)
        {
            Salarie? findSalarie = context.Salaries.FirstOrDefault(x => x.Id == id);

            if (findSalarie == null)
            {
                return NotFound(new
                {
                    Message = "Aucun salarie trouvé avec cet ID !"
                });
            }
            else
            {
                context.Salaries.Remove(findSalarie);
                if (context.SaveChanges() > 0)
                {
                    List<Salarie> Salaries = context.Salaries.ToList();
                    return View("Index", Salaries);
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur est survenue..."
                    });
                }
            }
        }
    }
}
