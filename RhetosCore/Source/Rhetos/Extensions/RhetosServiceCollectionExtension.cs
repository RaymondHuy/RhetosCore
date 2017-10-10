using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rhetos.Extensions
{
    public static class RhetosServiceCollectionExtension
    {
        public static void AddRhetosPlugin(this IServiceCollection services)
        {
            var stopwatch = Stopwatch.StartNew();

            Paths.InitializeRhetosServer();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new DefaultAutofacConfiguration());
            var container = builder.Build();
            
        }
    }
}
