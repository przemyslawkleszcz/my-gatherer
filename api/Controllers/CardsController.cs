using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using my_gatherer_api.Data;
using my_gatherer_api.Data.Models;
using my_gatherer_api.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MtgApiManager.Lib.Core;
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;

namespace my_gatherer_api.Controllers
{
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly MyGathererApiDbContext _context;
        private readonly CardService _cardService;

        public CardsController(MyGathererApiDbContext context, CardService cardService)
        {
            _context = context;
            _cardService = cardService;
        }

        [HttpGet]
        public JsonResult Get(string set)
        {
            var service = new SetService();
            var setData = service.Find(set);
            var cards = _context.CardItems.Where(x => x.SetName == setData.Value.Name);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var inventoryItems = _context.InventoryItems.Where(x => x.UserId == userId);
                var data = from c in cards
                            join i in inventoryItems on c.Id equals i.Id into ci
                            from pos in ci.DefaultIfEmpty()
                            select new CardItemViewData
                            {
                                Id = c.Id,
                                Name = c.Name,
                                ManaCost = c.ManaCost,
                                ImageUrl = c.ImageUrl,
                                InInventory = pos != null
                            };

                var model = new CardItemViewModel {Items = data};
                return Json(model);
            }
            else
            {
                var data = cards.ProjectToType<CardItemViewData>();
                var model = new CardItemViewModel { Items = data };
                return Json(model);
            }
        }
    }
}
