using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinalProject_Recipes.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace FinalProject_Recipes.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetSection("AppConfiguration")["RecipeAPIKey"];

        }
        public IActionResult Index()
        {
            Recipe displayRecipes = new Recipe();
            Meal meal1 = new Meal();
            var recipes = GetRandomRecipe().Result;
            meal1 = recipes.meals[0];

            string mealOfTheMonth = "52857";
            var meal2 = FindFavRecipesById(mealOfTheMonth).Result;

            Meal[] meals = new Meal[2];
            meals[0] = meal1;
            meals[1] = meal2;
            displayRecipes.meals = meals;

            return View(displayRecipes);
        }
        public async Task<Recipe> GetRandomRecipe()
        {
                var client = RecipeMethods.GetHttpClient();
                var response = await client.GetAsync($"api/json/v1/{_apiKey}/random.php");
                var recipes = await response.Content.ReadAsAsync<Recipe>();
                return recipes;
        }
        public async Task<Meal> FindFavRecipesById(string mealId)
        {
            var client = RecipeMethods.GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={mealId}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            var meal = viewMeal.meals[0];
            return meal;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
