using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rhetos.Compiler;
using Rhetos.Compiler.Interfaces;
using Rhetos.Extensibility;
using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICodeGenerator = Rhetos.Compiler.ICodeGenerator;

namespace Rhetos.RestGenerator
{
    [Export(typeof(IGenerator))]
    public class RestGenerator : IGenerator
    {
        private readonly IPluginsContainer<IRestGeneratorPlugin> _plugins;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IAssemblyGenerator _assemblyGenerator;
        private readonly ILogger _logger;
        private readonly ILogger _sourceLogger;

        public static string GetAssemblyPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "ApiService.dll");
        }

        public RestGenerator(
            IPluginsContainer<IRestGeneratorPlugin> plugins,
            ICodeGenerator codeGenerator,
            ILogProvider logProvider,
            IAssemblyGenerator assemblyGenerator
        )
        {
            _plugins = plugins;
            _codeGenerator = codeGenerator;
            _assemblyGenerator = assemblyGenerator;

            _logger = logProvider.GetLogger("RestGenerator");
            _sourceLogger = logProvider.GetLogger("Rest service");
        }

        public void Generate()
        {
            string assemblyName = "ApiService";
            IAssemblySource assemblySource = _codeGenerator.ExecutePlugins(_plugins, "/*", "*/", new InitialCodeGenerator());
            _logger.Trace("References: " + string.Join(", ", assemblySource.RegisteredReferences));
            _sourceLogger.Trace(assemblySource.GeneratedCode);
            CompilerParameter parameters = new CompilerParameter
            {
                OutputAssemblyPath = Path.Combine(Paths.ExternalApiModulesFolder, assemblyName + ".dll"),
                OutputPdbPath = Path.Combine(Paths.ExternalApiModulesFolder, assemblyName + ".pdb"),
                Options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                AssemblyName = assemblyName
            };
            _assemblyGenerator.Generate(assemblySource, parameters);
        }

        public IEnumerable<string> Dependencies
        {
            get { return new[] { "" }; }
        }
    }
}
