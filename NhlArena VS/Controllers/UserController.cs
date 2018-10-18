using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NhlArena_VS.Models;

namespace NhlArena_VS.Controllers
{
    public class UserController : Controller
    {
        public bool checkLogedIn()
        {
            return HttpContext.Session.GetString("id") != null;
        }

        public IActionResult Main()
        {
            if (!checkLogedIn()) return RedirectToAction("Login", "Home");

            return View();
        }

        [HttpGet()]
        public IActionResult Game(string id)
        {
            if (!checkLogedIn()) return RedirectToAction("Login", "Home");
            ViewData["game"] = id;
            return View();
        }

        public IActionResult Logout()
        {
            if (checkLogedIn())
            {
                HttpContext.Session.Remove("id");
                HttpContext.Session.Remove("username");
            }
            return RedirectToAction("Index", "Home");
        }


    }
}