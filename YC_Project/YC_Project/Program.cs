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

            // �t�m Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "Ticket";
                    options.LoginPath = "/Account/Login";  // �n�J���|
                    options.LogoutPath = "/Account/Logout";  // �n�X���|
                    options.AccessDeniedPath = "/Account/AccessDenied";  // �L�v�X�ݸ��|
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(0.1);  // �]�m�L���ɶ�
                    options.SlidingExpiration = true;
                });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // �t�m Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);   // �]�w Session �L���ɶ�
                options.Cookie.HttpOnly = true;                  // �]�w cookie �u��q�L���A���s��
                options.Cookie.IsEssential = true;               // �]�m cookie �����n�A�קK EU cookie �F�������D
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }


            // ���\�����R�A�ɮ�
            app.UseStaticFiles();

            app.UseRouting();

            // �ҥ�Session
            app.UseSession();

            // �ϥΨ�������
            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}