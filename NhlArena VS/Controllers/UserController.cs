using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NhlArena_VS.Models;

namespace NhlArena_VS.Controllers
{
    public class UserController : Controller
    {
        public bool checkLogedin()
        {
            return HttpContext.Session.GetComplexData<string>("id") != null;
        }

        public IActionResult Logout()
        {
            if (checkLogedin())
            {
                HttpContext.Session.Remove("id");
                HttpContext.Session.Remove("username");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}