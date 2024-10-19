using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace PoTsen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 配置 Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "Ticket";
                    options.LoginPath = "/Account/Login";  // 登入路徑
                    options.LogoutPath = "/Account/Logout";  // 登出路徑
                    options.AccessDeniedPath = "/Account/AccessDenied";  // 無權訪問路徑
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(0.1);  // 設置過期時間
                    options.SlidingExpiration = true;
                });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 配置 Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);   // 設定 Session 過期時間
                options.Cookie.HttpOnly = true;                  // 設定 cookie 只能通過伺服器存取
                options.Cookie.IsEssential = true;               // 設置 cookie 為必要，避免 EU cookie 政策的問題
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }


            // 允許提供靜態檔案
            app.UseStaticFiles();

            app.UseRouting();

            // 啟用Session
            app.UseSession();

            // 使用身份驗證
            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}