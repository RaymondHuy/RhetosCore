using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.IO;
using Rhetos.Utilities;
using System.Runtime.Loader;
using System.Diagnostics;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Rhetos.Extensions;
using Microsoft.AspNetCore.Http;

namespace Rhetos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Paths.InitializeRhetosServerRootPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));

            services.LoadRhetosPluginWebApi();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var stopwatch = Stopwatch.StartNew();
            ConfigUtility.SetConfiguration("appsettings.json");

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new DefaultAutofacConfiguration());

            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        var response = new { message = error.Error.Message };

                        var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                        await context.Response.WriteAsync(json);
                    });
                });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
