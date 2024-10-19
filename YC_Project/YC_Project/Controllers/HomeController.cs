using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoTsen.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PoTsen.Controllers
{
    [Authorize]  // 不允許造訪
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            TempData["UserName"] = userName;
            return View();
        }

    }
}