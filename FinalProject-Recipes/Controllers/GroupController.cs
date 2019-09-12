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
    [Authorize]
    public class GroupController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public GroupController(FinalDbContext context, IConfiguration configuration)
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
        public List<string> AddIngredients(Meal item)
        {
            var ingredients = new List<string>();
            if (item.strIngredient1 != null)
            {
                ingredients.Add(item.strIngredient1.ToLower());
            }
            if (item.strIngredient2 != null)
            {
                ingredients.Add(item.strIngredient2.ToLower());
            }
            if (item.strIngredient3 != null)
            {
                ingredients.Add(item.strIngredient3.ToLower());
            }
            if (item.strIngredient4 != null)
            {
                ingredients.Add(item.strIngredient4.ToLower());
            }
            if (item.strIngredient5 != null)
            {
                ingredients.Add(item.strIngredient5.ToLower());
            }
            if (item.strIngredient6 != null)
            {
                ingredients.Add(item.strIngredient6.ToLower());
            }
            if (item.strIngredient7 != null)
            {
                ingredients.Add(item.strIngredient7.ToLower());
            }
            if (item.strIngredient8 != null)
            {
                ingredients.Add(item.strIngredient8.ToLower());
            }
            if (item.strIngredient9 != null)
            {
                ingredients.Add(item.strIngredient9.ToLower());
            }
            if (item.strIngredient10 != null)
            {
                ingredients.Add(item.strIngredient10.ToLower());
            }
            if (item.strIngredient11 != null)
            {
                ingredients.Add(item.strIngredient11.ToLower());
            }
            if (item.strIngredient12 != null)
            {
                ingredients.Add(item.strIngredient12.ToLower());
            }
            if (item.strIngredient13 != null)
            {
                ingredients.Add(item.strIngredient13.ToLower());
            }
            if (item.strIngredient14 != null)
            {
                ingredients.Add(item.strIngredient14.ToLower());
            }
            if (item.strIngredient15 != null)
            {
                ingredients.Add(item.strIngredient15.ToLower());
            }
            if (item.strIngredient16 != null)
            {
                ingredients.Add(item.strIngredient16.ToLower());
            }
            if (item.strIngredient17 != null)
            {
                ingredients.Add(item.strIngredient17.ToLower());
            }
            if (item.strIngredient18 != null)
            {
                ingredients.Add(item.strIngredient18.ToLower());
            }
            if (item.strIngredient19 != null)
            {
                ingredients.Add(item.strIngredient19.ToLower());
            }
            if (item.strIngredient20 != null)
            {
                ingredients.Add(item.strIngredient20.ToLower());
            }
            return ingredients;
        }

        public List<DietsAndRestriction> GetAllergies()
        {
            //Getting list of group
            List<Group> groupList = _context.Group.ToList();

            List<DietsAndRestriction> restrictions = new List<DietsAndRestriction>();
            foreach (var member in groupList)
            {
                AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.Id == member.UserId).First();
                var milk = thisUser.Milk;
                var eggs = thisUser.Eggs;
                var shellfish = thisUser.Shellfish;
                var fish = thisUser.Fish;
                var soy = thisUser.Soy;
                var wheat = thisUser.Wheat;
                var treenuts = thisUser.Treenuts;
                var peanuts = thisUser.Peanuts;
                var diet = thisUser.Diet;

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
                if(diet == "Vegan")
                {
                    var veganList = _context.DietsAndRestriction.Where(x => x.Vegan == true).ToList();
                    restrictions.AddRange(veganList);
                }
                if (diet == "Vegetarian")
                {
                    var vegetarianList = _context.DietsAndRestriction.Where(x => x.Vegetarian == true).ToList();
                    restrictions.AddRange(vegetarianList);
                }
                if (diet == "Keto")
                {
                    var ketoList = _context.DietsAndRestriction.Where(x => x.Keto == true).ToList();
                    restrictions.AddRange(ketoList);
                }
                if (diet == "Paleo")
                {
                    var paleoList = _context.DietsAndRestriction.Where(x => x.Paleo == true).ToList();
                    restrictions.AddRange(paleoList);
                }
                if (diet == "Pescatarian")
                {
                    var pescatarianList = _context.DietsAndRestriction.Where(x => x.Pescatarian == true).ToList();
                    restrictions.AddRange(pescatarianList);
                }
                if (diet == "GlutenFree")
                {
                    var glutenFreeList = _context.DietsAndRestriction.Where(x => x.GlutenFree == true).ToList();
                    restrictions.AddRange(glutenFreeList);
                }


            }


            return restrictions;
        }
        public List<DietsAndRestriction> GetAllRestrictions()
        {
            //var diet = GetDiet();
            var allergies = GetAllergies();
            //diet.AddRange(allergies);
            return allergies;
        }
        public async Task<IActionResult> SearchRecipesTitle(string search)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
            var recipes = await response.Content.ReadAsAsync<Recipe>();
            var filteredRecipes = FilterRecipes(recipes);

            return View("FindRecipes", filteredRecipes);
        }
        //public async Task<IActionResult> SearchRecipesCategory(string search)
        //{
        //    var client = GetHttpClient();
        //    var response = await client.GetAsync($"api/json/v1/{_apiKey}/filter.php?c={search}");
        //    var recipes = await response.Content.ReadAsAsync<Recipe>();
        //    var filteredRecipes = FilterRecipesGroup(recipes);

        //    return View("FindRecipes", filteredRecipes);
        //}
        
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
            return filteredRecipes;
        }
    }
}