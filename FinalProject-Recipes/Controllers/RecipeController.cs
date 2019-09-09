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
    public class RecipeController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        public RecipeController(FinalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _apiKey = _configuration.GetSection("AppConfiguration")["RecipeAPIKey"];

        }
        public IActionResult Index(Recipe recipe)
        {
            return View(recipe);
        }
        public IActionResult Search()
        {
            return View();
        }

        public IActionResult ViewRecipe(Recipe recipes)
        {
            return View(recipes);

        }
        public IActionResult Preferences()
        {
            return View();
        }

        public async Task<IActionResult> GetRandomRecipe()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/random.php");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return View("ViewRecipe", meal);
        }
        public IActionResult FindRecipes(string search)
        {
            return View(search);
        }
        // for ingredients
        public async Task<IActionResult> SearchRecipesIngredients(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?i={search}");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return View("FindRecipes", meal);
        }
        public async Task<IActionResult> SearchRecipesTitle(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return View("FindRecipes", meal);
        }
        // for Area
        public async Task<IActionResult> SearchRecipesArea(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?a={search}");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return View("FindRecipes", meal);
        }
        // for Category
        public async Task<IActionResult> SearchRecipesCategory(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c={search}");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return View("FindRecipes", meal);
        }
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.themealdb.com");
            return client;
        }


        public IActionResult AddToFavorites(Meal meal)
        {

            FavoriteRecipes newFavorite = new FavoriteRecipes();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            newFavorite.UserId = thisUser.Id; 
            newFavorite.RecipeId = meal.strMeal; 
            if (ModelState.IsValid)
            {
                _context.FavoriteRecipes.Add(newFavorite);
                _context.SaveChanges();
                return RedirectToAction("ViewRecipe", meal);
            }
            return View(meal);
        }

        public IActionResult DisplayFavorite()
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<FavoriteRecipes> userFavorites = _context.FavoriteRecipes.Where(u => u.UserId == thisUser.Id).ToList();
            List<Meal> favoriteList = new List<Meal>();

            foreach (var meal in userFavorites) 
            {
                string mealName = meal.RecipeId;
                Meal foundMeal = GetRecipeByName(mealName).Result; 
                favoriteList.Add(foundMeal); 
            }
            return View(favoriteList);
        }

        public async Task<Meal> GetRecipeByName(string meal)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={meal}");
            var name = await response.Content.ReadAsAsync<Meal>();
            return name;
        }

        //public async Task<Recipe> FindRecipesById(Meal meal)
        //{
        //    string search = meal.idMeal;
        //    var client = GetHttpClient();

        //    var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={search}");
        //    var viewMeal = await response.Content.ReadAsAsync<Recipe>();
        //    return viewMeal;
        //}
    }
}