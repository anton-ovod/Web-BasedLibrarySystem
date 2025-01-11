using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Repositories.Implementations;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
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

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = "/Auth/Login";
                    options.SlidingExpiration = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                });

            if(builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 10485760;
            });

            var app = builder.Build();

            var cookiePolicyOptions = new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                Secure = CookieSecurePolicy.Always,
                HttpOnly = HttpOnlyPolicy.Always

            };

            app.UseCookiePolicy(cookiePolicyOptions);
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts
            }
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
           

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Welcome}/{action=Index}");

            app.Run();
        }
    }
}
