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
        public IActionResult DisplayGroups()
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            var groupList = _context.Group.Where(u => u.UserId == thisUser.Id).ToList();
            return View(groupList);

        }
        public IActionResult ListGroups()
        {
            var groupList = _context.Group.ToList();
            List<string> groupNames = new List<string>();
            foreach (var item in groupList)
            {
                if (!groupNames.Contains(item.GroupName))
                {
                    groupNames.Add(item.GroupName);
                }
            }
            List<Group> edittedList = new List<Group>();
            foreach (var name in groupNames)
            {
                Group findGroup = _context.Group.Where(u => u.GroupName == name).First();
                edittedList.Add(findGroup);
            }
            return View(edittedList);
        }


        public IActionResult ViewGroup(Group group)
        {
            var groupMembers = _context.Group.Where(m => m.GroupName == group.GroupName).ToList();
            List<AspNetUsers> groupList = new List<AspNetUsers>();
            foreach (var member in groupMembers)
            {
                var memberInfo = _context.AspNetUsers.Where(u => u.Id == member.UserId).First();
                groupList.Add(memberInfo);
            }
            TempData["Name"] = group.GroupName;
            return View(groupList);

        }

        public IActionResult AddGroup(string groupName)
        {
            Group newGroup = new Group();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();

            var groupList = _context.Group.Where(u => u.UserId == thisUser.Id).ToList();

            List<string> groupNames = new List<string>();
            foreach (var item in groupList)
            {
                    groupNames.Add(item.GroupName);
            }
            if (groupNames.Contains(groupName))
            {
                ViewData["Name"] = groupName;
                return RedirectToAction("ListGroups");
            }
            else
            {
                newGroup.GroupName = groupName;
                newGroup.UserId = thisUser.Id;
                _context.Group.Add(newGroup);
                _context.SaveChanges();

                return RedirectToAction("DisplayGroups");

            }


            
        }
        public IActionResult AddExistingGroup(Group name)
        {
            Group newGroup = new Group();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();

            var groupList = _context.Group.Where(u => u.UserId == thisUser.Id).ToList();

            List<string> groupNames = new List<string>();
            foreach (var item in groupList)
            {
                groupNames.Add(item.GroupName);
            }
            if (groupNames.Contains(name.GroupName))
            {
                ViewData["Name"] = name.GroupName;
                return RedirectToAction("ListGroups");
            }
            else
            {
                newGroup.GroupName = name.GroupName;
                newGroup.UserId = thisUser.Id;
                _context.Group.Add(newGroup);
                _context.SaveChanges();
                return RedirectToAction("DisplayGroups");
            }
            
        }


        public IActionResult RemoveGroup(Group group)
        {
            Group oldGroup = new Group();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<Group> groupFavorites = _context.Group.Where(u => u.UserId == thisUser.Id).ToList();
            foreach (var item in groupFavorites)
            {
                if (item.GroupName == group.GroupName)
                {
                    oldGroup = item;
                    break;
                }
            }
            _context.Group.Remove(oldGroup);
            _context.SaveChanges();
            return RedirectToAction("DisplayGroups");
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
   
            var allergies = GetAllergies();
            return allergies;
        }

        public async Task<IActionResult> SearchRecipesTitle(string search, string groupName)
        {
            if (search == null)
            {
                Group group = _context.Group.Where(u => u.GroupName == groupName).First();
                //TempData["RegexMatch"] = "Please enter a valid search";
                return RedirectToAction("ViewGroup", "Group", group);
            }
        
            else
            {
                var client = RecipeMethods.GetHttpClient();
                var response = await client.GetAsync($"api/json/v1/{_apiKey}/search.php?s={search}");
                var recipes = await response.Content.ReadAsAsync<Recipe>();
                if (recipes.meals == null)
                {
                    Group group = _context.Group.Where(u => u.GroupName == groupName).First();
                    //TempData["RegexMatch"] = "Please enter a valid search";
                    return RedirectToAction("ViewGroup", "Group", group);
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
    }
}