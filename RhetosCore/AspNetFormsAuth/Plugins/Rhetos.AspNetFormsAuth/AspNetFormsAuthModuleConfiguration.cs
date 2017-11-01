using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhetos.AspNetFormsAuth.Interfaces;
using Rhetos.Extensibility;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using System.Threading.Tasks;

namespace Rhetos.AspNetFormsAuth
{
    [Export(typeof(Module))]
    public class AspNetFormsAuthModuleConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddDbContext<AccessControlDbContext>(
                options => options.UseSqlServer(ConfigUtility.GetConnectionStringValue()));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AccessControlDbContext>()
                    .AddDefaultTokenProviders()
                    .AddClaimsPrincipalFactory<RhetosUserClaimPrincipalFactory>();

            services.Configure<IdentityOptions>(options =>
            {
                // Lockout settings: disable lock out
                options.Lockout.MaxFailedAccessAttempts = ConfigUtility.GetPluginSetting<int>("RhetosAspNetFormsAuth", "MaxFailedAccessAttempts", AspNetFormsAuthDefaultSetting.LOCKOUT_MAX_FAILED_ACCESS_ATTEMPTS);
                options.SignIn.RequireConfirmedEmail = ConfigUtility.GetPluginSetting<bool>("RhetosAspNetFormsAuth", "RequireConfirmedEmail", AspNetFormsAuthDefaultSetting.SIGIN_IN_REQUIRE_CONFIRMED_EMAIL);
                
                // User settings
                options.User.RequireUniqueEmail = ConfigUtility.GetPluginSetting<bool>("RhetosAspNetFormsAuth", "RequireUniqueEmail", AspNetFormsAuthDefaultSetting.USER_REQUIRE_UNIQUE_EMAIL); ;
                options.User.AllowedUserNameCharacters = ConfigUtility.GetPluginSetting<string>("RhetosAspNetFormsAuth", "AllowedUserNameCharacters", AspNetFormsAuthDefaultSetting.USER_ALLOWED_USERNAME_CHARACTERS); ;

                // Password settings
                options.Password.RequireUppercase = ConfigUtility.GetPluginSetting<bool>("RhetosAspNetFormsAuth", "RequireUppercase", AspNetFormsAuthDefaultSetting.PASSWORD_REQUIRE_UPPER_CASE); ;
                options.Password.RequireNonAlphanumeric = ConfigUtility.GetPluginSetting<bool>("RhetosAspNetFormsAuth", "RequireNonAlphanumeric", AspNetFormsAuthDefaultSetting.PASSWORD_REQUIRE_NON_ALPHANUMERIC); ;
                options.Password.RequireLowercase = ConfigUtility.GetPluginSetting<bool>("RhetosAspNetFormsAuth", "RequireLowercase", AspNetFormsAuthDefaultSetting.PASSWORD_REQUIRE_LOWER_CASE); ;
                options.Password.RequiredLength = ConfigUtility.GetPluginSetting<int>("RhetosAspNetFormsAuth", "RequiredLength", AspNetFormsAuthDefaultSetting.PASSWORD_REQUIRED_LENGTH); ;
                options.Password.RequireDigit = ConfigUtility.GetPluginSetting<bool>("RhetosAspNetFormsAuth", "RequireDigit", AspNetFormsAuthDefaultSetting.PASSWORD_REQUIRED_DIGITS); ;
                options.Password.RequiredUniqueChars = ConfigUtility.GetPluginSetting<int>("RhetosAspNetFormsAuth", "RequiredUniqueChars", AspNetFormsAuthDefaultSetting.PASSWORD_REQUIRED_UNIQUE_CHARS); ;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            builder.Populate(services);

            builder.RegisterType<AccessControlDbContextInitializer>().As<IDatabaseInitializer>().SingleInstance();
            builder.RegisterType<AspNetUserInfo>().As<IUserInfo>().InstancePerLifetimeScope();
            builder.RegisterType<RhetosFormAuthenticationService>().As<IRhetosFormAuthenticationService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
