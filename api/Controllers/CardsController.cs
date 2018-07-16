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
        public JsonResult Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cards = _context.Cards.Include(x => x.CardItem).Where(x => x.UserId == userId);

            //TODO: Mapster
            var model = cards.Select(x => new InventoryItemViewModel
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

        //TODO: Create que task
        //[HttpGet]
        //public JsonResult Get()
        //{
        //    //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var userId = "auth0|5b44794e6d829f101860278c";

        //    var allx = _cardService.All();

        //    for (int i = 308; i < 369; i++)
        //    {
        //        var cards = _cardService.Where(x => x.Page, i).All();
        //        foreach (var card in cards.Value)
        //        {
        //            if (_context.CardItems.Local.FirstOrDefault(x => x.Id == card.Id) != null)
        //                continue;

        //            if (_context.CardItems.FirstOrDefault(x=>x.Id == card.Id) != null)
        //                continue;

        //            var carditem = new CardItem
        //            {
        //                Id = card.Id,
        //                Name = card.Name,
        //                SetName = card.SetName,
        //                ImageUrl = card.ImageUrl != null ? card.ImageUrl.ToString() : null,
        //                ManaCost = card.ManaCost
        //            };

        //            _context.CardItems.Add(carditem);
        //        }

        //        _context.SaveChanges();
        //    }

        //    //var cardsIds = _context.Cards.Where(x => x.UserId == userId);
        //    //var list = new List<Card>();

        //    //foreach (var cardsId in cardsIds)
        //    //{
        //    //    var cardx = _cardService.Find(cardsId.Id);
        //    //    list.Add(cardx.Value);
        //    //}

        //    return Json("");
        //}

        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var card = _context.Cards.Include(x => x.CardItem).FirstOrDefault(x => x.UserId == userId && x.Id == id);
            return Json(card);
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

            var card = _context.Cards.Include(x => x.CardItem).FirstOrDefault(x => x.UserId == userId && x.Id == id);
            if (card != null)
                card.Quantity++;
            else
            {
                card = new Data.Models.Card
                {
                    Id = id,
                    Quantity = 1,
                    UserId = userId
                };

                _context.Add(card);
            }

            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CheckAvailability(id);

            var card = _context.Cards.FirstOrDefault(x => x.UserId == userId && x.Id == id);
            if (card == null)
                return;

            card.Quantity--;
            if (card.Quantity <= 0)
                _context.Remove(card);

            _context.SaveChanges();
        }
    }
}
