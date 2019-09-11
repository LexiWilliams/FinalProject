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
            var recipes = await response.Content.ReadAsAsync<Recipe>();
            var filteredRecipes = FilterRecipes(recipes);

            return View("FindRecipes", filteredRecipes);
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
            var recipes = await response.Content.ReadAsAsync<Recipe>();
            var actualRecipes = GetRecipesById(recipes);
            var filteredRecipes = FilterRecipes(actualRecipes);

            return View("FindRecipes", filteredRecipes);
        }
        public Recipe GetRecipesById(Recipe recipes)
        {
            Recipe allRecipes = new Recipe();
            Meal[] meals = new Meal[recipes.meals.Length];
            int index = 0;
            foreach(var item in recipes.meals)
            {
                var recipe = FindFavRecipesById(item.idMeal);
                meals[index] = recipe.Result;
                index++;
            }
            allRecipes.meals = meals;
            return allRecipes;
        }

        public async Task<Meal> FindFavRecipesById(string mealId)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={mealId}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            var meal = viewMeal.meals[0];
            return meal;
        }

        // for Area
        public async Task<IActionResult> SearchRecipesArea(string search)
        {
            var client = GetHttpClient();

            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?a={search}");
            var recipes = await response.Content.ReadAsAsync<Recipe>();
            var filteredRecipes = FilterRecipes(recipes);

            return View("FindRecipes", filteredRecipes);
        }
        // for Category
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.themealdb.com");
            return client;
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
        public List<DietsAndRestriction> GetDiet()
        {
            if (User.Identity.Name != null)
            {
                AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
                var userDiet = thisUser.Diet;

                if (userDiet == "Vegan")
                {
                    var vegan = _context.DietsAndRestriction.Where(x => x.Vegan == true).ToList();
                    return vegan;
                }
                else if (userDiet == "Keto")
                {
                    var keto = _context.DietsAndRestriction.Where(x => x.Keto == true).ToList();
                    return keto;
                }
                else if (userDiet == "Paleo")
                {
                    var paleo = _context.DietsAndRestriction.Where(x => x.Paleo == true).ToList();
                    return paleo;
                }
                else if (userDiet == "Vegetarian")
                {
                    var vegetarian = _context.DietsAndRestriction.Where(x => x.Vegetarian == true).ToList();
                    return vegetarian;
                }
                else if (userDiet == "GlutenFree")
                {
                    var glutenFree = _context.DietsAndRestriction.Where(x => x.GlutenFree == true).ToList();
                    return glutenFree;
                }
                else if (userDiet == "Pescatarian")
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
            else
            {
               List<DietsAndRestriction> none = new List<DietsAndRestriction>();
                return none;
            }
        }
        public List<DietsAndRestriction> GetAllergies()
        {
            List<DietsAndRestriction> restrictions = new List<DietsAndRestriction>();
            if (User.Identity.Name != null)
            {
                AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
                var milk = thisUser.Milk;
                var eggs = thisUser.Eggs;
                var shellfish = thisUser.Shellfish;
                var fish = thisUser.Fish;
                var soy = thisUser.Soy;
                var wheat = thisUser.Wheat;
                var treenuts = thisUser.Treenuts;
                var peanuts = thisUser.Peanuts;

                if (milk == true)
                {
                    var milkList = _context.DietsAndRestriction.Where(x => x.Milk == true).ToList();
                    restrictions.AddRange(milkList);
                }
                if (eggs == true)
                {
                    var eggsList = _context.DietsAndRestriction.Where(x => x.Eggs == true).ToList();
                    restrictions.AddRange(eggsList);
                }
                if (shellfish == true)
                {
                    var shellfishList = _context.DietsAndRestriction.Where(x => x.Shellfish == true).ToList();
                    restrictions.AddRange(shellfishList);
                }
                if (fish == true)
                {
                    var fishList = _context.DietsAndRestriction.Where(x => x.Fish == true).ToList();
                    restrictions.AddRange(fishList);
                }
                if (soy == true)
                {
                    var soyList = _context.DietsAndRestriction.Where(x => x.Soy == true).ToList();
                    restrictions.AddRange(soyList);
                }
                if (wheat == true)
                {
                    var wheatList = _context.DietsAndRestriction.Where(x => x.Wheat == true).ToList();
                    restrictions.AddRange(wheatList);
                }
                else if (treenuts == true)
                {
                    var treenutsList = _context.DietsAndRestriction.Where(x => x.Treenuts == true).ToList();
                    restrictions.AddRange(treenutsList);
                }
                if (peanuts == true)
                {
                    var peanutsList = _context.DietsAndRestriction.Where(x => x.Peaunts == true).ToList();
                    restrictions.AddRange(peanutsList);
                }
            }
            return restrictions;
        }
        public List<DietsAndRestriction> GetAllRestrictions()
        {
            var diet = GetDiet();
            var allergies = GetAllergies();
            diet.AddRange(allergies);
            return diet;
        }
        public async Task<IActionResult> SearchRecipesTitle(string search)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
            var recipes = await response.Content.ReadAsAsync<Recipe>();
            var filteredRecipes = FilterRecipes(recipes);

            return View("FindRecipes", filteredRecipes);
        }
        public async Task<IActionResult> SearchRecipesCategory(string search)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c={search}");
            var recipes = await response.Content.ReadAsAsync<Recipe>();
            var filteredRecipes = FilterRecipesGroup(recipes);

            return View("FindRecipes", filteredRecipes);
        }
        public Recipe FilterRecipesGroup(Recipe recipes)
        {
            var restrictions = GetAllRestrictions();

            int count = recipes.meals.Length;
            var filteredMeals = new Meal[count];
            int index = 0;

            if (restrictions != null)
            {
                foreach (var item in recipes.meals)
                {
                    bool isBad = false;
                    var ingredients = AddIngredients(item);

                    foreach (var name in restrictions)
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
                    index++;
                }
            }
            Recipe filteredRecipes = new Recipe();
            filteredRecipes.meals = filteredMeals;
            return filteredRecipes;
        }
        public Recipe FilterRecipes(Recipe recipes)
        {
            var restrictions = GetAllRestrictions();

            int count = recipes.meals.Length;
            var filteredMeals = new Meal[count];
            int index = 0;

            if (restrictions != null)
            {
                foreach (var item in recipes.meals)
                {
                    bool isBad = false;
                    var ingredients = AddIngredients(item);
                    var instructions = item.strInstructions;

                    foreach (var name in restrictions)
                    {
                        var ingred = name.Id;

                        if (ingredients.Contains(ingred))
                        {
                            isBad = true;
                        }
                    }
                    foreach (var name in restrictions)
                    {
                        var ingred = name.Id;

                        if (instructions.Contains(ingred))
                        {
                            isBad = true;
                        }
                    }
                    if (isBad == false)
                    {
                        filteredMeals[index] = item;
                    }
                    index++;
                }
            }
            Recipe filteredRecipes = new Recipe();
            filteredRecipes.meals = filteredMeals;
            return filteredRecipes;
        }
    }
}