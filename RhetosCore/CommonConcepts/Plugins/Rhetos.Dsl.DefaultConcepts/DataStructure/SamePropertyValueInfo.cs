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
using System.Text;
using Rhetos.Dsl;
using Rhetos.Utilities;
using System.Composition;

namespace Rhetos.Dsl.DefaultConcepts
{
    /// <summary>
    /// Used for internal optimizations when a property on one data structure returns the same value
    /// as a property on referenced (base or parent) data structure.
    /// </summary>
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("SamePropertyValue")]
    public class SamePropertyValueInfo : IConceptInfo, IValidatedConcept
    {
        [ConceptKey]
        public PropertyInfo DerivedProperty { get; set; }

        [ConceptKey]
        /// <summary>Object model property name on the inherited data structure that references the base data structure class.</summary>
        public string BaseSelector { get; set; }

        public PropertyInfo BaseProperty { get; set; }

        public void CheckSemantics(IDslModel existingConcepts)
        {
            DslUtility.ValidateIdentifier(BaseSelector, this, "BaseSelector should be set to a property name from '"
                + DerivedProperty.DataStructure.GetKeyProperties() + "' class.");
        }
    }

    [Export(typeof(IConceptMacro))]
    public class SamePropertyInheritRowPermissionsMacro : IConceptMacro<InitializationConcept>
    {
        public IEnumerable<IConceptInfo> CreateNewConcepts(InitializationConcept conceptInfo, IDslModel existingConcepts)
        {
            var newConcepts = new List<IConceptInfo>();

            var samePropertiesByInheritance = existingConcepts.FindByType<SamePropertyValueInfo>()
                .GroupBy(same => new {
                    Module = same.DerivedProperty.DataStructure.Module.Name,
                    DataStructure = same.DerivedProperty.DataStructure.Name,
                    same.BaseSelector })
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var inherit in existingConcepts.FindByType<RowPermissionsInheritReadInfo>())
            {
                var key = new
                {
                    Module = inherit.InheritFromInfo.RowPermissionsFilters.DataStructure.Module.Name,
                    DataStructure = inherit.InheritFromInfo.RowPermissionsFilters.DataStructure.Name,
                    BaseSelector = inherit.InheritFromInfo.SourceSelector
                };
                var optimizeProperties = samePropertiesByInheritance.GetValueOrEmpty(key);
                newConcepts.AddRange(optimizeProperties
                    .Select(op =>
                        new RowPermissionsInheritReadSameMemberInfo
                        {
                            InheritRead = inherit,
                            BaseMemberName = op.BaseProperty.Name,
                            DerivedMemberName = op.DerivedProperty.Name
                        }));
                newConcepts.AddRange(optimizeProperties
                    .Where(op => op.DerivedProperty is ReferencePropertyInfo && op.BaseProperty is ReferencePropertyInfo)
                    .Select(op =>
                        new RowPermissionsInheritReadSameMemberInfo
                        {
                            InheritRead = inherit,
                            BaseMemberName = op.BaseProperty.Name + "ID",
                            DerivedMemberName = op.DerivedProperty.Name + "ID"
                        }));
            }

            foreach (var inherit in existingConcepts.FindByType<RowPermissionsInheritWriteInfo>())
            {
                var key = new
                {
                    Module = inherit.InheritFromInfo.RowPermissionsFilters.DataStructure.Module.Name,
                    DataStructure = inherit.InheritFromInfo.RowPermissionsFilters.DataStructure.Name,
                    BaseSelector = inherit.InheritFromInfo.SourceSelector
                };
                var optimizeProperties = samePropertiesByInheritance.GetValueOrEmpty(key);
                newConcepts.AddRange(optimizeProperties
                    .Select(op =>
                        new RowPermissionsInheritWriteSameMemberInfo
                        {
                            InheritWrite = inherit,
                            BaseMemberName = op.BaseProperty.Name,
                            DerivedMemberName = op.DerivedProperty.Name
                        }));
                newConcepts.AddRange(optimizeProperties
                    .Where(op => op.DerivedProperty is ReferencePropertyInfo && op.BaseProperty is ReferencePropertyInfo)
                    .Select(op =>
                        new RowPermissionsInheritWriteSameMemberInfo
                        {
                            InheritWrite = inherit,
                            BaseMemberName = op.BaseProperty.Name + "ID",
                            DerivedMemberName = op.DerivedProperty.Name + "ID"
                        }));
            }

            return newConcepts;
        }
    }
}
