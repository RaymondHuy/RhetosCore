using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace Rhetos.Dsl.DefaultConcepts
{
    [Export(typeof(IConceptInfo))]
    public class AlternativeKeyComparerInfo : IConceptInfo
    {
        [ConceptKey]
        public EntityComputedFromInfo EntityComputedFrom { get; set; }
    }
}
