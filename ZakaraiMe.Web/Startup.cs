﻿namespace ZakaraiMe.Web
{
    using AutoMapper;
    using Data;
    using Data.Entities.Implementations;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authentication.OAuth;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Service.Hubs;
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading.Tasks;

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
            services.AddDbContext<ZakaraiMeContext>(options =>
                        options.UseLazyLoadingProxies()
                               .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = options.User.AllowedUserNameCharacters + WebConstants.CyrillicCharacters;
            })
                .AddEntityFrameworkStores<ZakaraiMeContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.Fields.Add("picture");
                facebookOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        var profileImg = context.User["picture"]["data"].Value<string>("url");
                        identity.AddClaim(new Claim(CustomClaimTypes.Picture.ToString(), profileImg));
                        return Task.CompletedTask;
                    }
                };
            });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; // TODO: set this to false in order to use tempData
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDomainServices();

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                SupportedCultures = new[] { new CultureInfo("en-GB") },
                SupportedUICultures = new[] { new CultureInfo("en-GB") }
            });

            app.UseDatabaseMigrations<ZakaraiMeContext>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Home/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
