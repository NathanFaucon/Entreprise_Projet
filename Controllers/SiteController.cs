using Entreprise_Projet.Datas;
using Entreprise_Projet.Models;
using Microsoft.AspNetCore.Mvc;

namespace Entreprise_Projet.Controllers
{
    public class SiteController : Controller
    {
        private readonly ApplicationDbContext context;

        public SiteController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<Site> Sites = context.Sites.ToList();
            return View(Sites);
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            Site Site = GetSiteById(id);
            return View(Site);
        }

        public IActionResult Filter(string searchString)
        {
            var sites = context.Sites.ToList();
            if (string.IsNullOrEmpty(searchString)) return View("Index", sites);

            var filteredList = sites.Where(x => x.NomSite.ToUpper().Contains(searchString.ToUpper())).ToList();

            return View("Index", filteredList);
        }

        [HttpGet("sites")]
        public IActionResult GetSites()
        {
            List<Site> mySites = context.Sites.ToList();

            if (mySites.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Sites:",
                    Site = mySites
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun site dans la base de données !"
                });
            }
        }
        [HttpGet("sites/{siteId}")]
        public IActionResult GetSitesById(int siteId)
        {
            Site? findSite = context.Sites.FirstOrDefault(x => x.Id == siteId);

            if (findSite == null)
            {
                return NotFound(new
                {
                    Message = "Aucun site trouvé avec cet ID !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Site trouvé !",
                    Site = new SiteDTO() { Id = findSite.Id, NomSite = findSite.NomSite }
                });
            }
        }

        public Site GetSiteById(int siteId)
        {
            return context.Sites.FirstOrDefault(x => x.Id == siteId);
        }

        [HttpPost("sites")]
        public IActionResult AddSites(SiteDTO newSite)
        {
            Site addSite= new Site()
            {
                NomSite = newSite.NomSite,
            };
            context.Sites.Add(addSite);
            if (context.SaveChanges() > 0)
            {
                List<Site> Sites = context.Sites.ToList();
                return View("Index", Sites);
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }

        public IActionResult EditSite(SiteDTO newInfos)
        {
            Site? findSite = context.Sites.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findSite != null)
            {
                findSite.NomSite = newInfos.NomSite;

                context.Sites.Update(findSite);
                if (context.SaveChanges() > 0)
                {
                    List<Site> Sites = context.Sites.ToList();
                    return View("Index", Sites);
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
                    Message = "Aucun site n'a été trouvé avec cet ID !"
                });
            }
        }

        public IActionResult DeleteSite(int id)
        {
            Site? findSite = context.Sites.FirstOrDefault(x => x.Id == id);
            Salarie? findSalarie = context.Salaries.FirstOrDefault(x => x.Site.Id == id);
            if (findSite == null)
            {
                return NotFound(new
                {
                    Message = "Aucun site trouvé avec cet ID !"
                });
            }
            else if (findSalarie != null)
            {
                return BadRequest(new
                {
                    Message = "Des salariés sont associés à ce site, suppression impossible..."
                });
            }
            else
            {
                context.Sites.Remove(findSite);
                if (context.SaveChanges() > 0)
                {
                    List<Site> Sites = context.Sites.ToList();
                    return View("Index", Sites);
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
