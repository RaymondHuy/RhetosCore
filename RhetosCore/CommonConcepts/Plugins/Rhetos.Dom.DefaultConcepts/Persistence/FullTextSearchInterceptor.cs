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
using System.Composition;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;

namespace Rhetos.Dom.DefaultConcepts.Persistence
{
    /// <summary>
    /// Based on http://www.entityframework.info/Home/FullTextSearch.
    /// This interceptor modifies SQL query generated by FullTextSearchId function mapping in DatabaseExtensionFunctionsMapping.
    /// </summary>
    public class FullTextSearchInterceptor : IDbCommandInterceptor
    {
        #region IDbCommandInterceptor implementation

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }

        #endregion

        private static void RewriteFullTextQuery(DbCommand cmd)
        {
            if (cmd.CommandText.Contains(DatabaseExtensionFunctions.InterceptFullTextSearchFunction))
            {
                var parseFtsQuery = new Regex(@"\(\[Rhetos\]\.\[" + DatabaseExtensionFunctions.InterceptFullTextSearchFunction + @"\]\((?<itemId>.+?), (?<pattern>.*?), (?<tableName>.*?), (?<searchColumns>.*?)\)\) = 1", RegexOptions.Singleline);

                var ftsQueries = parseFtsQuery.Matches(cmd.CommandText).Cast<Match>();

                // Note: When modifying regex, update the error description below!
                var itemIdFormat = new Regex(@"^(\[\w+\]|\w+)(\.(\[\w+\]|\w+))+$", RegexOptions.Singleline);
                var tableNameFormat = new Regex(@"^N'(?<unquoted>(\[\w+\]|\w+)(\.(\[\w+\]|\w+))+)'$", RegexOptions.Singleline);
                var searchColumnsFormat = new Regex(@"^N'(?<unquoted>(\w+|\(\w+(\s*,\s*\w+)*\)|\*))'$", RegexOptions.Singleline);
                var patternFormat = new Regex(@"^(\@\w+|N'([^']*('')*)*')$", RegexOptions.Singleline);

                foreach (var ftsQuery in ftsQueries.OrderByDescending(m => m.Index))
                {
                    // T-SQL CONTAINSTABLE function does not support parameters for table name and columns list,
                    // so the LINQ query must contain string literals for this interceptor to construct a valid SQL subquery with CONTAINSTABLE.
                    // Formatting validations are used here to avoid SQL injection.

                    string itemId = ftsQuery.Groups["itemId"].Value;
                    string tableName = ftsQuery.Groups["tableName"].Value;
                    string searchColumns = ftsQuery.Groups["searchColumns"].Value;
                    string pattern = ftsQuery.Groups["pattern"].Value;

                    if (pattern == "CAST(NULL AS varchar(1))")
                        throw new FrameworkException("Invalid FTS search pattern format. Search pattern must not be NULL.");

                    var validations = new[]
                    {
                        new { Parameter = "itemId", GeneratedSql = itemId, Format = itemIdFormat,
                            InfoSql = "a multi-part identifier",
                            InfoLinq = "a simple property of the queried data structure" },
                        new { Parameter = "tableName", GeneratedSql = tableName, Format = tableNameFormat,
                            InfoSql = "a quoted multi-part identifier",
                            InfoLinq = "a string literal, not a variable," },
                        new { Parameter = "searchColumns", GeneratedSql = searchColumns, Format = searchColumnsFormat,
                            InfoSql = "a quoted column, column list or '*' (see CONTAINSTABLE on MSDN)",
                            InfoLinq = "a string literal, not a variable," },
                        new { Parameter = "pattern", GeneratedSql = pattern, Format = patternFormat,
                            InfoSql = "an SQL query parameter or a quoted string without single-quote characters",
                            InfoLinq = "a simple string variable, not an expression," }, // Not mentioning string literals to simplify the message.
                    };

                    foreach (var test in validations)
                        if (!test.Format.IsMatch(test.GeneratedSql))
                            throw new FrameworkException("Invalid FullTextSearch '" + test.Parameter + "' parameter format in LINQ query."
                                + " Please use " + test.InfoLinq + " for the '" + test.Parameter + "' parameter of the FullTextSeach method."
                                + " The resulting SQL expression (" + test.GeneratedSql + ") should be " + test.InfoSql + ".");

                    string ftsSql = string.Format("{0} IN (SELECT [KEY] FROM CONTAINSTABLE({1}, {2}, {3}))",
                        itemId,
                        tableNameFormat.Match(tableName).Groups["unquoted"].Value,
                        searchColumnsFormat.Match(searchColumns).Groups["unquoted"].Value,
                        pattern);

                    cmd.CommandText =
                        cmd.CommandText.Substring(0, ftsQuery.Index)
                        + ftsSql
                        + cmd.CommandText.Substring(ftsQuery.Index + ftsQuery.Length);
                }
            }

            if (cmd.CommandText.Contains(DatabaseExtensionFunctions.InterceptFullTextSearchFunction))
                throw new FrameworkException("Error while parsing FTS query. Not all search conditions were handled.");
        }
    }
}
