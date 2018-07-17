using System;
using System.Linq;
using System.Security.Claims;
using my_gatherer_api.Data;
using my_gatherer_api.Data.Models;
using my_gatherer_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtgApiManager.Lib.Service;

namespace my_gatherer_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {
        private readonly MyGathererApiDbContext _context;
        private readonly CardService _cardService;

        public InventoryController(MyGathererApiDbContext context, CardService cardService)
        {
            _context = context;
            _cardService = cardService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var inventoryItem = _context.InventoryItems.Include(x => x.CardItem).Where(x => x.UserId == userId);

            var model = inventoryItem.Select(x => new InventoryItemViewModel
            {
                Id = x.Id,
                Quantity = x.Quantity,
                ManaCost = x.CardItem.ManaCost,
                SetName = x.CardItem.SetName,
                Name = x.CardItem.Name,
                ImageUrl = x.CardItem.ImageUrl
            });

            return Json(model);
        }

        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var inventoryItem = _context.InventoryItems.Include(x => x.CardItem).FirstOrDefault(x => x.UserId == userId && x.Id == id);
            return Json(inventoryItem);
        }
        private void CheckAvailability(string id)
        {
            var cardItem = _context.CardItems.FirstOrDefault(x => x.Id == id);
            if (cardItem != null)
                return;

            var item = _cardService.Find(id);
            if (item.Value == null)
                throw new Exception("Incorrect id");

            var newItem = new CardItem
            {
                Id = item.Value.Id,
                ImageUrl = item.Value.ImageUrl != null ? item.Value.ImageUrl.ToString() : null, //method
                ManaCost = item.Value.ManaCost,
                Name = item.Value.Name,
                SetName = item.Value.SetName
            };

            _context.CardItems.Add(newItem);
            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public void Put(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CheckAvailability(id);

            var inventoryItem = _context.InventoryItems.Include(x => x.CardItem).FirstOrDefault(x => x.UserId == userId && x.Id == id);
            if (inventoryItem != null)
                inventoryItem.Quantity++;
            else
            {
                inventoryItem = new Data.Models.InventoryItem
                {
                    Id = id,
                    Quantity = 1,
                    UserId = userId
                };

                _context.Add(inventoryItem);
            }

            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CheckAvailability(id);

            var inventoryItem = _context.InventoryItems.FirstOrDefault(x => x.UserId == userId && x.Id == id);
            if (inventoryItem == null)
                return;

            inventoryItem.Quantity--;
            if (inventoryItem.Quantity <= 0)
                _context.Remove(inventoryItem);

            _context.SaveChanges();
        }
    }
}
