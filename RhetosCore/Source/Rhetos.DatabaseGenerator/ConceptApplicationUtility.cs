using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rhetos.DatabaseGenerator
{
    public class ConceptApplicationUtility
    {
        private readonly SqlUtility _sqlUtility;

        public ConceptApplicationUtility(SqlUtility sqlUtility)
        {
            _sqlUtility = sqlUtility;
        }

        public void CheckKeyUniqueness(IEnumerable<ConceptApplication> appliedConcepts, string errorContext)
        {
            var firstError = appliedConcepts.GroupBy(pca => pca.GetConceptApplicationKey()).Where(g => g.Count() > 1).FirstOrDefault();
            if (firstError != null)
                throw new FrameworkException(String.Format("More than one concept application with same key {2} ('{0}') loaded in repository. Concept application IDs: {1}.",
                    firstError.Key, string.Join(", ", firstError.Select(ca => _sqlUtility.QuoteGuid(ca.Id))), errorContext));
        }
    }
}
