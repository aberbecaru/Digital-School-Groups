using SchoolApp.Controllers;
using SchoolApp.Data;
using SchoolApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using SchoolApp.ViewModels;
using System.Runtime.InteropServices;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public GroupsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin, User, Moderator")]
        [HttpGet]
        [Route("/new-group")]
        public IActionResult New()
        {
            CreateGroup createGroup = new()
            {
                group = new Group(),
                categories = db.Categories
            };
            return View(createGroup);
        }


        [Authorize(Roles = "Admin, User, Moderator")]
        [HttpPost]
        [Route("/new-group")]
        public async Task <IActionResult> New(CreateGroup createGroup)
        {

           
            createGroup.categories = db.Categories;
            if (ModelState.IsValid)
            {
                db.Groups.Add(createGroup.group);
                db.SaveChanges();

               

                UserGroup userGroup = new()
                {
                    GroupId = createGroup.group.GroupId,
                    UserId = _userManager.GetUserId(User),
                    IsModerator = true 


                };



                db.UserGroups.Add(userGroup);
                db.SaveChanges();

                if (User.IsInRole("User"))
                {
                    var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                    await _userManager.RemoveFromRoleAsync(user, "User");
                    await _userManager.AddToRoleAsync(user, "Moderator");
                    await _userManager.UpdateAsync(user);

                }

                TempData["message"] = "Grupul a fost creat";

                return RedirectToAction("Index", "Home");



            }
            else
            {
                return View(createGroup);
                
            }
        }

        [Authorize(Roles = "Admin, User, Moderator")]
        public IActionResult Show()
        {
            var userGroups = db.UserGroups.Where(m => m.UserId == _userManager.GetUserId(User));
            userGroups.Select(m => m.Group).Load();
            userGroups.Select(m => m.Group.Category).Load();
            return View(userGroups);
        }

        [Authorize(Roles = "Admin, User, Moderator")]
        public IActionResult ShowGroup(int id)
        {
            ViewBag.GroupId = id;
            Group group = db.Groups.Find(id);
            var messages = db.Messages.Where(m => m.GroupId == id);
            messages.Select(m => m.User).Load();
            ViewBag.Messages = messages;
            return View(group);

        }

        [Authorize(Roles = "Admin, User, Moderator")]
    
        public IActionResult ShowAll()
        {
            var Groups = db.Groups;
            Groups.Select(m => m.Category).Load();
            return View(Groups);
        }



        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Group group = db.Groups.Find(id);
            return View(group);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public IActionResult Edit(int id, Group requestGroup)
        {
            Group group = db.Groups.Find(id);
            if (ModelState.IsValid)
            {
                group.GroupName = requestGroup.GroupName;
                db.SaveChanges();
                TempData["message"] = "Grupul a fost modificat!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(requestGroup);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Group group = db.Groups.Find(id);

            if (group != null)
            {
                var messages = db.Messages.Where(m => m.GroupId == id);
                var userGroups = db.UserGroups.Where(m => m.GroupId == id);
                db.Groups.Remove(group);
                foreach (var message in messages)
                    db.Messages.Remove(message);
                foreach (var userGroup in userGroups)
                    db.UserGroups.Remove(userGroup);
            }
                TempData["message"] = "Grupul a fost sters";
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize(Roles = "Admin, User, Moderator")]
        public IActionResult Join(int id)
        {
            UserGroup currentUserGroup = db.UserGroups.FirstOrDefault(m => m.GroupId == id && m.UserId == _userManager.GetUserId(User));
            if (currentUserGroup == null)
            {
                UserGroup userGroup = new();
                userGroup.GroupId = id;
                userGroup.UserId = _userManager.GetUserId(User);

                db.UserGroups.Add(userGroup);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
    
}