
using System;
using System.Composition;
using System.Globalization;
using System.Xml;
using Rhetos.Compiler;
using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;
using Rhetos.Utilities;

namespace Rhetos.RestGenerator.Plugins
{
    [Export(typeof(IRestGeneratorPlugin))]
    [ExportMetadata(MefProvider.Implements, typeof(ReportDataInfo))]
    public class ReportCodeGenerator : IRestGeneratorPlugin
    {
        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (ReportDataInfo)conceptInfo;
            
            codeBuilder.InsertCode(ServiceDefinitionCodeSnippet(info), InitialCodeGenerator.RhetosRestClassesTag);
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.RestGenerator.Utilities.ServiceUtility));
            codeBuilder.AddReferencesFromDependency(typeof(Newtonsoft.Json.JsonConvert));
        }

        public static readonly CsTag<DataStructureInfo> FilterTypesTag = "FilterTypes";

        public static readonly CsTag<DataStructureInfo> AdditionalOperationsTag = "AdditionalOperations";

        private static string ServiceDefinitionCodeSnippet(DataStructureInfo info)
        {
            return string.Format(@"
    
    [RoutePrefix(""api/{0}/{1}"")]
    [Authorize]
    public class {0}{1}Controller : ApiController
    {{
        private ServiceUtility _serviceUtility;

        public {0}{1}Controller(ServiceUtility serviceUtility)
        {{
            _serviceUtility = serviceUtility;
        }}
    
        [HttpGet]
        [Route("""")]
        public DownloadReportResult DownloadReport(string parameter = null, string convertFormat = null)
        {{
            return _serviceUtility.DownloadReport<{0}.{1}>(parameter, convertFormat);
        }}

        " + AdditionalOperationsTag.Evaluate(info) + @"
    }}

    ",
            info.Module.Name,
            info.Name);
        }
        
    }
}