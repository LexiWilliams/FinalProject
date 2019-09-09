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
        public IActionResult Index()
        {
            var meal = GetRandomRecipe().Result;
            return View(meal);
        }
        public IActionResult Search()
        {
            return View();
        }
        public IActionResult FindRecipe(string search)
        {
            var meal = SearchRecipesIngredients(search).Result;
            return View(meal);
        }
        public IActionResult ViewRecipe(Meal id)
        {
            var meal = FindRecipesById(id).Result;
            return View(meal);

        }

        public IActionResult Preferences()
        {
            return View();
        }



        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.themealdb.com");
            return client;
        }
        public async Task<Recipe> GetRandomRecipe()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/random.php");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return meal;
        }
        // for ingredients
        public async Task<Recipe> SearchRecipesIngredients(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?i={search}");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return meal;
        }
        // for Area
        public async Task<Recipe> SearchRecipesArea(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?a={search}");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return meal;
        }
        // for Category
        public async Task<Recipe> SearchRecipesCategory(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c={search}");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return meal;
        }
        public async Task<Recipe> FindRecipesById(Meal meal)
        {
            string search = meal.idMeal;
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={search}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            return viewMeal;
        }
    }