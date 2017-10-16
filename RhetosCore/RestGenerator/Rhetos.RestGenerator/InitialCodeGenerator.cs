using System;
using Rhetos.Compiler;
using Rhetos.Dsl;
using Microsoft.CSharp.RuntimeBinder;
using System.IO;
using System.Collections.Generic;
using Rhetos.Utilities;

namespace Rhetos.RestGenerator
{
    public class InitialCodeGenerator : IRestGeneratorPlugin
    {
        public const string RhetosRestClassesTag = "/*InitialCodeGenerator.RhetosRestClassesTag*/";
        //public const string ServiceRegistrationTag = "/*InitialCodeGenerator.ServiceRegistrationTag*/";
        //public const string ServiceInitializationTag = "/*InitialCodeGenerator.ServiceInitializationTag*/";

        private const string CodeSnippet =
@"
using Autofac;
using Module = Autofac.Module;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.RestGenerator.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Reflection;
using Rhetos.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Rhetos.WebApiRest
{

    [System.Composition.Export(typeof(Module))]
    public class AutofacModuleConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceUtility>().InstancePerLifetimeScope();
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            base.Load(builder);
        }
    }

    [Route(""api/example/common"")]
    public class ExampleCommonController : Controller
    {
        [HttpGet]
        [Route("""")]
        public string Get(string filter = null)
        {
            return filter;
        }
    }

" + RhetosRestClassesTag + @"
}
";

        private static readonly string _rootPath = Paths.GeneratedFolder;

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            codeBuilder.InsertCode(CodeSnippet);
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.AspNetCore.Mvc.Controller));
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute));
            codeBuilder.AddReferencesFromDependency(typeof(RuntimeBinderException)); // Includes reference to Microsoft.CSharp.
            codeBuilder.AddReferencesFromDependency(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute)); // Includes reference to System.Runtime.Serialization.dll.
            codeBuilder.AddReferencesFromDependency(typeof(Autofac.ContainerBuilder));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.RestGenerator.RestGeneratorModuleConfiguration));
            codeBuilder.AddReferencesFromDependency(typeof(System.Composition.ExportAttribute));
            codeBuilder.AddReferencesFromDependency(typeof(System.Linq.Enumerable));
            codeBuilder.AddReferencesFromDependency(typeof(System.Net.HttpStatusCode));
            codeBuilder.AddReferencesFromDependency(typeof(HashSet<>));
            codeBuilder.AddReferencesFromDependency(typeof(IEnumerable<>));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Processing.ProcessingResult));
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.AspNetCore.Mvc.FromBodyAttribute));

            foreach (var file in Directory.GetFiles(_rootPath, "ServerDom.dll", SearchOption.AllDirectories))
                codeBuilder.AddReference(file);
        }

    }

}
