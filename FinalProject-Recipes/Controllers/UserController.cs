using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_Recipes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FinalProject_Recipes.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        public UserController(FinalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _apiKey = _configuration.GetSection("AppConfiguration")["RecipeAPIKey"];
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Preferences()
        {
            return View();
        }
        public IActionResult EditPreferences(string milk, string eggs, string fish, string shellfish, string treenuts, string peanuts, string soy, string wheat, string diet, string privacy)
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (milk == "milk")
            {
                thisUser.Milk = true;
            }
            else
            {
                thisUser.Milk = false;
            }
            if (eggs == "eggs")
            {
                thisUser.Eggs = true;
            }
            else
            {
                thisUser.Eggs = false;
            }
            if (fish == "fish")
            {
                thisUser.Fish = true;
            }
            else
            {
                thisUser.Fish = false;
            }
            if (shellfish == "shellfish")
            {
                thisUser.Shellfish = true;
            }
            else
            {
                thisUser.Shellfish = false;
            }
            if (treenuts == "treenuts")
            {
                thisUser.Treenuts = true;
            }
            else
            {
                thisUser.Treenuts = false;
            }
            if (peanuts == "peanuts")
            {
                thisUser.Peanuts = true;
            }
            else
            {
                thisUser.Peanuts = false;
            }
            if (soy == "soy")
            {
                thisUser.Soy = true;
            }
            else
            {
                thisUser.Soy = false;
            }
            if (wheat == "wheat")
            {
                thisUser.Wheat = true;
            }
            else
            {
                thisUser.Wheat = false;
            }

            if (diet == "none")
            {
                thisUser.Diet = null;
            }
            else
            {
                thisUser.Diet = diet;
            }
            if (privacy == "private")
            {
                thisUser.Private = true;
            }
            else if (privacy == "public")
            {
                thisUser.Private = false;
            }
            _context.Entry(thisUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Search");
        }
    }
}