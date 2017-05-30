﻿/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Rhetos.Utilities;
using Rhetos.Dsl;
using Rhetos.Extensibility;
using Rhetos.Logging;
using System.Text;

namespace Rhetos.DatabaseGenerator
{
    public class ConceptApplicationRepository : IConceptApplicationRepository
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly ILogger _logger;
        private readonly XmlUtility _xmlUtility;
        private readonly SqlUtility _sqlUtility;
        private readonly ConceptApplicationUtility _conceptApplicationUtility;
        private readonly SqlResource _sqlResource;

        public ConceptApplicationRepository(
            ISqlExecuter sqlExecuter,
            ILogProvider logProvider,
            XmlUtility xmlUtility,
            SqlUtility sqlUtility,
            ConceptApplicationUtility conceptApplicationUtility)
        {
            _sqlExecuter = sqlExecuter;
            _logger = logProvider.GetLogger("ConceptApplicationRepository");
            _xmlUtility = xmlUtility;
            _sqlUtility = sqlUtility;
            _conceptApplicationUtility = conceptApplicationUtility;
        }

        public List<ConceptApplication> Load()
        {
            var previoslyAppliedConcepts = LoadOldConceptApplicationsFromDatabase();
            _conceptApplicationUtility.CheckKeyUniqueness(previoslyAppliedConcepts, "loaded");

            var dependencies = LoadDependenciesFromDatabase();
            EvaluateDependencies(previoslyAppliedConcepts, dependencies); // Replace guids with actual ConceptApplication instances.

            return previoslyAppliedConcepts;
        }

        private List<ConceptApplication> LoadOldConceptApplicationsFromDatabase()
        {
            var previoslyAppliedConcepts = new List<ConceptApplication>();

            _sqlExecuter.ExecuteReader(
                "SELECT ID, InfoType, ConceptInfoKey, ImplementationType, CreateQuery, RemoveQuery, ModificationOrder FROM Rhetos.AppliedConcept ORDER BY ModificationOrder",
                dataReader =>
                {
                    previoslyAppliedConcepts.Add(new ConceptApplication
                    {
                        Id = _sqlUtility.ReadGuid(dataReader, 0),
                        ConceptInfoTypeName = dataReader.GetString(1),
                        ConceptInfoKey = dataReader.GetString(2),
                        ConceptImplementationTypeName = dataReader.GetString(3),
                        CreateQuery = _sqlUtility.EmptyNullString(dataReader, 4),
                        RemoveQuery = _sqlUtility.EmptyNullString(dataReader, 5),
                        OldCreationOrder = _sqlUtility.ReadInt(dataReader, 6)
                    });
                });

            var invalidCa = previoslyAppliedConcepts.FirstOrDefault(ca => ca.ConceptInfoKey == ObsoleteConceptApplicationMark);
            if (invalidCa != null)
                throw new FrameworkException("Obsolete concept application loaded from database (Rhetos.ConceptApplication, ID = " + _sqlUtility.GuidToString(invalidCa.Id)
                    + "). Upgrade procedure for old version of the system should include deployment with empty DslScritps folder (using old version of Rhetos server and packages) to remove old database structure and keep the data.");

            return previoslyAppliedConcepts;
        }

        private const string ObsoleteConceptApplicationMark = "/*UNKNOWN*/";

        private List<DependencyGuids> LoadDependenciesFromDatabase()
        {
            var dependencies = new List<DependencyGuids>();
            _sqlExecuter.ExecuteReader(
                "SELECT DependentID, DependsOnID FROM Rhetos.AppliedConceptDependsOn",
                dataReader =>
                {
                    dependencies.Add(new DependencyGuids
                    {
                        Dependent = _sqlUtility.ReadGuid(dataReader, 0),
                        DependsOn = _sqlUtility.ReadGuid(dataReader, 1),
                        DebugInfo = "From database"
                    });
                });
            return dependencies;
        }
        
