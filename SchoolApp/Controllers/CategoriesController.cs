using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;
using System.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult New()
        {
            Category category = new Category();
            return View(category); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata";
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return View(cat);
            }
        }

        public IActionResult Show()
        {
            ViewBag.Categories = db.Categories;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(int id, Category requestCategory)
        {
            Category category = db.Categories.Find(id);

            if (ModelState.IsValid)
            {

                category.CategoryName = requestCategory.CategoryName;
                db.SaveChanges();
                TempData["message"] = "Categoria a fost modificata!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(requestCategory);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            var groups = db.Groups.Where(m => m.CategoryId == id);
            foreach (var group in groups)
            {
                var messages = db.Messages.Where(m => m.GroupId == group.GroupId);
                var userGroups = db.UserGroups.Where(m => m.GroupId == group.GroupId);
                foreach (var message in messages)
                    db.Messages.Remove(message);
                foreach (var userGroup in userGroups)
                    db.UserGroups.Remove(userGroup);
                db.Groups.Remove(group);
            }
            if (category != null)
            {
                db.Categories.Remove(category);
                
            }
            TempData["message"] = "Categoria a fost stearsa";
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }

}
