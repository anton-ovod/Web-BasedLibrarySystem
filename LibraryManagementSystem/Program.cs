using LibraryManagementSystem.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LibraryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<SqlDatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabaseConnection"));
            });

            builder.Services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.SlidingExpiration = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.IsEssential = true;
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
                pattern: "{controller=Welcome}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
