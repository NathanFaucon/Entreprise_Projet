using Entreprise_Projet.Datas;
using Entreprise_Projet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Entreprise_Projet.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext context;

        public ServiceController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<Service> Services = context.Services.ToList();
            return View(Services);
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            Service Service = GetServiceById(id);
            return View(Service);
        }
        public IActionResult Filter(string searchString)
        {
            var services = context.Services.ToList();
            if (string.IsNullOrEmpty(searchString)) return View("Index", services);

            var filteredList = services.Where(x => x.NomService.ToUpper().Contains(searchString.ToUpper())).ToList();

            return View("Index", filteredList);
        }

        [HttpGet("services")]
        public IActionResult GetServices()
        {
            List<Service> myServices = context.Services.ToList();

            if (myServices.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Services:",
                    Service = myServices
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun service dans la base de données !"
                });
            }
        }
        [HttpGet("services/{serviceId}")]
        public IActionResult GetServicesById(int serviceId)
        {
            Service? findService = context.Services.FirstOrDefault(x => x.Id == serviceId);

            if (findService == null)
            {
                return NotFound(new
                {
                    Message = "Aucun service trouvé avec cet ID !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Service trouvé !",
                    Service = new ServiceDTO() { Id = findService.Id, NomService = findService.NomService }
                });
            }
        }

        public Service GetServiceById(int serviceId)
        {
            return context.Services.FirstOrDefault(x => x.Id == serviceId);
        }

        [HttpPost("services")]
        public IActionResult AddServices(ServiceDTO newService)
        {
            Service addService = new Service()
            {
                NomService = newService.NomService,
            };
            context.Services.Add(addService);
            if (context.SaveChanges() > 0)
            {
                List<Service> Services = context.Services.ToList();
                return View("Index", Services);
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }

        public IActionResult EditService(ServiceDTO newInfos)
        {
            Service? findService = context.Services.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findService != null)
            {
                findService.NomService = newInfos.NomService;

                context.Services.Update(findService);
                if (context.SaveChanges() > 0)
                {
                    List<Service> Services = context.Services.ToList();
                    return View("Index", Services);
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
                    Message = "Aucun service n'a été trouvé avec cet ID !"
                });
            }
        }

        public IActionResult DeleteService(int id)
        {
            Service? findService = context.Services.FirstOrDefault(x => x.Id == id);
            Salarie? findSalarie = context.Salaries.FirstOrDefault(x => x.Service.Id == id);
            if (findService == null)
            {
                return NotFound(new
                {
                    Message = "Aucun service trouvé avec cet ID !"
                });
            }else if (findSalarie != null)
            {
                return BadRequest(new
                {
                    Message = "Des salariés sont associés à ce service, suppression impossible..."
                });
            }
            else
            {
                context.Services.Remove(findService);
                if (context.SaveChanges() > 0)
                {
                    List<Service> Services = context.Services.ToList();
                    return View("Index", Services);
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
