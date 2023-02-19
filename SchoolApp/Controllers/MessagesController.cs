using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.VisualStudio;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public MessagesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }
        public IActionResult New(string id)
        {
            ViewBag.GroupId = id;
            return View();

        }
        [HttpPost]
        public IActionResult New(int id, Message message)
        {
            if (ModelState.IsValid)
            {
                Message message1 = new();
                message1.GroupId = id;
                message1.UserId = _userManager.GetUserId(User);
                message1.Content = message.Content;
                db.Messages.Add(message1);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                return View(message);
            }
            
        }

        public IActionResult Show(int id)
        {
            var messages = db.Messages.Where(m => m.GroupId == id);

            ViewBag.messages = messages;
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Message message = db.Messages.Find(id); 
            return View(message);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Message requestMessage)
        {
            Message message = db.Messages.Find(id);

            if (ModelState.IsValid)
            {

                message.Content = requestMessage.Content;
                db.SaveChanges();
                TempData["message"] = "Mesajul a fost modificat!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(requestMessage);
            }
        }

        public ActionResult Delete(int id)
        {
            var message = db.Messages.Find(id);
            if ( message != null )
                db.Messages.Remove(message);
            TempData["message"] = "Mesajul a fost sters";
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
