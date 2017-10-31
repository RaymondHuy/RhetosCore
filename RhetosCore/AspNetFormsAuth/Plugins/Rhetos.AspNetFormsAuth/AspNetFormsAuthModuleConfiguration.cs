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
            Console.WriteLine("Loaded AspNetFormsAuthModuleConfiguration");
            var services = new ServiceCollection();

            services.AddDbContext<AccessControlDbContext>(
                //options => options.UseSqlServer("Data Source = (local); Initial Catalog = TestContext; Integrated Security = SSPI; "));
                options => options.UseSqlServer(ConfigUtility.GetConnectionStringValue()));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AccessControlDbContext>()
                    .AddDefaultTokenProviders()
                    .AddClaimsPrincipalFactory<RhetosUserClaimPrincipalFactory>();

            //services.Configure<IdentityOptions>(options =>
            //{
            //    // Lockout settings: disable lock out
            //    options.Lockout.MaxFailedAccessAttempts = int.MaxValue;
                
            //    // User settings
            //    options.User.RequireUniqueEmail = true;

            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequiredLength = 4;
            //});

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            builder.Populate(services);

            builder.RegisterType<AccessControlDbContextInitializer>().As<IDatabaseInitializer>().SingleInstance();
            builder.RegisterType<AspNetUserInfo>().As<IUserInfo>().InstancePerLifetimeScope();
            builder.RegisterType<RhetosFormAuthenticationService>().As<IRhetosFormAuthenticationService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
