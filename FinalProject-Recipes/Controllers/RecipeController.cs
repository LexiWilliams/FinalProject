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
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.themealdb.com");
            return client;
        }
        public async Task<Recipe> GetFlights()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/random.php");
            var meal = await response.Content.ReadAsAsync<Recipe>();
            return meal;
        }
        public IActionResult Index()
        {
            var meal = GetFlights().Result;
            return View(meal);
        }
        public async Task<IngredientList> GetIngredients()
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/1/list.php?i=list");
            var ingredients = await response.Content.ReadAsAsync<IngredientList>();
            return ingredients;
        }
            //public async Task<IngredientList> GetIngredients()
            //{
            //    var client = GetHttpClient();

            //    var response = await client.GetAsync($"api/json/v1/1/list.php?i=list");
            //    var ingredients = await response.Content.ReadAsAsync<IngredientList>();
            //    return ingredients;
            //}
            //public IActionResult SaveIngredients(IngredientList ingredientList)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        foreach (var x in ingredientList.ingredients)
            //        {
            //            Ingredients ingredient = new Ingredients();
            //            ingredient.IdIngredient = x.idIngredient;
            //            ingredient.StrIngredient = x.strIngredient;
            //            ingredient.StrType = x.strType;
            //            _context.Ingredients.Add(ingredient);
            //        }
            //        _context.SaveChanges();
            //        return View("Recipe");
            //    }
            //    return View("Recipe");
            //}
            //public async Task<IActionResult> Recipe()
            //{
            //   var ins = await GetIngredients();
            //    SaveIngredients(ins);
            //    return View();
            //}

        }
}