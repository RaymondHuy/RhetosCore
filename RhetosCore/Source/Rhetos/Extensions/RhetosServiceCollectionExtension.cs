using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhetos.Extensions
{
    public static class RhetosServiceCollectionExtension
    {
        public static void LoadRhetosPluginWebApi(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            var assemblies = Directory.GetFiles(Paths.ExternalApiModulesFolder, "*.dll", SearchOption.AllDirectories);
            foreach (var item in assemblies)
            {
                var assembly = Assembly.LoadFile(item);
                mvcBuilder.AddApplicationPart(assembly);
            }
        }
    }
}
