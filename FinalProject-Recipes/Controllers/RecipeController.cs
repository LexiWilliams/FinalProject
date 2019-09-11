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

        public IActionResult ViewRecipe(Meal meal)
        {
            var recipe = FindRecipesById(meal).Result;
            return View(recipe);

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

        public async Task<Meal> GetRecipeByName(string meal)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={meal}");
            var name = await response.Content.ReadAsAsync<Meal>();
            return name;
        }
        
        public async Task<Recipe> FindRecipesById(Meal meal)
        {
            string search = meal.idMeal;
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={search}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            return viewMeal;
        }
        public IActionResult EditPreferences(string milk, string eggs, string fish, string shellfish, string treenuts, string peanuts, string soy, string wheat, string diet)
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (milk == "milk")
            {
                thisUser.Milk = true;

            }
            if (eggs == "eggs")
            {
                thisUser.Eggs = true;

            }

            if (fish == "fish")
            {
                thisUser.Fish = true;

            }
            if (shellfish == "shellfish")
            {
                thisUser.Shellfish = true;

            }
            if (treenuts == "treenuts")
            {
                thisUser.Treenuts = true;

            }
            if (peanuts == "peanuts")
            {
                thisUser.Peanuts = true;

            }
            if (soy == "soy")
            {
                thisUser.Soy = true;

            }
            if (wheat == "wheat")
            {
                thisUser.Wheat = true;

            }

            if (diet == "none")
            {
                thisUser.Diet = null;
            }
            else
            {
                thisUser.Diet = diet;
            }


            _context.Entry(thisUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Search");

        }
        public List<DietsAndRestriction> GetDiets(string diet)
        {
            if(diet == "Vegan")
            {
            var vegan = _context.DietsAndRestriction.Where(x => x.Vegan == true).ToList();
            return vegan;
            }
            else if(diet == "Keto")
            {
                var keto = _context.DietsAndRestriction.Where(x => x.Keto == true).ToList();
                return keto;
            }
            else if (diet == "Paleo")
            {
                var paleo = _context.DietsAndRestriction.Where(x => x.Paleo == true).ToList();
                return paleo;
            }
            else if (diet == "Vegetarian")
            {
                var vegetarian = _context.DietsAndRestriction.Where(x => x.Vegetarian == true).ToList();
                return vegetarian;
            }
            else if (diet == "GlutenFree")
            {
                var glutenFree = _context.DietsAndRestriction.Where(x => x.GlutenFree == true).ToList();
                return glutenFree;
            }
            else if (diet == "Pescatarian")
            {
                var pescatarian = _context.DietsAndRestriction.Where(x => x.Pescatarian == true).ToList();
                return pescatarian;
            }
            else
            {
                List<DietsAndRestriction> none = new List<DietsAndRestriction>();
                return none;
            }
        }
        public List<string> AddIngredients(Meal item)
        {
            var ingredients = new List<string>();

            ingredients.Add(item.strIngredient1);
            ingredients.Add(item.strIngredient2);
            ingredients.Add(item.strIngredient3);
            ingredients.Add(item.strIngredient4);
            ingredients.Add(item.strIngredient5);
            ingredients.Add(item.strIngredient6);
            ingredients.Add(item.strIngredient7);
            ingredients.Add(item.strIngredient8);
            ingredients.Add(item.strIngredient9);
            ingredients.Add(item.strIngredient10);
            ingredients.Add(item.strIngredient11);
            ingredients.Add(item.strIngredient12);
            ingredients.Add(item.strIngredient13);
            ingredients.Add(item.strIngredient14);
            ingredients.Add(item.strIngredient15);
            ingredients.Add(item.strIngredient16);
            ingredients.Add(item.strIngredient17);
            ingredients.Add(item.strIngredient18);
            ingredients.Add(item.strIngredient19);
            ingredients.Add(item.strIngredient20);
            return ingredients;
        }
        public async Task<IActionResult> SearchRecipesTitle(string search)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
            var recipes = await response.Content.ReadAsAsync<Recipe>();

            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            var userDiet = thisUser.Diet;
            var diet = GetDiets(userDiet);
            int count = recipes.meals.Length;
            var filteredMeals = new Meal[count];

            int index = -1;
            if (diet != null)
            {
              
                foreach (var item in recipes.meals)
                {
                    bool isBad = false;
                    index++;
                    var ingredients = AddIngredients(item);
                    
                    foreach (var name in diet)
                    {
                        var ingred = name.Id;

                        if (ingredients.Contains(ingred))
                        {
                            isBad = true;
                        }
                    }
                    if (isBad == false)
                    {
                        filteredMeals[index] = item;
                    }
                }
            }
            Recipe filteredRecipes = new Recipe();
            filteredRecipes.meals = filteredMeals;
            return View("FindRecipes", filteredRecipes);
        }
    }
}