using Rhetos.Compiler;
using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace Rhetos.Dom.DefaultConcepts
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(AlternativeKeyComparerInfo))]
    public class AlternativeKeyComparerCodeGenerator : IConceptCodeGenerator
    {
        public static readonly CsTag<AlternativeKeyComparerInfo> CompareKeyPropertyTag = "CompareKeyProperty";

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (AlternativeKeyComparerInfo)conceptInfo;

            string targetEntity = info.EntityComputedFrom.Target.GetKeyProperties();
            string recomputeFunctionName = EntityComputedFromCodeGenerator.RecomputeFunctionName(info.EntityComputedFrom);
            string keyComparerName = recomputeFunctionName + "_KeyComparer";
            string keyComparerSnippet =
        $@"private class {keyComparerName} : IComparer<{targetEntity}>
        {{
            public int Compare({targetEntity} x, {targetEntity} y)
            {{
                int diff;
                {CompareKeyPropertyTag.Evaluate(info)}
                return diff;
            }}
        }}

        ";
            codeBuilder.InsertCode(keyComparerSnippet, RepositoryHelper.RepositoryMembers, info.EntityComputedFrom.Target);
            codeBuilder.InsertCode($"new {keyComparerName}(), //", EntityComputedFromCodeGenerator.OverrideKeyComparerTag, info.EntityComputedFrom);
        }
    }
}
