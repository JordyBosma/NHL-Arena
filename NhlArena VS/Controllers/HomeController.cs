using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NhlArena_VS.Models;
using Microsoft.AspNetCore.Http;
using App_Data;
using objects;

namespace NhlArena_VS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Homepage";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "About";
            ViewData["Message"] = "Your application description page.";
            return View();
        }
        public IActionResult Credits()
        {
            ViewData["Title"] = "Credits";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginPostData data)
        {
            if (ModelState.IsValid)
            {
                Person p = LoginManager.Login(data.username, data.password);
                if (p != null)
                {
                    HttpContext.Session.SetString("id", Convert.ToString(p.id));
                    HttpContext.Session.SetString("username", p.username);
                    return RedirectToAction("Main", "User");
                }
            }
            ViewData["Title"] = "Login Failed";
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            ViewData["Title"] = "SignUp";
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpPostData data)
        {
            if (ModelState.IsValid)
            {
                Person p = LoginManager.Login(data.username, data.password);
                if (p == null)  //no existing user
                {
                    p = SignUpManager.SignUp(data.username, data.password);
                    HttpContext.Session.SetString("id", Convert.ToString(p.id));
                    HttpContext.Session.SetString("username", p.username);
                    return RedirectToAction("Main", "User");
                }
            }
            ViewData["Title"] = "SignUp Failed";
            return View();
        }
        
        public IActionResult TestScene()
        {
            return View();
        }
        public IActionResult TestUI()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
