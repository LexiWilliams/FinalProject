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
            //if (User.Identity.IsAuthenticated)
            //{
            //    if(User.Identity.Name != null)
            //    {
            //        return RedirectToAction("UserPage", "User");
            //    }
            //}
            ////Recipe filteredRecipes = new Recipe();
            ////bool notNull = false;
            ////while (notNull == false)
            ////{
            ////    var client = RecipeMethods.GetHttpClient();
            ////    var response = await client.GetAsync($"api/json/v1/{_apiKey}/random.php");
            ////    var recipes = await response.Content.ReadAsAsync<Recipe>();
            ////    filteredRecipes = FilterRecipes(recipes);
            ////    if (filteredRecipes.meals[0] != null)
            ////    {
            ////        notNull = true;
            ////    }
            ////}
            ////return View(filteredRecipes);
            //return View();
            return RedirectToAction("GetRandomRecipe", "Recipe");
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
