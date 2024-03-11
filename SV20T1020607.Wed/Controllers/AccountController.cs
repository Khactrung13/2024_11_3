using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SV20T1020607.Wed.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            if (Request.Method == "POST")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}

