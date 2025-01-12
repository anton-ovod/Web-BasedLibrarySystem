using System.Runtime.InteropServices.JavaScript;
using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Implementations;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

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
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = "/Auth/Login";
                    options.SlidingExpiration = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = async context =>
                        {
                            var sessionIdClaim = context.Principal?.FindFirst("SessionId");
                            if (sessionIdClaim is null)
                            {
                                context.RejectPrincipal();
                                await context.HttpContext.SignOutAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme);
                            }

                            var sessionId = sessionIdClaim?.Value!;

                            var userSessionRepository = context.HttpContext.RequestServices
                                .GetRequiredService<IUserSessionRepository>();
                            var session = await userSessionRepository.GetByIdAsync(sessionId);

                            if (session is null)
                            {
                                await userSessionRepository.DeactivateAsync(sessionId);
                                context.RejectPrincipal();
                                await context.HttpContext.SignOutAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme);
                            }
                            else if (session!.ExpiresAt <= DateTime.UtcNow)
                            {
                                await userSessionRepository.DeactivateAsync(sessionId);
                                context.RejectPrincipal();
                                await context.HttpContext.SignOutAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme);
                            }
                            else
                            {
                                var slidingThreshold = session.ExpireTimeSpan / 2;

                                if (DateTime.UtcNow > session.CreatedAt + slidingThreshold)
                                {
                                    session.CreatedAt = DateTime.UtcNow;
                                    await userSessionRepository.UpdateAsync(session);
                                }
                            }
 
                        },

                        OnSigningOut = async context =>
                        {
                            var principal = context.HttpContext.User;

                            var sessionId = principal?.FindFirst("SessionId")?.Value;

                            if (!string.IsNullOrEmpty(sessionId))
                            {
                                var userSessionRepository = context.HttpContext.RequestServices
                                    .GetRequiredService<IUserSessionRepository>();
                                await userSessionRepository.DeactivateAsync(sessionId);
                            }
                        },

                        OnSigningIn = async context =>
                        {
                            var sessionId = Guid.NewGuid().ToString();
                            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString() ??
                                            "Unknown IP address";
                            var userAgent = context.HttpContext.Request.Headers.UserAgent;
                            var device = string.IsNullOrEmpty(userAgent.ToString())
                                ? "Unknown device"
                                : userAgent.ToString();

                            var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                            if (userId != null)
                            {
                                var userSessionRepository = context.HttpContext.RequestServices
                                    .GetRequiredService<IUserSessionRepository>();

                                var expirationTimespan = context.Properties.IsPersistent
                                    ? TimeSpan.FromDays(14)
                                    : TimeSpan.FromHours(3);

                                var newSession = new UserSession
                                {
                                    SessionId = sessionId,
                                    UserId = int.Parse(userId),
                                    CreatedAt = DateTime.UtcNow,
                                    ExpireTimeSpan = expirationTimespan,
                                    IpAddress = ipAddress,
                                    Device = device
                                };

                                await userSessionRepository.AddAsync(newSession);
                                
                                if (context.Principal?.Identity is ClaimsIdentity claimsIdentity)
                                {
                                    claimsIdentity.AddClaim(new Claim("SessionId", sessionId));
                                }
                            }

                        },

                    };
                });

            if(builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();

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
