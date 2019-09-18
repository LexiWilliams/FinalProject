using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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
        // We aren't using the index. We can delete this and the view

        //public IActionResult Index()
        //{
        //    var meal = GetRandomRecipe().Result;
        //    return View(meal);
        //}
        public IActionResult Search()
        {
            return View();
        }
        public IActionResult ViewRecipe(Meal meal)
        {
            var recipe = FindRecipesById(meal).Result;
            return View(recipe);
        }
       


        public async Task<IActionResult> GetRandomRecipe()
        {
            Recipe filteredRecipes = new Recipe();
            bool notNull = false;
            while (notNull == false)
            {
                var client = RecipeMethods.GetHttpClient();
                var response = await client.GetAsync($"api/json/v1/{_apiKey}/random.php");
                var recipes = await response.Content.ReadAsAsync<Recipe>();
                filteredRecipes = FilterRecipes(recipes);
                if (filteredRecipes.meals[0] != null)
                {
                    notNull = true;
                }
            }
            return View("ViewRecipe", filteredRecipes);
        }
        public IActionResult FindRecipes(Recipe recipes)
        {
            return View(recipes);
        }
        public async Task<IActionResult> SearchRecipesTitle(string search)
        {
            Regex rgx = new Regex(@"^[a-zA-Z]+[ a-zA-Z]*$");
            if (search == null)
            {
                TempData["RegexMatch"] = "Please enter a valid search";
                return View("Search");
            }
            else if (rgx.IsMatch(search))
            {

                var client = RecipeMethods.GetHttpClient();
                var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
                var recipes = await response.Content.ReadAsAsync<Recipe>();
                if (recipes.meals == null)
                {
                    TempData["RegexMatch"] = "No meal by this name";
                    return View("search");
                }
                else
                {
                    var filteredRecipes = FilterRecipes(recipes);
                    if (filteredRecipes.meals == null)
                    {
                        filteredRecipes.meals[0] = null;
                    }

                    return View("FindRecipes", filteredRecipes);
                }

            }
            else
            {
                TempData["RegexMatch"] = "Please enter a valid search";
                return View("Search");
            }
        }
        // for ingredients
        public async Task<IActionResult> SearchRecipesIngredients(string search)
        {
            Regex rgx = new Regex(@"^[a-zA-Z]+[ a-zA-Z]*$");
            if (search == null)
            {
                TempData["RegexMatch"] = "Please enter a valid search";
                return View("Search");
            }
            else if (rgx.IsMatch(search))
            {
                var client = RecipeMethods.GetHttpClient();

                var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?i={search}");
                var recipes = await response.Content.ReadAsAsync<Recipe>();
                if (recipes.meals == null)
                {
                    TempData["RegexMatch"] = "No meals by this name";
                    return View("search");

                }
                else
                {
                    var actualRecipes = GetRecipesById(recipes);
                    var filteredRecipes = FilterRecipes(actualRecipes);
                    if (filteredRecipes.meals[0] == null)
                    {
                        filteredRecipes.meals[0] = null;
                    }

                    return View("FindRecipes", filteredRecipes);

                }
            }
            else
            {
                TempData["RegexMatch"] = "Please enter a valid search";
                return View("search");
            }
        }
        public Recipe GetRecipesById(Recipe recipes)
        {
            Recipe allRecipes = new Recipe();
            Meal[] meals = new Meal[recipes.meals.Length];
            int index = 0;
            foreach (var item in recipes.meals)
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
            var client = RecipeMethods.GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={mealId}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            var meal = viewMeal.meals[0];
            return meal;
        }
        // for Area
        public async Task<IActionResult> SearchRecipesArea(string search)
        {
            var client = RecipeMethods.GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?a={search}");
            var recipes = await response.Content.ReadAsAsync<Recipe>();

            var actualRecipes = GetRecipesById(recipes);
            var filteredRecipes = FilterRecipes(actualRecipes);

            return View("FindRecipes", filteredRecipes);
        }

        public async Task<Meal> GetRecipeByName(string meal)
        {
            var client = RecipeMethods.GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={meal}");
            var name = await response.Content.ReadAsAsync<Meal>();
            return name;
        }
        public async Task<Recipe> FindRecipesById(Meal meal)
        {
            string search = meal.idMeal;
            var client = RecipeMethods.GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/lookup.php?i={search}");
            var viewMeal = await response.Content.ReadAsAsync<Recipe>();
            return viewMeal;
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

        public async Task<IActionResult> SearchRecipesCategory(string search)
        {
            var client = RecipeMethods.GetHttpClient();
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
                    var ingredients = RecipeMethods.AddIngredients(item);

                    foreach (var name in restrictions)
                    {
                        var ingred = name.Id.ToLower();

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
                    var ingredients = RecipeMethods.AddIngredients(item);
                    var instructions = item.strInstructions.ToLower();

                    foreach (var name in restrictions)
                    {
                        var ingred = name.Id.ToLower();

                        if (ingredients.Contains(ingred))
                        {
                            isBad = true;
                        }
                    }
                    foreach (var name in restrictions)
                    {
                        var ingred = name.Id.ToLower();

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

            Recipe myRecipes = new Recipe();
            count = 0;
            foreach (var recipe in filteredRecipes.meals)
            {
                if (recipe != null)
                {
                    count++;
                }
            }
            if (count != 0)
            {
                Meal[] newMeals = new Meal[count];
                count = 0;
                foreach (var recipe in filteredRecipes.meals)
                {
                    if (recipe != null)
                    {
                        newMeals[count] = recipe;
                        count++;
                    }
                }
                filteredRecipes.meals = newMeals;
            }
            else
            {
                Meal[] mealStuff = new Meal[1];
                filteredRecipes.meals = mealStuff;
            }
            return filteredRecipes;
        }

        public IActionResult AutoComplete()
        {
            return View();
        }
    }
}