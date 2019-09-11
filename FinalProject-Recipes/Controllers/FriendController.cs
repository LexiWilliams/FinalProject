using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_Recipes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_Recipes.Controllers
{
    public class FriendController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;


        public FriendController(FinalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //public IActionResult Index()
        //{
        //    List<AspNetUsers> users = new List<AspNetUsers>();
        //    foreach (var user in _context.AspNetUsers)
        //    {

        //        users.Add(new AspNetUsers() { Id = user.Id, UserName = user.UserName });
        //    }

        //    return View("UserSearchResult", users);

        //}

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var user = from s in _context.AspNetUsers select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                user = user.Where(s => s.UserName.Contains(searchString));

            }

            switch (sortOrder)
            {
                case "name_desc":
                    user = user.OrderByDescending(s => s.UserName);
                    break;

                default:
                    user = user.OrderBy(s => s.UserName);
                    break;
            }

            return View(await user.AsNoTracking().ToListAsync());

        }

    }

}