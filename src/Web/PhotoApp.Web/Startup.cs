using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.CloudinaryService;
using PhotoApp.Services.PhotoService;
using PhotoApp.Services.UpdateService;
using PhotoApp.Services.UserService;
using PhotoApp.Web.Hubs;
using System;
using System.Linq;

namespace PhotoApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PhotoAppDbContext>(options =>
                 options.UseMySql(
                     Configuration.GetConnectionString("DefaultConnection")));


            services.AddDefaultIdentity<PhotoAppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 3;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PhotoAppDbContext>();


            Account account = new Account(
                this.Configuration["Cloudinary:Appname"],
                this.Configuration["Cloudinary:ApiKey"],
                this.Configuration["Cloudinary:ApiSecret"]
                );
            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
            services.AddTransient<ICloundinaryService, CloudinaryService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IChallangeService, ChallangeService>();


            services.AddHostedService<ChallangeUpdateService>();

            services.AddSignalR();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<PhotoAppDbContext>())
                {
                    dbContext.Database.Migrate();


                    if (dbContext.Roles.Count() == 0)
                    {
                        dbContext.Roles.Add(new IdentityRole
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN",
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        });

                        dbContext.Roles.Add(new IdentityRole
                        {
                            Name = "Moderator",
                            NormalizedName = "MODERATOR",
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        });

                        dbContext.Roles.Add(new IdentityRole
                        {
                            Name = "User",
                            NormalizedName = "USER",
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        });

                        dbContext.SaveChanges();
                    }
                }
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LoadHub>("/load");
                endpoints.MapHub<VoteHub>("/vote");

                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
