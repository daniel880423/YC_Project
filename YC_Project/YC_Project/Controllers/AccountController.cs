using System;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PoTsen.Models;
using System.Security.Claims;

namespace PoTsen.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Login/
        public IActionResult Login()  // 首頁
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Login(AccountModel accountModel)  // 登入
        {
            if (accountModel.Account == "admin" && accountModel.Password == "1234")
            {
                HttpContext.Session.Clear();
                // 將用戶名稱存入Session
                HttpContext.Session.SetString("UserName", "孟柏岑");

                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = "帳號或密碼錯誤!";
            return PartialView();
        }

        [HttpPost]
        public IActionResult Logout()  // 登出
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

    }
}
