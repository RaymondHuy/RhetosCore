
using System;
using System.Composition;
using Rhetos.Compiler;
using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;

namespace Rhetos.RestGenerator.Plugins
{
    /// <summary>
    /// This is not exported, but called from DataStructureCodeGenerator if exists.
    /// </summary>
    public class WritableOrmDataStructureCodeGenerator
    {
        private const string ImplementationCodeSnippet = @"
        [HttpPost]
        [Route("""")]
        public InsertDataResult Insert{0}{1}({0}.{1} entity)
        {{
            if (Guid.Empty == entity.ID)
                entity.ID = Guid.NewGuid();

            var result = _serviceUtility.InsertData(entity);
            return new InsertDataResult {{ ID = entity.ID }};
        }}

        [HttpPut]
        [Route[""{{id}}""]]
        public void Update{0}{1}(string id, {0}.{1} entity)
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
        [Route[""{{id}}""]]
        public void Delete{0}{1}(string id)
        {{
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                throw new Rhetos.LegacyClientException(""Invalid format of GUID parametar 'ID'."");
            var entity = new {0}.{1} {{ ID = guid }};

            _serviceUtility.DeleteData(entity);
        }}

";

        public static void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            DataStructureInfo info = (DataStructureInfo)conceptInfo;

            if (info is IWritableOrmDataStructure)
            {
                codeBuilder.InsertCode(
                    String.Format(ImplementationCodeSnippet, info.Module.Name, info.Name),
                    DataStructureCodeGenerator.AdditionalOperationsTag.Evaluate(info));
            }
        }
    }
}