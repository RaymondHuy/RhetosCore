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
    [ExportMetadata(MefProvider.Implements, typeof(DataStructureInfo))]
    [ExportMetadata(MefProvider.DependsOn, typeof(OrmDataStructureCodeGenerator))]
    public class WritableOrmDataStructureCodeGenerator : IConceptCodeGenerator
    {
        /// <summary>Clear objects and context from any state other than new values to be saved.</summary>
        public static readonly CsTag<DataStructureInfo> ClearContextTag = "WritableOrm ClearContext";

        /// <summary>Inserted code can use enumerables "insertedNew", "updatedNew" and "deletedIds".</summary>
        public static readonly CsTag<DataStructureInfo> ArgumentValidationTag = "WritableOrm ArgumentValidation";

        /// <summary>Inserted code can use enumerables "insertedNew", "updatedNew" and "deletedIds".</summary>
        public static readonly CsTag<DataStructureInfo> InitializationTag = "WritableOrm Initialization";

        /// <summary>Lists "updated" and "deleted" contain OLD data.
        /// Enumerables "insertedNew", "updatedNew" and "deletedIds" contain NEW data.
        /// Sample usage: 1. Verify that locked items are not going to be updated or deleted, 2. Cascade delete.</summary>
        public static readonly CsTag<DataStructureInfo> OldDataLoadedTag = "WritableOrm OldDataLoaded";

        [Obsolete]
        public static readonly CsTag<DataStructureInfo> ProcessedOldDataTag = "WritableOrm ProcessedOldData";

        /// <summary>Insert code here to recompute (insert/update/delete) other entities that depend on the changes items.
        /// Queries "inserted" and "updated" will return NEW data.
        /// Data is already saved to the database (but the SQL transaction has not yet been committed) so SQL validations and computations can be used.</summary>
        public static readonly CsTag<DataStructureInfo> OnSaveTag1 = "WritableOrm OnSaveTag1";

        /// <summary>Insert code here to verify that invalid items are not going to be inserted or updated.
        /// Queries "inserted" and "updated" will return NEW data.
        /// Data is already saved to the database (but the SQL transaction has not yet been committed) so SQL validations and computations can be used.</summary>
        public static readonly CsTag<DataStructureInfo> OnSaveTag2 = "WritableOrm OnSaveTag2";

        /// <summary>Insert code here to returns a list at errors for the given items (IList&lt;Guid&gt; ids).
        /// Data is already saved to the database (but the SQL transaction has not yet been committed) so SQL validations and computations can be used.</summary>
        public static readonly CsTag<DataStructureInfo> OnSaveValidateTag = "WritableOrm OnSaveValidate";

        public static readonly CsTag<DataStructureInfo> OnDatabaseErrorTag = "WritableOrm OnDatabaseError";

        // TODO: Remove "duplicateObjects" check after implementing stateless session with "manual" saving.
        protected static string MemberFunctionsSnippet(DataStructureInfo info)
        {
            return string.Format(
        @"public void Save(IEnumerable<{0}.{1}> insertedNew, IEnumerable<{0}.{1}> updatedNew, IEnumerable<{0}.{1}> deletedIds, bool checkUserPermissions = false)
        {{
            Common.Infrastructure.MaterializeItemsToSave(ref insertedNew);
            Common.Infrastructure.MaterializeItemsToSave(ref updatedNew);
            Common.Infrastructure.MaterializeItemsToDelete(ref deletedIds);

            if (insertedNew.Count() == 0 && updatedNew.Count() == 0 && deletedIds.Count() == 0)
                return;

            foreach (var item in insertedNew)
                if (item.ID == Guid.Empty)
                    item.ID = Guid.NewGuid();

            " + ClearContextTag.Evaluate(info) + @"

            if (insertedNew.Count() > 0)
            {{
                var duplicateObjects = Filter(insertedNew.Select(item => item.ID).ToArray());
                if (duplicateObjects.Count() > 0)
                {{
                    var deletedIndex = new HashSet<Guid>(deletedIds.Select(item => item.ID));
                    duplicateObjects = duplicateObjects.Where(item => !deletedIndex.Contains(item.ID)).ToArray();
                }}
                if (duplicateObjects.Count() > 0)
                {{
                    string msg = ""Inserting a record that already exists. ID="" + duplicateObjects.First().ID;
                    if (checkUserPermissions)
                        throw new Rhetos.ClientException(msg);
                    else
                        throw new Rhetos.FrameworkException(msg);
                }}
            }}

            " + ArgumentValidationTag.Evaluate(info) + @"

            " + InitializationTag.Evaluate(info) + @"

            // Using old data, including lazy loading of navigation properties:
            IEnumerable<Common.Queryable.{0}_{1}> deleted = this.Query(deletedIds.Select(item => item.ID)).ToList();
            Rhetos.Utilities.Graph.SortByGivenOrder((List<Common.Queryable.{0}_{1}>)deleted, deletedIds.Select(item => item.ID), item => item.ID);
            IEnumerable<Common.Queryable.{0}_{1}> updated = this.Query(updatedNew.Select(item => item.ID)).ToList();
            Rhetos.Utilities.Graph.SortByGivenOrder((List<Common.Queryable.{0}_{1}>)updated, updatedNew.Select(item => item.ID), item => item.ID);

            " + OldDataLoadedTag.Evaluate(info) + @"

            " + ProcessedOldDataTag.Evaluate(info) + @"

            try
            {{
                if (deletedIds.Count() > 0)
                {{
                    _executionContext.EntityFrameworkContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    foreach (var item in deletedIds.Select(item => item.ToNavigation()))
                        _executionContext.EntityFrameworkContext.Entry(item).State = EntityState.Deleted;
                    _executionContext.EntityFrameworkContext.ChangeTracker.AutoDetectChangesEnabled = true;
                    _executionContext.EntityFrameworkContext.SaveChanges();
                }}

                if (updatedNew.Count() > 0)
                {{
                    _executionContext.EntityFrameworkContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    foreach (var item in updatedNew.Select(item => item.ToNavigation()))
                        _executionContext.EntityFrameworkContext.Entry(item).State = EntityState.Modified;
                    _executionContext.EntityFrameworkContext.ChangeTracker.AutoDetectChangesEnabled = true;
                    _executionContext.EntityFrameworkContext.SaveChanges();
                }}

                if (insertedNew.Count() > 0)
                {{
                    _executionContext.EntityFrameworkContext.{0}_{1}.AddRange(insertedNew.Select(item => item.ToNavigation()));
                    _executionContext.EntityFrameworkContext.SaveChanges();
                }}

                _executionContext.EntityFrameworkContext.ClearCache();
            }}
            catch (DbUpdateException saveException)
            {{
        		var interpretedException = _sqlUtility.InterpretSqlException(saveException);
        		" + OnDatabaseErrorTag.Evaluate(info) + @"

        		if (interpretedException != null)
        			Rhetos.Utilities.ExceptionsUtility.Rethrow(interpretedException);
                var sqlException = _sqlUtility.ExtractSqlException(saveException);
                if (sqlException != null)
                    Rhetos.Utilities.ExceptionsUtility.Rethrow(sqlException);
                throw;
            }}

            deleted = null;
            updated = this.Query(updatedNew.Select(item => item.ID));
            IEnumerable<Common.Queryable.{0}_{1}> inserted = this.Query(insertedNew.Select(item => item.ID));

            bool allEffectsCompleted = false;
            try
            {{
                " + OnSaveTag1.Evaluate(info) + @"

                " + OnSaveTag2.Evaluate(info) + @"

                Rhetos.Dom.DefaultConcepts.InvalidDataMessage.ValidateOnSave(insertedNew, updatedNew, this, ""{0}.{1}"");
                allEffectsCompleted = true;
            }}
            finally
            {{
                if (!allEffectsCompleted)
                    _executionContext.PersistenceTransaction.DiscardChanges();
            }}
        }}

        public IEnumerable<Rhetos.Dom.DefaultConcepts.InvalidDataMessage> Validate(IList<Guid> ids, bool onSave)
        {{
            " + OnSaveValidateTag.Evaluate(info) + @"
            yield break;
        }}

        ",
                info.Module.Name,
                info.Name);
        }

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (DataStructureInfo)conceptInfo;

            if (info is IWritableOrmDataStructure)
            {
                codeBuilder.InsertCode("IWritableRepository<" + info.Module.Name + "." + info.Name + ">", RepositoryHelper.RepositoryInterfaces, info);
                codeBuilder.InsertCode("IValidateRepository", RepositoryHelper.RepositoryInterfaces, info);
                codeBuilder.AddReferencesFromDependency(typeof(IWritableRepository<>));
                codeBuilder.AddReferencesFromDependency(typeof(IValidateRepository));

                codeBuilder.InsertCode(MemberFunctionsSnippet(info), RepositoryHelper.RepositoryMembers, info);
                codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Utilities.ExceptionsUtility));
                codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Utilities.CsUtility));
                codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Dom.DefaultConcepts.InvalidDataMessage));
            }
        }
    }
}
