
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_Recipes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject_Recipes.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private readonly FinalDbContext _context;
        private readonly IConfiguration _configuration;


        public FriendController(FinalDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

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

        public IActionResult ViewFriends()
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();

            var friendList = _context.Friends.Where(f => f.UserId == thisUser.Id).ToList();
            List<AspNetUsers> userFriendList = new List<AspNetUsers>();
            foreach(var friend in friendList)
            {
                if (friend.FriendId != null)
                {
                    var individual = _context.AspNetUsers.Where(u => u.Id == friend.FriendId).First();
                    userFriendList.Add(individual);
                }
            }
            return View(userFriendList);
        }

        public IActionResult AddFriend(AspNetUsers user)
        {
            Friends newFriend = new Friends();
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();

            var favList = _context.Friends.Where(u => u.UserId == thisUser.Id).ToList();
            List<string> friendIdList = new List<string>();
            foreach (var friend in favList)
            {
                friendIdList.Add(friend.FriendId);
            }
            if (friendIdList.Contains(user.Id))
            {
                return View();
            }
            else
            {
                newFriend.UserId = thisUser.Id;
                newFriend.FriendId = user.Id;
            }
            if (ModelState.IsValid)
            {
                _context.Friends.Add(newFriend);
                _context.SaveChanges();
                return RedirectToAction("ViewFriends");
            }
            return View(user);
        }


        public IActionResult RemoveFriend(AspNetUsers friend)
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<Friends> oldFriend = _context.Friends.Where(u => u.UserId == thisUser.Id).ToList();
            Friends friendToDelete = new Friends();
            foreach (var item in oldFriend)
            {
                if (item.FriendId == friend.Id)
                {
                    friendToDelete = item;
                    break;
                }
            }
            _context.Friends.Remove(friendToDelete);
            _context.SaveChanges();
            return RedirectToAction("ViewFriends");
        }

    }

}


