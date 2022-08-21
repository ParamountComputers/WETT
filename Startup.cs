using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WETT.Data;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using WETT.Services;
using WETT.Infrastructure;
using Constants = WETT.Infrastructure.Constants;
using Microsoft.Identity.Web.UI;

namespace WETT
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
			services.AddDbContext<WETT_DBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WETTDbConnection")));
			//      services.AddDatabaseDeveloperPageExceptionFilter();
			//services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			//	.AddMicrosoftIdentityWebApp(options => Configuration.Bind("AzureAD", options));

			services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(options =>
			{
				Configuration.Bind("AzureAd", options);
				options.TokenValidationParameters.RoleClaimType = "roles";
			});

			services.AddAuthorizationCore();
			services.AddDatabaseDeveloperPageExceptionFilter();
			services.AddControllersWithViews();

			//			services.AddControllersWithViews(options =>
			//			{
			//				var policy = new AuthorizationPolicyBuilder()
			//				.RequireAuthenticatedUser()
			//				.Build();
			//				options.Filters.Add(new AuthorizeFilter(policy));
			//			});

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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
