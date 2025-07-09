using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class HomeController(IAgentService agentService,
        IPropertyService propertyService,
        IPropertyTypeService propertyTypeService) : Controller
    {
        private readonly IAgentService _agentService = agentService;
        private readonly IPropertyService _propertyService = propertyService;
        private readonly IPropertyTypeService _propertyTypeService = propertyTypeService;

        [HttpGet]
        public async Task<IActionResult> Index(string? searchCode, int? propertyTypeId, decimal? minPrice, decimal? maxPrice, int? bedrooms, int? bathrooms)
        {
            var properties = await _propertyService.GetAvailablePropertiesAsync();

            if (!string.IsNullOrEmpty(searchCode))
                properties = properties.Where(p => p.UniqueCode.Contains(searchCode, StringComparison.OrdinalIgnoreCase)).ToList();

            if (propertyTypeId.HasValue)
                properties = properties.Where(p => p.PropertyTypeId == propertyTypeId).ToList();

            if (minPrice.HasValue)
                properties = properties.Where(p => p.Price >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                properties = properties.Where(p => p.Price <= maxPrice.Value).ToList();

            if (bedrooms.HasValue)
                properties = properties.Where(p => p.RoomCount == bedrooms.Value).ToList();

            if (bathrooms.HasValue)
                properties = properties.Where(p => p.BathroomCount == bathrooms.Value).ToList();
            properties = properties.OrderByDescending(p => p.CreatedAt).ToList();

            ViewBag.PropertyTypes = (await _propertyTypeService.GetAllViewModelAsync())
                .Select(pt => new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name
                }).ToList();

            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> PropertyDetails(int id)
        {
            var property = await _propertyService.GetPropertyViewModelByIdAsync(id);

            if (property == null)
                return RedirectToAction("Index"); 

            var agent = await _agentService.GetAgentByIdAsync(property.UserId);

            ViewBag.Agent = agent;

            return View(property);
        }

        [HttpGet]
        public async Task<IActionResult> Agents(string? search)
        {
            var agents = await _agentService.GetAllActiveAgentsAsync();

            if (!string.IsNullOrEmpty(search))
            {
                ViewData["Search"] = search;
                agents = agents.Where(a => a.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(agents.OrderBy(a => a.FullName).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> AgentProperties(string id)
        {
            var agent = await _agentService.GetAgentByIdAsync(id);
            if (agent == null) return RedirectToAction("Agents");

            ViewData["AgentName"] = agent.FullName;
            var properties = await _propertyService.GetPropertiesByAgentIdAsync(id);

            return View(properties);
        }
    }
}
