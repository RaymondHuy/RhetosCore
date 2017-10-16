
using System;
using System.Composition;
using Rhetos.Compiler;
using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;

namespace Rhetos.RestGenerator.Plugins
{
    [Export(typeof(IRestGeneratorPlugin))]
    [ExportMetadata(MefProvider.Implements, typeof(WriteInfo))]
    public class WriteCodeGenerator : IRestGeneratorPlugin
    {
        private const string ImplementationCodeSnippet = @"
        [HttpPost]
        [Route("""")]
        public InsertDataResult Insert{0}{1}([FromBody]{0}.{1} entity)
        {{
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult {{ ID = entity.ID }};
        }}

        [HttpPut]
        [Route(""{{id}}"")]
        public void Update{0}{1}(string id, [FromBody]{0}.{1} entity)
        {{
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException(""Invalid format of GUID parametar 'ID'."");
            if (Guid.Empty == entity.ID)
                entity.ID = guid;
            if (guid != entity.ID)
                throw new Rhetos.LegacyClientException(""Given entity ID is not equal to resource ID from URI."");

            _serviceUtility.UpdateData(entity);
        }}

        [HttpDelete]
        [Route(""{{id}}"")]
        public void Delete{0}{1}(string id)
        {{
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException(""Invalid format of GUID parametar 'ID'."");
            var entity = new {0}.{1} {{ ID = guid }};

            _serviceUtility.DeleteData(entity);
        }}

";

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            WriteInfo info = (WriteInfo)conceptInfo;

            codeBuilder.InsertCode(
                String.Format(ImplementationCodeSnippet, info.DataStructure.Module.Name, info.DataStructure.Name),
                DataStructureCodeGenerator.AdditionalOperationsTag.Evaluate(info.DataStructure));
        }

    }
}