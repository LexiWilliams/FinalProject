
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_Recipes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_Recipes.Controllers
{
    public class FriendController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;


        public FriendController(FinalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var user = from s in _context.AspNetUsers select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                user = user.Where(s => s.UserName.Contains(searchString));

            }

            switch (sortOrder)
            {
                case "name_desc":
                    user = user.OrderByDescending(s => s.UserName);
                    break;

                default:
                    user = user.OrderBy(s => s.UserName);
                    break;
            }

            return View(await user.AsNoTracking().ToListAsync());

        }

        public IActionResult AddFriend(AspNetUsers user)
        {

            Friends newFriend = new Friends();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();

            newFriend.UserId = thisUser.Id;
            newFriend.FriendId = user.Id;
            if (ModelState.IsValid)
            {
                _context.Friends.Add(newFriend);
                _context.SaveChanges();
                return RedirectToAction("DisplayFriend");
            }
            return View(user);
        }

        //public IActionResult AddToFavorites(Meal item)
        //{

        //    FavoriteRecipes newFavorite = new FavoriteRecipes();
        //    AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
        //    newFavorite.UserId = thisUser.Id;
        //    newFavorite.RecipeId = item.idMeal;
        //    if (ModelState.IsValid)
        //    {
        //        _context.FavoriteRecipes.Add(newFavorite);
        //        _context.SaveChanges();
        //        return RedirectToAction("DisplayFavorite");
        //    }
        //    return View(item);
        //}


    }

}


