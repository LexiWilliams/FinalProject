using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinalProject_Recipes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FinalProject_Recipes.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        public FavoritesController(FinalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _apiKey = _configuration.GetSection("AppConfiguration")["RecipeAPIKey"];

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToFavorites(Meal item)
        {

            FavoriteRecipes newFavorite = new FavoriteRecipes();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            newFavorite.UserId = thisUser.Id;
            newFavorite.RecipeId = item.idMeal;
            if (ModelState.IsValid)
            {
                _context.FavoriteRecipes.Add(newFavorite);
                _context.SaveChanges();
                return RedirectToAction("DisplayFavorite");
            }
            return View(item);
        }
        public IActionResult DisplayFavorite()
        {

            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<FavoriteRecipes> userFavorites = _context.FavoriteRecipes.Where(u => u.UserId == thisUser.Id).ToList();
            List<Meal> favoriteList = new List<Meal>();

            foreach (var item in userFavorites)
            {

                var foundMeal = FindFavRecipesById(item.RecipeId).Result;
                favoriteList.Add(foundMeal);
            }
            return View(favoriteList);
        }

        public IActionResult RemoveFavorite(Meal meal)
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<FavoriteRecipes> userFavorites = _context.FavoriteRecipes.Where(u => u.UserId == thisUser.Id).ToList();
            FavoriteRecipes recipeToDelete = new FavoriteRecipes();
            foreach (var item in userFavorites)
            {
                if (item.RecipeId == meal.idMeal)
                {
                    recipeToDelete = item;
                    break;
                }
            }
            _context.FavoriteRecipes.Remove(recipeToDelete);
            _context.SaveChanges();
            return RedirectToAction("DisplayFavorite");
        }
        public async Task<Meal> FindFavRecipesById(string mealId)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={mealId}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            var meal = viewMeal.meals[0];
            return meal;
        }
        // need to move this to another class to be pulled
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.themealdb.com");
            return client;
        }
    }
}