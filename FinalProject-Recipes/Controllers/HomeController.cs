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
            Meal[] meals = new Meal[16];

            for (int i = 0; i <= 7; i++)
            {
            var recipes = GetRandomRecipe().Result;
            meal1 = recipes.meals[0];
            meals[i] = meal1;
            }
            List<string> septemberMeals = new List<string>{ "52857", "52845", "52914","52859","52812","52814","52968","52893"};
            int count = 0;
            for(int i = 8; i <= 15; i++) 
            {
            var meal2 = FindFavRecipesById(septemberMeals[count]).Result;
                count++;
                meals[i] = meal2;
            }
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
