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

        [ValidateAntiForgeryToken] // 驗證防偽令牌
        [AllowAnonymous] // 允許未登入的用戶訪問
        [HttpPost]
        public IActionResult Login(AccountModel Accountmodel)  // 登入
        {
            if (ModelState.IsValid)
            {
                if (Accountmodel.Account == "admin" && Accountmodel.Password == "1234")
                {
                    HttpContext.Session.Clear();
                    // 將用戶名稱存入Session
                    HttpContext.Session.SetString("UserName", "孟柏岑");

                    // 創建 Ticket
                    var Ticket = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "孟柏岑"),
                        new Claim("FullName", "Administrator"),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("CustomData", "Some additional info")  // 自定義數據
                    };

                    var claimsIdentity = new ClaimsIdentity(Ticket, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        //IsPersistent = true, // 設置 Cookie 為持久性，關閉視窗後是否還存在
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(0.1) // 設置 Cookie 的過期時間
                    };

                    // 登入使用者並創建 Cookie
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index", "Home");
                }
                TempData["Error"] = "帳號或密碼錯誤!";
            }

            return View(Accountmodel);
        }


        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Logout()  // 登出
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync("Ticket");
            return RedirectToAction("Login", "Account");
        }

    }
}
