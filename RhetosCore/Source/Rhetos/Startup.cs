﻿using System;
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
            var apiService = Assembly.LoadFile(@"D:\Project\Rhetos\RhetosCore\RhetosCore\Source\Rhetos\bin\Debug\netcoreapp2.0\Generated\ApiService.dll");
            services
                .AddMvc()
                .AddApplicationPart(apiService);


            var stopwatch = Stopwatch.StartNew();

            Paths.InitializeRhetosServerRootPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
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

            app.UseMvc();
        }
    }
}
