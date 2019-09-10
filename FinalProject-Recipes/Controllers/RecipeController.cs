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

        //public async Task<IActionResult> SearchRecipesTitle(string search)
        //{
        //    var client = GetHttpClient();

        //    var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
        //    var meal = await response.Content.ReadAsAsync<Recipe>();
        //    return View("FindRecipes", meal);
        //}

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

            if(diet == "none")
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


        // Filtering 
        public List<Diets> GetDiets(string diet)
        {

            var vegan = _context.Diets.Where(x => x.Id == "Vegan").ToList();
            var keto = _context.Diets.Where(x => x.Id == "Keto").ToList();
            var vegetarian = _context.Diets.Where(x => x.Id == "Vegetarian").ToList();
            var paleo = _context.Diets.Where(x => x.Id == "Paleo").ToList();
            var gluten = _context.Diets.Where(x => x.Id == "GlutenFree").ToList();
            var none = _context.Diets.Where(x => x.Id == "null").ToList();
            //var none = new List<Diets>();
            if (diet == "Vegan")
            {

                return vegan;
            }
            else if (diet == "Keto")
            {
                return keto;
            }
            else if (diet == "Vegetarian")
            {
                return vegetarian;
            }
            else if (diet == "Paleo")
            {
                return paleo;
            }
            else if (diet == "GlutenFree")
            {
                return gluten;
            }
            else
            {
                return none;
            }
        }

        public async Task<IActionResult> SearchRecipesTitle(string search)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
            var recipes = await response.Content.ReadAsAsync<Recipe>();

            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            //var userDiet = thisUser.Diet;
            var userDiet = "Vegan";
            var diet = GetDiets(userDiet);
            var pizza = diet[0];
            var dietStrList = new List<string>();
            foreach (var item in diet)
            {
                dietStrList.Add(item.ToString());
            }


            int count = recipes.meals.Length;
            Recipe filteredRecipes = new Recipe();
            Meal[] filteredMeals = new Meal[count];

            var filteredRecipeList = new List<Meal>();

            if (diet != null)
            {
                foreach (var item in recipes.meals)
                {
                    var ingredients = new List<string>();
                    ingredients.Add(item.strIngredient1.ToString());
                    ingredients.Add(item.strIngredient2.ToString());
                    ingredients.Add(item.strIngredient3.ToString());
                    ingredients.Add(item.strIngredient4.ToString());
                    ingredients.Add(item.strIngredient5.ToString());
                    ingredients.Add(item.strIngredient6.ToString());
                    ingredients.Add(item.strIngredient7.ToString());
                    ingredients.Add(item.strIngredient8.ToString());
                    ingredients.Add(item.strIngredient9.ToString());
                    ingredients.Add(item.strIngredient10.ToString());
                    ingredients.Add(item.strIngredient11.ToString());
                    ingredients.Add(item.strIngredient12.ToString());
                    ingredients.Add(item.strIngredient13.ToString());
                    ingredients.Add(item.strIngredient14.ToString());
                    ingredients.Add(item.strIngredient15.ToString());
                    ingredients.Add(item.strIngredient16.ToString());
                    ingredients.Add(item.strIngredient17.ToString());
                    ingredients.Add(item.strIngredient18.ToString());
                    ingredients.Add(item.strIngredient19.ToString());
                    ingredients.Add(item.strIngredient20.ToString());

                    bool isValid = true;
                    while (isValid)
                    {
                        foreach (var dietIngredient in dietStrList)
                        {
                            if (ingredients.Contains(dietIngredient))
                            {
                                isValid = false;
                            }

                        }
                        if (isValid)
                        {
                            //filteredMeals.Append(item);
                            filteredRecipeList.Add(item);
                        }
                        break;
                    }
                    
                    
                    //for (int i = 0; i <= diet.Count; i++)
                    //{
                    //    var dietString = diet[i].ToString();
                    //    if (ingredients.Contains(dietString))
                    //    {
                    //    }
                    //    else
                    //    {
                    //        filteredMeals.Append(item);
                    //    }
                    //}
                }
            }
            if (filteredMeals == null)
            {
                return View("FindRecipes", recipes);
            }
            else
            {
                //filteredRecipes.meals = filteredMeals;
                //return View("FindRecipes", filteredRecipes);
                return View("FindRecipes", filteredRecipeList);
            }
        }
    }
}