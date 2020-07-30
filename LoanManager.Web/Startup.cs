using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LoanManager.Data;
using Microsoft.EntityFrameworkCore;
using LoanManager.Repo;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using LoanManager.Service; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Localization;  
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;

namespace LoanManager.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {  
            services.AddTransient<SetViewDataFilter>();
            //services.AddMvc(options =>
            //{
            //    options.Filters.AddService<SetViewDataFilter>();
            //});

            services.AddHttpContextAccessor(); //for access AppContext.Current

             //Localization 
            services.AddMvc(options =>
            {
                options.Filters.AddService<SetViewDataFilter>();
            }).AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddPortableObjectLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"), 
                new CultureInfo("ar-SA"),
            };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            //Localization end

            //this service is used for mysql data base.
             //services.AddDbContext<ApplicationContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),  mysqlOptions => {  mysqlOptions.ServerVersion(new Version(5, 7, 14), ServerType.MySql);    }));

            //if you want to use MS Sql server database than comment above servies and uncomment below.
           services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), i => i.UseRowNumberForPaging()));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IAuthLoginService, AuthLoginService>();
            services.AddTransient<IMenuBarService, MenuBarService>(); 
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleUserService, RoleUserService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IMenuPermissionService, MenuPermissionService>();
            services.AddTransient<IAppSettingService, AppSettingService>();
            services.AddTransient<IGeneralSettingService, GeneralSettingService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<ILoanManService, LoanManService>();
 

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login";
                    options.LogoutPath = "/auth/logout";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //Localization
            app.UseRequestLocalization();
            //Localization end

            AppContext.Configure(app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>()); //for access AppContext.Current

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

