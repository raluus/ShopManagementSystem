using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Migrations;
using ShopManagementSystem.Models;
using System.Data;

namespace ShopManagementSystem
{
    public class Startup
    {


        public IConfiguration Configuration { get; }



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShopManagementSystemContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ShopManagementSystemContext")));

            services.AddIdentity<Users, UsersRole>(options =>

            options.SignIn.RequireConfirmedEmail = true

            )
           .AddEntityFrameworkStores<ShopManagementSystemContext>()
           .AddDefaultTokenProviders()
           .AddRoles<UsersRole>();

            services.AddScoped<RoleManager<UsersRole>>();
            services.AddScoped<UserManager<Users>, UserManager<Users>>();
            services.AddScoped<SignInManager<Users>, SignInManager<Users>>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}