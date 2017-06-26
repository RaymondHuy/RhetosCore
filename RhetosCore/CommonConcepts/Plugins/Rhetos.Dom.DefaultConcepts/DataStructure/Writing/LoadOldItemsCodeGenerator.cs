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
using Rhetos.Dsl.DefaultConcepts;
using System.Globalization;
using System.Composition;
using Rhetos.Compiler;
using Rhetos.Extensibility;
using Rhetos.Dsl;

namespace Rhetos.Dom.DefaultConcepts
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(LoadOldItemsInfo))]
    public class LoadOldItemsCodeGenerator : IConceptCodeGenerator
    {
        public static readonly CsTag<LoadOldItemsInfo> SelectPropertiesTag = "SelectProperties";

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (LoadOldItemsInfo)conceptInfo;
            codeBuilder.InsertCode(GetSnippet(info), WritableOrmDataStructureCodeGenerator.InitializationTag, info.SaveMethod.Entity);
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Utilities.Graph));
        }

        private string GetSnippet(LoadOldItemsInfo info)
        {
            return string.Format(
            @"var updatedIdsList = updatedNew.Select(item => item.ID).ToList();
            var deletedIdsList = deletedIds.Select(item => item.ID).ToList();
            var updatedOld = Filter(Query(), updatedIdsList).Select(item => new {{ item.ID{0} }}).ToList();
            var deletedOld = Filter(Query(), deletedIdsList).Select(item => new {{ item.ID{0} }}).ToList();
            Rhetos.Utilities.Graph.SortByGivenOrder(updatedOld, updatedIdsList, item => item.ID);
            Rhetos.Utilities.Graph.SortByGivenOrder(deletedOld, deletedIdsList, item => item.ID);

            ",
                SelectPropertiesTag.Evaluate(info));
        }
    }
}
