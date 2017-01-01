using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BetterStarterTemplate.Web.Services;
using BetterStarterTemplate.Identity.Data;
using Microsoft.Extensions.FileProviders;
//using Microsoft.AspNetCore.Hosting.Internal;
using System.IO;

namespace BetterStarterTemplate.Builder
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("aspnet-BetterStarterTemplate.Web-5e621f9a-2638-44a6-845a-f9205732cb41");
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                ;
                //Adding controllers from another assembly                
                //.ConfigureApplicationPartManager(manager => manager.ApplicationParts.Clear())
                //.AddApplicationPart(typeof(Controllers.AccountController).GetTypeInfo().Assembly)
                //.AddRazorOptions(options =>
                //{
                    //options.FileProviders.Clear();
                    //options.FileProviders.Add(new PhysicalFileProvider(                        
                    //        typeof(Client.ViewLocationExpander).GetTypeInfo().Assembly.Location                            
                    //    )
                    //);
                    //options.FileProviders.Add(new PhysicalFileProvider(Directory.GetCurrentDirectory() + "..\\..\\"));
                    //options.FileProviders.Add(new CompositeFileProvider(
                    //   new EmbeddedFileProvider(
                    //       typeof(Client.ViewLocationExpander).GetTypeInfo().Assembly,
                    //       "BetterStarterTemplate.Web.Client" // your external assembly's base namespace
                    //   )
                    //));

                    //options.ViewLocationExpanders.Add(new Client.ViewLocationExpander());
                //});


            //.AddApplicationPart(typeof(BetterStarterTemplate.App.Controllers.HomeController).GetTypeInfo().Assembly);

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

