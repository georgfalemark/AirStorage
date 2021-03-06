﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirStorage.Data;
using AirStorage.Data.DbContext;
using EmailSender.Configuration;
using EmailSender.Configuration.Impl;
using EmailSender.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirStorage
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });






            if (Configuration["Environment:Name"] == "PRODUCTION")  //Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
            else
                services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



            // Automatically perform database migration
            services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();


            services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<AppDbContext>();






            //TODO
            //string facebookAppId = Configuration["Authentication:Facebook:AppId"];

            //IConfigurationSection facebookAuthNSection2 =
            //            Configuration.GetSection("Authentication:Facebook");

            //string ClientId = facebookAuthNSection2["ClientId"];
            //string ClientSecret = facebookAuthNSection2["ClientSecret"];



            //TODO
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddFacebook(options =>
                {
                    IConfigurationSection facebookAuthNSection =
                        Configuration.GetSection("Authentication:Facebook");

                    options.ClientId = facebookAuthNSection["ClientId"];
                    options.ClientSecret = facebookAuthNSection["ClientSecret"];
                });
            //services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            //    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //}).AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //}).AddMicrosoftAccount(microsoftOptions =>
            //{
            //    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ApplicationId"];
            //    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:Password"];
            //});


            //för att koppla interface mot klassen
            //services.AddTransient<ITestInterface, TestRepository>();

            //TODO
            services.AddTransient<IEmailSender, EmailSenderImpl>();
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());




            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            //TODO
            //Nuget
            //Används för att det skall autoladdas om varje gång man öppnar upp sidan
            //app.UseBrowserLink();





            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
