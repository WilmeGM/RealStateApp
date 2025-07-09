using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.ChatMessage;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Entities;
using System.Security.Claims;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController(IPropertyService propertyService,
        IFavoritePropertyService favoritePropertyService,
        IChatMessageService chatMessageService,
        IOfferService offerService,
        IAgentService agentService) : Controller
    {
        private readonly IPropertyService _propertyService = propertyService;
        private readonly IFavoritePropertyService _favoritePropertyService = favoritePropertyService;
        private readonly IChatMessageService _chatMessageService = chatMessageService;
        private readonly IOfferService _offerService = offerService;
        private readonly IAgentService _agentService = agentService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAvailablePropertiesAsync();
            var userId = GetLoggedUserId();

            // Marcar las propiedades como favoritas si están en la lista del usuario
            var favoritePropertyIds = await _favoritePropertyService.GetFavoritePropertyIdsByUserIdAsync(userId);
            foreach (var property in properties)
            {
                property.IsFavorite = favoritePropertyIds.Contains(property.Id);
            }

            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> FavoritesProperties()
        {
            var properties = await _propertyService.GetAvailablePropertiesAsync();
            var userId = GetLoggedUserId();

            // Marcar las propiedades como favoritas si están en la lista del usuario
            var favoritePropertyIds = await _favoritePropertyService.GetFavoritePropertyIdsByUserIdAsync(userId);
            foreach (var property in properties)
            {
                property.IsFavorite = favoritePropertyIds.Contains(property.Id);
            }

            var favoritesProperties = new List<PropertyViewModel>();

            foreach(var favoriteProperty in properties)
            {
                if(favoriteProperty.IsFavorite)
                {
                    favoritesProperties.Add(favoriteProperty);
                }
            }

            return View(favoritesProperties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int propertyId)
        {
            var userId = GetLoggedUserId();
            await _favoritePropertyService.ToggleFavoriteAsync(userId, propertyId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ClientPropertyDetail(int id)
        {
            var property = await _propertyService.GetPropertyViewModelByIdAsync(id);

            var agent = await _agentService.GetAgentByIdAsync(property.UserId);
            ViewBag.Agent = agent;

            var userId = GetLoggedUserId();
            var agentId = property.UserId;

            var offers = await _offerService.GetOffersByPropertyAndUserAsync(id, userId);
            var hasActiveOffer = await _offerService.HasApprovedOrPendingOfferAsync(id, userId);

            var messages = await _chatMessageService.GetMessagesAsync(id, userId, agentId);

            var viewModel = new PropertyDetailViewModel
            {
                Property = property,
                Messages = messages.Select(m => new ChatMessageViewModel
                {
                    SenderId = m.SenderId,
                    Message = m.Message,
                    IsSender = m.SenderId == userId,
                    CreatedAt = m.CreatedAt
                }).ToList(),
                Offers = offers,
                CanMakeOffer = !hasActiveOffer
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int propertyId, string message)
        {
            var userId = GetLoggedUserId();
            var property = await _propertyService.GetPropertyViewModelByIdAsync(propertyId);

            var chatMessage = new ChatMessage
            {
                SenderId = userId,
                ReceiverId = property.UserId,
                PropertyId = propertyId,
                Message = message,
                IsRead = false
            };

            await _chatMessageService.SendMessageAsync(chatMessage);
            return RedirectToAction("ClientPropertyDetail", new { id = propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOffer(int propertyId, decimal amount)
        {
            var userId = GetLoggedUserId();

            var offer = new CreateOfferViewModel
            {
                PropertyId = propertyId,
                UserId = userId,
                Amount = amount
            };

            await _offerService.CreateOfferAsync(offer);
            return RedirectToAction("ClientPropertyDetail", new { id = propertyId });
        }

        #region Private Methods
        private string GetLoggedUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);
        #endregion
    }
}