        private static void EvaluateDependencies(List<ConceptApplication> previoslyAppliedConcepts, List<DependencyGuids> dependencies)
        {
            var dependenciesByDependent = dependencies.GroupBy(d => d.Dependent).ToDictionary(g => g.Key, g => g.Select(item => new { Id = item.DependsOn, item.DebugInfo }).ToArray());
            var conceptApplicationsById = previoslyAppliedConcepts.ToDictionary(ca => ca.Id);

            foreach (var dependent in previoslyAppliedConcepts)
            {
                var dependsOns = dependenciesByDependent.GetValueOrDefault(dependent.Id);
                if (dependsOns != null)
                    dependent.DependsOn = dependsOns
                        .Select(dependsOn => new ConceptApplicationDependency
                        {
                            ConceptApplication = conceptApplicationsById.GetValue(dependsOn.Id, "Nonexistent dependency on Rhetos.AppliedConcept, ID={0}, referenced from table Rhetos.AppliedConceptDependsOn column DependsOn."),
                            DebugInfo = dependsOn.DebugInfo
                        })
                        .ToArray();
                else
                    dependent.DependsOn = new ConceptApplicationDependency[] { };
            }

            var invalidDependentId = dependenciesByDependent.Keys.Except(previoslyAppliedConcepts.Select(ca => ca.Id)).ToArray();
            if (invalidDependentId.Length > 0)
                throw new FrameworkException(string.Format("Nonexistent dependency on Rhetos.AppliedConcept, ID={0}, referenced from table Rhetos.AppliedConceptDependsOn column DependentID.",
                    invalidDependentId.First()));
        }

        private class DependencyGuids
        {
            public Guid Dependent;
            public Guid DependsOn;
            public string DebugInfo;
        }

        public string DeleteAllMetadataSql()
        {
            return _sqlResource.Get("ConceptApplicationRepository_DeleteAll");
        }

        public List<string> DeleteMetadataSql(ConceptApplication ca)
        {
            return new List<string>
            {
                _sqlResource.Format("ConceptApplicationRepository_Delete", _sqlUtility.QuoteGuid(ca.Id))
            };
        }

        public List<string> InsertMetadataSql(NewConceptApplication ca)
        {
            var sql = new List<string>();

            sql.Add(_sqlResource.Format("ConceptApplicationRepository_Insert",
                _sqlUtility.QuoteGuid(ca.Id),
                _sqlUtility.QuoteText(ca.ConceptInfoTypeName),
                _sqlUtility.QuoteText(ca.ConceptInfoKey),
                _sqlUtility.QuoteText(ca.ConceptImplementationTypeName),
                _sqlUtility.QuoteText(_xmlUtility.SerializeToXml(ca.ConceptInfo)),
                _sqlUtility.QuoteText(ca.CreateQuery),
                _sqlUtility.QuoteText(ca.RemoveQuery),
                _sqlUtility.QuoteText(ca.ConceptImplementationVersion.ToString())));

            foreach (var dependsOnId in ca.DependsOn.Select(d => d.ConceptApplication.Id).Distinct())
                sql.Add(_sqlResource.Format("ConceptApplicationRepository_InsertDependency",
                    _sqlUtility.QuoteGuid(ca.Id),
                    _sqlUtility.QuoteGuid(dependsOnId)));

            return sql;
        }

        public List<string> UpdateMetadataSql(NewConceptApplication ca, ConceptApplication oldApp)
        {
            var sql = new List<string>();
            if (oldApp.RemoveQuery != ca.RemoveQuery)
                sql.Add(Sql.Format("ConceptApplicationRepository_Update",
                    _sqlUtility.QuoteGuid(ca.Id),
                    _sqlUtility.QuoteText(ca.ConceptInfoTypeName),
                    _sqlUtility.QuoteText(ca.ConceptInfoKey),
                    _sqlUtility.QuoteText(ca.ConceptImplementationTypeName),
                    _sqlUtility.QuoteText(_xmlUtility.SerializeToXml(ca.ConceptInfo)),
                    _sqlUtility.QuoteText(ca.CreateQuery),
                    _sqlUtility.QuoteText(ca.RemoveQuery),
                    _sqlUtility.QuoteText(ca.ConceptImplementationVersion.ToString())));

            HashSet<Guid> oldDependsOn = new HashSet<Guid>(oldApp.DependsOn.Select(depOn => depOn.ConceptApplication.Id));
            HashSet<Guid> newDependsOn = new HashSet<Guid>(ca.DependsOn.Select(depOn => depOn.ConceptApplication.Id));

            foreach (var dependsOnId in ca.DependsOn.Select(d => d.ConceptApplication.Id).Distinct())
                if (!oldDependsOn.Contains(dependsOnId))
                    sql.Add(Sql.Format("ConceptApplicationRepository_InsertDependency",
                        _sqlUtility.QuoteGuid(ca.Id),
                        _sqlUtility.QuoteGuid(dependsOnId)));

            foreach (var dependsOnId in oldApp.DependsOn.Select(d => d.ConceptApplication.Id).Distinct())
                if (!newDependsOn.Contains(dependsOnId))
                    sql.Add(Sql.Format("ConceptApplicationRepository_DeleteDependency",
                        _sqlUtility.QuoteGuid(ca.Id),
                        _sqlUtility.QuoteGuid(dependsOnId)));

            return sql;
        }
    }

    
}
