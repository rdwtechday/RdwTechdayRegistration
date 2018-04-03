using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using RdwTechdayRegistration.Services;
using RdwTechdayRegistration.Policies;
using Microsoft.AspNetCore.Authorization;
using RdwTechdayRegistration.Utility;
using Microsoft.AspNetCore.Http;

namespace RdwTechdayRegistration
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
            services.AddDbContext<RdwTechdayRegistration.Data.ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<RdwTechdayRegistration.Data.ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = new System.TimeSpan(5,0,0,0));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SiteNotLocked", policy =>
                    policy.Requirements.Add(new SiteNotLockedRequirement() ));
            });

            services.AddSingleton<IAuthorizationHandler, SiteNotLockedHandler>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            // Add Database Initializer
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IProvider<ApplicationDbContext>, Provider<ApplicationDbContext>>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            // Black magic warning, this makes sure the db context remains intact during initialize. Otherwise the dbcontext will be disposed during init 
            ((DbInitializer)dbInitializer).Initialize().Wait();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
