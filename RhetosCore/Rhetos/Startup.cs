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
        public void ConfigureServices(IServiceCollection services)
        {
            var plugins = Directory.GetFiles(@"D:\Project\Rhetos\RhetosCore\RhetosCore\Source\DeployPackages\bin\Debug\netcoreapp2.0\Plugins", "*.dll");
            foreach (var item in plugins)
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(item);
            }
            AssemblyLoadContext.Default.LoadFromAssemblyPath(@"D:\Project\Rhetos\RhetosCore\RhetosCore\Source\DeployPackages\bin\Debug\netcoreapp2.0\ServerDom.dll");
            
            var apiService = Assembly.LoadFile(@"D:\Project\Rhetos\RhetosCore\RhetosCore\Source\DeployPackages\bin\Debug\netcoreapp2.0\Generated\ApiService.dll");
            services
                .AddMvc()
                .AddApplicationPart(apiService);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
