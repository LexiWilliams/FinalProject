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

 
    public class BrowseController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        public BrowseController(FinalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _apiKey = _configuration.GetSection("AppConfiguration")["RecipeAPIKey"];

        }

        public IActionResult Index()
        {
            return View();
        }
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.themealdb.com");
            return client;
        }

        public async Task<IActionResult> Beef()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Beef");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Breakfast()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Breakfast");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Chicken()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Chicken");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Goat()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Goat");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Lamb()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Lamb");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Pasta()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Pasta");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Pork()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Pork");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes",meal);
        }
        public async Task<IActionResult> Seafood()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Seafood");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Side()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Side");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }
        public async Task<IActionResult> Starter()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c=Starter");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return RedirectToAction("ViewRecipes", meal);
        }

    }
}