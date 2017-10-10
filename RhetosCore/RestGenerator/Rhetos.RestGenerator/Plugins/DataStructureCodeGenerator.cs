
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
    [ExportMetadata(MefProvider.Implements, typeof(DataStructureInfo))]
    public class DataStructureCodeGenerator : IRestGeneratorPlugin
    {
        public static readonly CsTag<DataStructureInfo> FilterTypesTag = "FilterTypes";

        public static readonly CsTag<DataStructureInfo> AdditionalOperationsTag = "AdditionalOperations";

        public static readonly CsTag<DataStructureInfo> AdditionalPropertyInitialization = "AdditionalPropertyInitialization";
        public static readonly CsTag<DataStructureInfo> AdditionalPropertyConstructorParameter = "AdditionalPropertyConstructorParameter";
        public static readonly CsTag<DataStructureInfo> AdditionalPropertyConstructorSetProperties = "AdditionalPropertyConstructorSetProperties";

        private static string ServiceDefinitionCodeSnippet(DataStructureInfo info)
        {
            return string.Format(@"
    
    [Route(""api/{0}/{1}"")]
    [Authorize]
    public class {0}{1}Controller : Controller
    {{
        private ServiceUtility _serviceUtility;
        {2}

        public {0}{1}Controller(ServiceUtility serviceUtility{3})
        {{
            _serviceUtility = serviceUtility;
            {4}
        }}
    
        public static readonly IDictionary<string, Type[]> FilterTypes = new List<Tuple<string, Type>>
            {{
                " + FilterTypesTag.Evaluate(info) + @"
            }}
            .GroupBy(typeName => typeName.Item1)
            .ToDictionary(g => g.Key, g => g.Select(typeName => typeName.Item2).Distinct().ToArray());

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [HttpGet]
        [Route("""")]
        public RecordsResult<{0}.{1}> Get(string filter = null, string fparam = null, string genericfilter = null, string filters = null, string sort = null, int top = 0, int skip = 0, int page = 0, int psize = 0)
        {{
            var data = _serviceUtility.GetData<{0}.{1}>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: false);
            return new RecordsResult<{0}.{1}> {{ Records = data.Records }};
        }}

        [Obsolete]
        [HttpGet]
        [Route(""Count"")]
        public CountResult GetCount(string filter = null, string fparam = null, string genericfilter = null, string filters = null, string sort = null)
        {{
            var data = _serviceUtility.GetData<{0}.{1}>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new CountResult {{ TotalRecords = data.TotalCount }};
        }}

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters).
        [HttpGet]
        [Route(""TotalCount"")]
        public TotalCountResult GetTotalCount(string filter = null, string fparam = null, string genericfilter = null, string filters = null, string sort = null)
        {{
            var data = _serviceUtility.GetData<{0}.{1}>(filter, fparam, genericfilter, filters, FilterTypes, 0, 0, 0, 0, sort,
                readRecords: false, readTotalCount: true);
            return new TotalCountResult {{ TotalCount = data.TotalCount }};
        }}

        // [Obsolete] parameters: filter, fparam, genericfilter (use filters), page, psize (use top and skip).
        [HttpGet]
        [Route(""RecordsAndTotalCount"")]
        public RecordsAndTotalCountResult<{0}.{1}> GetRecordsAndTotalCount(string filter = null, string fparam = null, string genericfilter = null, string filters = null, string sort = null, int top = 0, int skip = 0, int page = 0, int psize = 0)
        {{
            return _serviceUtility.GetData<{0}.{1}>(filter, fparam, genericfilter, filters, FilterTypes, top, skip, page, psize, sort,
                readRecords: true, readTotalCount: true);
        }}

        [HttpGet]
        [Route(""{{id}}"")]
        public {0}.{1} GetById(string id)
        {{
            var result = _serviceUtility.GetDataById<{0}.{1}>(id);
            if (result == null)
                throw new Rhetos.LegacyClientException(""There is no resource of this type with a given ID."") {{ HttpStatusCode = HttpStatusCode.NotFound }};
            return result;
        }}

        " + AdditionalOperationsTag.Evaluate(info) + @"
    }}
    ",
            info.Module.Name,
            info.Name,
            AdditionalPropertyInitialization.Evaluate(info),
            AdditionalPropertyConstructorParameter.Evaluate(info),
            AdditionalPropertyConstructorSetProperties.Evaluate(info)
            );
        }

        public static bool IsTypeSupported(DataStructureInfo conceptInfo)
        {
            return conceptInfo is IOrmDataStructure
                || conceptInfo is BrowseDataStructureInfo
                || conceptInfo is QueryableExtensionInfo
                || conceptInfo is ComputedInfo;
        }

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            DataStructureInfo info = (DataStructureInfo)conceptInfo;

            if (IsTypeSupported(info))
            {
                //codeBuilder.InsertCode(ServiceRegistrationCodeSnippet(info), InitialCodeGenerator.ServiceRegistrationTag);
                //codeBuilder.InsertCode(ServiceInitializationCodeSnippet(info), InitialCodeGenerator.ServiceInitializationTag);
                codeBuilder.InsertCode(ServiceDefinitionCodeSnippet(info), InitialCodeGenerator.RhetosRestClassesTag);
                codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Processing.DefaultCommands.ReadCommandResult));
                codeBuilder.AddReferencesFromDependency(typeof(Newtonsoft.Json.Linq.JToken));
            }
        }
    }
}