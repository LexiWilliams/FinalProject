using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FinalProject_Recipes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FinalProject_Recipes.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> ViewRecipe(FavoriteRecipes myRecipe)
        {
            var meal = await FindFavRecipesById(myRecipe.RecipeId);
            var recipe = FindRecipesById(meal).Result;
            return View(recipe);
        }
        public async Task<Recipe> FindRecipesById(Meal meal)
        {
            string search = meal.idMeal;
            var client = RecipeMethods.GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={search}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            return viewMeal;
        }
        public IActionResult AddToFavorites(Meal item)
        {
            
            FavoriteRecipes newFavorite = new FavoriteRecipes();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();

            var favList = _context.FavoriteRecipes.Where(u => u.UserId == thisUser.Id).ToList();
            List<string> mealIdList = new List<string>();
            foreach (var meal in favList)
            {
                mealIdList.Add(meal.RecipeId);
            }

            if (mealIdList.Contains(item.idMeal))
            {
                return View();
            }
            else
            {
                newFavorite.UserId = thisUser.Id;
                newFavorite.RecipeId = item.idMeal;
                newFavorite.RecipeImage = item.strMealThumb;
                newFavorite.RecipeName = item.strMeal;
                if (ModelState.IsValid)
                {
                    _context.FavoriteRecipes.Add(newFavorite);
                    _context.SaveChanges();
                    return RedirectToAction("DisplayFavorite");
                }
                return View(item);
            }


        }
        public IActionResult DisplayFavorite()
        {

            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<FavoriteRecipes> userFavorites = _context.FavoriteRecipes.Where(u => u.UserId == thisUser.Id).ToList();
            //List<Meal> favoriteList = new List<Meal>();

            //foreach (var item in userFavorites)
            //{

            //    var foundMeal = FindFavRecipesById(item.RecipeId).Result;
            //    favoriteList.Add(foundMeal);
            //}

            return View(userFavorites);
        }

        public IActionResult RemoveFavorite(FavoriteRecipes myRecipe)
        {
            _context.FavoriteRecipes.Remove(myRecipe);
            _context.SaveChanges();
            return RedirectToAction("DisplayFavorite");
        }
        public IActionResult RemoveFavoriteByMeal(Meal meal)
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
            var client = RecipeMethods.GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={mealId}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            var meal = viewMeal.meals[0];
            return meal;
        }
        // need to move this to another class to be pulled
        
        public IActionResult DisplayFriendFavorite(AspNetUsers friend)
        {

            ///AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<FavoriteRecipes> userFavorites = _context.FavoriteRecipes.Where(u => u.UserId == friend.Id).ToList();
            List<Meal> favoriteList = new List<Meal>();

            foreach (var item in userFavorites)
            {

                var foundMeal = FindFavRecipesById(item.RecipeId).Result;
                favoriteList.Add(foundMeal);
            }
            return View("DisplayFriendFavorite",favoriteList);
        }
    }
}