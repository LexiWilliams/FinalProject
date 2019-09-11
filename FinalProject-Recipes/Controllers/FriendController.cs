using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Korzh.EasyQuery;
using FinalProject_Recipes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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

        public IActionResult Index()
        {
            List<AspNetUsers> users = new List<AspNetUsers>();
            foreach (var user in _context.AspNetUsers)
            {

                users.Add(new AspNetUsers() { Id = user.Id, UserName = user.UserName });
            }

                 return View("UserSearchResult", users);
               
            }

        }
    }