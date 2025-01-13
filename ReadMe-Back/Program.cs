using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.Services;

namespace ReadMe_Back
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // µù¥U OrderEFRepo
            builder.Services.AddScoped<OrderEFRepo>();

            // µù¥U OrderService
            builder.Services.AddScoped<OrderService>();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<ProductEFRepo>();
            builder.Services.AddScoped<AdminUsersEFRepo>();
            builder.Services.AddScoped<AdminUsersServices>();
            builder.Services.AddScoped<RoleFunctionsServices>();
            builder.Services.AddScoped<RoleFunctionsEFRepo>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/AdminUsers/Login";
                    options.AccessDeniedPath = "/AdminUsers/AccessDenied";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=AdminUsers}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
