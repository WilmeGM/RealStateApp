using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.User;
using System.Security.Claims;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IImprovementService _improvementService;
        private readonly IAgentService _agentService;
        private readonly IPropertyService _propertyService;

        public AdminController(IAgentService agentService,
            IUserService userService,
            IAccountService accountService,
            IPropertyTypeService propertyTypeService,
            ISaleTypeService saleTypeService,
            IImprovementService improvementService,
            IPropertyService propertyService)
        {
            _userService = userService;
            _accountService = accountService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
            _improvementService = improvementService;
            _agentService = agentService;
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.ActiveAgentsCount = await _accountService.GetAllActiveUsersCountByRoleAsync(Roles.Agent.ToString());
            ViewBag.InactiveAgentsCount = await _accountService.GetAllInactiveUsersCountByRoleAsync(Roles.Agent.ToString());

            ViewBag.ActiveClientsCount = await _accountService.GetAllActiveUsersCountByRoleAsync(Roles.Client.ToString());
            ViewBag.InactiveClintsCount = await _accountService.GetAllInactiveUsersCountByRoleAsync(Roles.Client.ToString());

            ViewBag.ActiveDevlopersCount = await _accountService.GetAllActiveUsersCountByRoleAsync(Roles.Developer.ToString());
            ViewBag.InactiveDevelopersCount = await _accountService.GetAllInactiveUsersCountByRoleAsync(Roles.Developer.ToString());

            var propertyCounts = await _propertyService.GetPropertyStatusCountsAsync();
            ViewBag.AvailablePropertiesCount = propertyCounts.Available;
            ViewBag.SoldPropertiesCount = propertyCounts.Sold;

            return View();
        }

        public async Task<IActionResult> AdminList()
        {
            var adminList = await _userService.GetAllAdminsAsync();
            return View(adminList);
        }

        public IActionResult AddAdmin()
        {
            return View(new SaveAdminViewModel());
        }

        public async Task<IActionResult> UpdateAdmin(string adminId)
        {
            var admin = await _userService.GetUpdateAdminByIdAsync(adminId);
            return View(admin);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(UpdateAdminViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _userService.UpdateAdminUserAsync(vm);
            if (result.HasError)
            {
                vm.HasError = true;
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }
            return RedirectToAction("AdminList");
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(SaveAdminViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var result = await _userService.CreateAdminUserAsync(vm);
            if (result.HasError)
            {
                vm.HasError = true;
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }
            return RedirectToAction("AdminList");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string adminId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == adminId)
            {
                TempData["ErrorMessage"] = "you cannot activate or deactivate yourself";
                return RedirectToAction("AdminList");
            }
            var result = await _accountService.UpdateAdminStatus(adminId);
            if (result == true)
            {
                return RedirectToAction("AdminList");
            }
            return RedirectToRoute(new { controller = "Error", action = "Error" });

        }

        //Metodos de Developers//


        public async Task<IActionResult> DevelopersList()
        {
            var developersList = await _userService.GetAllDevelopersAsync();
            return View(developersList);
        }
        public IActionResult AddDevelopers()
        {
            return View(new SaveDevelopersViewModel());
        }

        public async Task<IActionResult> UpdateDevelopers(string developersId)
        {
            var developers = await _userService.GetUpdateDevelopersByIdAsync(developersId);
            return View(developers);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateDevelopers(UpdateDevelopersViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _userService.UpdateDevelopersUserAsync(vm);
            if (result.HasError)
            {
                vm.HasError = true;
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }
            return RedirectToAction("DevelopersList");
        }

        [HttpPost]
        public async Task<IActionResult> AddDevelopers(SaveDevelopersViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var result = await _userService.CreateDevelopersUserAsync(vm);
            if (result.HasError)
            {
                vm.HasError = true;
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }
            return RedirectToAction("DevelopersList");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatu(string developersId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == developersId)
            {
                TempData["ErrorMessage"] = "you cannot activate or deactivate yourself";
                return RedirectToAction("DevelopersList");
            }
            var result = await _accountService.UpdateDevelopersStatus(developersId);
            if (result == true)
            {
                return RedirectToAction("DevelopersList");
            }
            return RedirectToRoute(new { controller = "Error", action = "Error" });

        }

        //Metodos de PropertyType//

        public async Task<IActionResult> PropertyTypeList()
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModelAsync();
            return View(propertyTypes); // Devuelve la vista con la lista de PropertyType
        }

        // Muestra el formulario para agregar un nuevo tipo de propiedad
        public IActionResult AddPropertyType()
        {
            return View(new SavePropertyTypeViewModel());
        }

        // Crea un nuevo tipo de propiedad
        [HttpPost]
        public async Task<IActionResult> AddPropertyType(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _propertyTypeService.AddAsync(vm);
            if (result.HasError)
            {
                vm.HasError = true;
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }
            return RedirectToAction("PropertyTypeList"); // Redirige a la lista después de agregar el tipo de propiedad
        }

        // Muestra la vista para actualizar un tipo de propiedad
        public async Task<IActionResult> UpdatePropertyType(int id)
        {
            var propertyType = await _propertyTypeService.GetByIdSaveViewModelAsync(id);
            if (propertyType == null)
            {
                return RedirectToAction("PropertyTypeList");
            }
            return View(propertyType); // Devuelve la vista con los detalles del tipo de propiedad
        }

        // Actualiza un tipo de propiedad
        [HttpPost]
        public async Task<IActionResult> UpdatePropertyType(UpdatePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _propertyTypeService.UpdateAsync(vm, vm.Id);

            return RedirectToAction("PropertyTypeList"); // Redirige a la lista de tipos de propiedad después de la actualización
        }

        // Elimina un tipo de propiedad
        [HttpPost]
        public async Task<IActionResult> DeletePropertyType(int id)
        {
            await _propertyTypeService.DeleteAsync(id);
            return RedirectToAction("PropertyTypeList"); // Redirige a la lista después de eliminar el tipo de propiedad
        }





        //Metodos de SaleType//
        public async Task<IActionResult> SaleTypeList()
        {
            var saleTypes = await _saleTypeService.GetAllViewModelAsync();
            return View(saleTypes);
        }


        public IActionResult AddSaleType()
        {
            return View(new SaveSaleTypeViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> AddSaleType(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _saleTypeService.AddAsync(vm);
            if (result.HasError)
            {
                vm.HasError = true;
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }
            return RedirectToAction("SaleTypeList");
        }


        public async Task<IActionResult> UpdateSaleType(int id)
        {
            var saleTypes = await _saleTypeService.GetByIdSaveViewModelAsync(id);
            if (saleTypes == null)
            {
                return RedirectToAction("SaleTypeList");
            }
            return View(saleTypes);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSaleType(UpdateSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _saleTypeService.UpdateAsync(vm, vm.Id);

            return RedirectToAction("SaleTypeList");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSaleType(int id)
        {
            await _saleTypeService.DeleteAsync(id);
            return RedirectToAction("SaleTypeList");
        }


        //Metodos de Improvement//



        public async Task<IActionResult> ImprovementList()
        {
            var improvements = await _improvementService.GetAllViewModelAsync();
            return View(improvements);
        }


        public IActionResult AddImprovement()
        {
            return View(new SaveImprovementViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> AddImprovement(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _improvementService.AddAsync(vm);
            if (result.HasError)
            {
                vm.HasError = true;
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }
            return RedirectToAction("ImprovementList");
        }


        public async Task<IActionResult> UpdateImprovement(int id)
        {
            var improvements = await _improvementService.GetByIdSaveViewModelAsync(id);
            if (improvements == null)
            {
                return RedirectToAction("ImprovementList");
            }
            return View(improvements);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateImprovement(UpdateImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _improvementService.UpdateAsync(vm, vm.Id);

            return RedirectToAction("ImprovementList");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteImprovement(int id)
        {
            await _improvementService.DeleteAsync(id);
            return RedirectToAction("ImprovementList");
        }

        [HttpGet]
        public async Task<IActionResult> AgentList()
        {
            return View(await _agentService.GetAllAgentsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAgentStatus(string agentId)
        {
            var result = await _accountService.UpdateAdminStatus(agentId);
            if (result == true)
            {
                return RedirectToAction("AgentList");
            }
            return RedirectToRoute(new { controller = "Error", action = "Error" });

        }
    }
}
