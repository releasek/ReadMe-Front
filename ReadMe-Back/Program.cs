using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.Security;
using ReadMe_Back.Models.Services;

namespace ReadMe_Back
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 註冊 MVC 控制器
            builder.Services.AddControllersWithViews();

            // 註冊 Repository 和 Service
            builder.Services.AddScoped<OrderEFRepo>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<ProductEFRepo>();
            builder.Services.AddScoped<AdminUsersEFRepo>();
            builder.Services.AddScoped<AdminUsersServices>();
            builder.Services.AddScoped<RoleFunctionsServices>();
            builder.Services.AddScoped<RoleFunctionsEFRepo>();
            builder.Services.AddScoped<PromotionEFRepo>();
            builder.Services.AddScoped<PromotionService>();

            // 設定資料庫
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // 設定身份驗證 (支援 Cookie 和 Google)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // 預設使用 Cookie
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Google 為預設挑戰方式
            })
            .AddCookie(options =>
            {
                int minutes = 100;
                options.LoginPath = "/AdminUsers/Login";
                options.LogoutPath = "/AdminUsers/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(minutes);

            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["GoogleLogin:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["GoogleLogin:ClientSecret"];

            });

            builder.Services.AddSingleton<IAuthorizationHandler, FunctionAuthorizeHandler>();

            var app = builder.Build();

            // 配置 HTTP Request 處理流程
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // 啟用身份驗證與授權
            app.UseAuthentication();
            app.UseAuthorization();

            // 設定預設路由
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=AdminUsers}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
