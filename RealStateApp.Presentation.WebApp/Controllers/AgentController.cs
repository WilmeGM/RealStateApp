using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Agent;
using RealStateApp.Core.Application.ViewModels.Property;
using System.Security.Claims;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.ViewModels.ChatMessage;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Services;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController(IAgentService agentService,
        IPropertyService propertyService,
        IPropertyTypeService propertyTypeService,
        ISaleTypeService saleTypeService,
        IImprovementService improvementService,
        IChatMessageService chatMessageService,
        IOfferService offerService) : Controller
    {
        private readonly IAgentService _agentService = agentService;
        private readonly IPropertyService _propertyService = propertyService;
        private readonly IPropertyTypeService _propertyTypeService = propertyTypeService;
        private readonly ISaleTypeService _saleTypeService = saleTypeService;
        private readonly IImprovementService _improvementService = improvementService;
        private readonly IChatMessageService _chatMessageService = chatMessageService;
        private readonly IOfferService _offerService = offerService;

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await _propertyService.GetPropertiesByAgentIdAsync(GetLoggedUserId()));

        [HttpGet]
        public async Task<IActionResult> PropertyMaintenance()
            => View(await _propertyService.GetAvailablePropertiesByAgentIdAsync(GetLoggedUserId()));

        [HttpGet]
        public async Task<IActionResult> MyProfile()
            => View(await _agentService.GetAgentUpdateViewModelByIdAsync(GetLoggedUserId()));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAgent(UpdateAgentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var existingAgent = await _agentService.GetAgentUpdateViewModelByIdAsync(model.Id);
                model.PhotoUrl = existingAgent.PhotoUrl;
                return View("MyProfile", model);
            }

            try
            {
                var existingAgent = await _agentService.GetAgentUpdateViewModelByIdAsync(model.Id);
                model.PhotoUrl = FileUploaderHelper.UploadImage(model.Photo, model.Id, true, existingAgent.PhotoUrl);
                await _agentService.UpdateAgentAsync(model);
                return RedirectToAction("Index", "Agent");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var existingAgent = await _agentService.GetAgentUpdateViewModelByIdAsync(model.Id);
                model.PhotoUrl = existingAgent.PhotoUrl;
                return View("MyProfile", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateProperty()
        {
            ViewBag.PropertyTypes = (await _propertyTypeService.GetAllViewModelAsync())
                .Select(pt => new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name
                }).ToList();

            ViewBag.SaleTypes = (await _saleTypeService.GetAllViewModelAsync())
                .Select(st => new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = st.Name
                }).ToList();

            ViewBag.Improvements = (await _improvementService.GetAllViewModelAsync())
                .Select(im => new SelectListItem
                {
                    Value = im.Id.ToString(),
                    Text = im.Name
                }).ToList();

            return View(new SavePropertyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProperty(SavePropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBags();
                return View(model);
            }

            if (model.Images == null || model.Images.Count == 0)
            {
                ModelState.AddModelError("", "At least one image is required.");
                await PopulateViewBags();
                return View(model);
            }

            model.UserId = GetLoggedUserId();

            try
            {
                await _propertyService.AddAsync(model);
                return RedirectToAction("PropertyMaintenance");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await PopulateViewBags();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProperty(int id)
        {
            var property = await _propertyService.GetByIdSaveViewModelAsync(id);
            if (property == null) return RedirectToAction("PropertyMaintenance");

            await PopulateViewBags();
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(SavePropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBags();
                return View(model);
            }

            try
            {
                await _propertyService.UpdateAsync(model, model.Id);
                return RedirectToAction("PropertyMaintenance");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await PopulateViewBags();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePropertyConfirmed(int id)
        {
            await _propertyService.DeleteAsync(id);
            return RedirectToAction("PropertyMaintenance");
        }

        [HttpGet]
        public async Task<IActionResult> PropertyDetails(int id)
        {
            var property = await _propertyService.GetPropertyViewModelByIdAsync(id);

            if (property == null)
                return RedirectToAction("Index");

            // Obtener clientes que han enviado mensajes
            var clientIds = await _chatMessageService.GetClientsForAgentAsync(GetLoggedUserId());
            var clients = new List<UserResponse>();
            foreach (var clientId in clientIds.Distinct()) // Evita duplicados
            {
                var client = await _agentService.GetClientByIdAsync(clientId);
                if (client != null)
                {
                    clients.Add(client);
                }
            }
            ViewBag.Clients = clients;

            // Obtener clientes que han hecho ofertas
            var clientIdsWhoOffered = await _offerService.GetClientsWithOffersByPropertyAsync(id);
            var clientsWhoOffered = new List<UserResponse>();
            foreach (var clientId in clientIdsWhoOffered.Distinct()) // Evita duplicados
            {
                var client = await _agentService.GetClientByIdAsync(clientId);
                if (client != null)
                {
                    clientsWhoOffered.Add(client);
                }
            }
            ViewBag.ClientsWhoOffered = clientsWhoOffered;

            return View(property);
        }


        [HttpGet]
        public async Task<IActionResult> OffersByClient(string clientId, int propertyId)
        {
            var offers = await _offerService.GetOffersByPropertyAndUserAsync(propertyId, clientId);

            ViewBag.ClientId = clientId;
            ViewBag.PropertyId = propertyId;

            return View(offers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespondToOffer(int offerId, string response, int propertyId)
        {
            if (response == "accept")
            {
                await _offerService.AcceptOfferAsync(offerId, propertyId);
            }
            else if (response == "reject")
            {
                await _offerService.RejectOfferAsync(offerId);
            }

            return RedirectToAction("OffersByClient", new { propertyId, clientId = (await _offerService.GetClientIdByOfferAsync(offerId)) });
        }

        [HttpGet]
        public async Task<IActionResult> ChatWithClient(string clientId)
        {
            var userId = GetLoggedUserId();
            var messages = await _chatMessageService.GetMessagesBetweenUsersAsync(clientId, userId);

            var viewModel = new AgentChatViewModel
            {
                ClientId = clientId,
                Messages = messages.Select(m => new ChatMessageViewModel
                {
                    SenderId = m.SenderId,
                    Message = m.Message,
                    IsSender = m.SenderId == userId,
                    CreatedAt = m.CreatedAt
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(string clientId, string message)
        {
            var userId = GetLoggedUserId();

            var chatMessage = new ChatMessage
            {
                SenderId = userId,
                ReceiverId = clientId,
                Message = message,
                IsRead = false
            };

            await _chatMessageService.SendMessageAsync(chatMessage);
            return RedirectToAction("ChatWithClient", new { clientId });
        }

        #region Private Methods
        private string GetLoggedUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private async Task PopulateViewBags()
        {
            ViewBag.PropertyTypes = (await _propertyTypeService.GetAllViewModelAsync())
                .Select(pt => new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name
                }).ToList();

            ViewBag.SaleTypes = (await _saleTypeService.GetAllViewModelAsync())
                .Select(st => new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = st.Name
                }).ToList();

            ViewBag.Improvements = (await _improvementService.GetAllViewModelAsync())
                .Select(im => new SelectListItem
                {
                    Value = im.Id.ToString(),
                    Text = im.Name
                }).ToList();
        }
        #endregion
    }
}
