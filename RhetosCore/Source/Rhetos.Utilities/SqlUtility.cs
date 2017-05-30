/*
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
using System.Data.Common;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Rhetos.Utilities
{
    public class SqlUtility
    {
        // TODO: Move most of the methods to ISqlUtility.

        private ConfigUtility _configUtility;
        public SqlUtility(ConfigUtility configUtility)
        {
            _configUtility = configUtility;
        }

        private static int? _sqlCommandTimeout = null;
        /// <summary>
        /// In seconds.
        /// </summary>
        public int SqlCommandTimeout
        {
            get
            {
                if (!_sqlCommandTimeout.HasValue)
                {
                    string value = _configUtility.GetAppSetting("SqlCommandTimeout");

                    if (!string.IsNullOrEmpty(value))
                        _sqlCommandTimeout = int.Parse(value);
                    else
                        _sqlCommandTimeout = 30;
                }
                return _sqlCommandTimeout.Value;
            }
        }

        private static string _databaseLanguage;
        private static string _nationalLanguage;

        private static void SetLanguageFromProviderName(string connectionStringProvider)
        {
            var match = new Regex(@"^Rhetos\.(?<DatabaseLanguage>\w+)(.(?<NationalLanguage>\w+))?$").Match(connectionStringProvider);
            if (!match.Success)
                throw new FrameworkException("Invalid 'providerName' format in 'ServerConnectionString' connection string. Expected providerName format is 'Rhetos.<database language>' or 'Rhetos.<database language>.<natural language settings>', for example 'Rhetos.MsSql' or 'Rhetos.Oracle.XGERMAN_CI'.");
            _databaseLanguage = match.Groups["DatabaseLanguage"].Value ?? "";
            _nationalLanguage = match.Groups["NationalLanguage"].Value ?? "";
        }

        private string GetProviderNameFromConnectionString()
        {
            var connectionStringProvider = _configUtility.GetConnectionStringProvider();
            if (string.IsNullOrEmpty(connectionStringProvider))
                throw new FrameworkException("Missing 'providerName' attribute in 'ServerConnectionString' connection string. Expected providerName format is 'Rhetos.<database language>' or 'Rhetos.<database language>.<natural language settings>', for example 'Rhetos.MsSql' or 'Rhetos.Oracle.XGERMAN_CI'.");
            return connectionStringProvider;
        }

        public string DatabaseLanguage
        {
            get
            {
                if (_databaseLanguage == null)
                    SetLanguageFromProviderName(GetProviderNameFromConnectionString());

                return _databaseLanguage;
            }
        }

        public string NationalLanguage
        {
            get
            {
                if (_nationalLanguage == null)
                    SetLanguageFromProviderName(GetProviderNameFromConnectionString());

                return _nationalLanguage;
            }
        }


        private static string _connectionString;
        public string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = _configUtility.GetConnectionStringValue();
                    if (string.IsNullOrEmpty(_connectionString))
                        throw new FrameworkException("Empty 'ServerConnectionString' connection string in application configuration.");
                }
                return _connectionString;
            }
        }

        public string ProviderName
        {
            get
            {
                if (DatabaseLanguage.Equals("MsSql"))
                    return "System.Data.SqlClient";
                else if (DatabaseLanguage.Equals("Oracle"))
                    return "Oracle.ManagedDataAccess.Client";
                else
                    throw new FrameworkException(UnsupportedLanguageError);
            }
        }

        public string UserContextInfoText(IUserInfo userInfo)
        {
            if (!userInfo.IsUserRecognized)
                return "";

            return "Rhetos:" + userInfo.Report();
        }

        public IUserInfo ExtractUserInfo(string contextInfo)
        {
            if (contextInfo == null)
                return new ReconstructedUserInfo { IsUserRecognized = false, UserName = null, Workstation = null };

            string prefix1 = "Rhetos:";
            string prefix2 = "Alpha:";

            int positionUser;
            if (contextInfo.StartsWith(prefix1))
                positionUser = prefix1.Length;
            else if (contextInfo.StartsWith(prefix2))
                positionUser = prefix2.Length;
            else
                return new ReconstructedUserInfo { IsUserRecognized = false, UserName = null, Workstation = null };

            var result = new ReconstructedUserInfo();

            int positionWorkstation = contextInfo.IndexOf(',', positionUser);
            if (positionWorkstation > -1)
            {
                result.UserName = contextInfo.Substring(positionUser, positionWorkstation - positionUser);
                result.Workstation = contextInfo.Substring(positionWorkstation + 1);
            }
            else
            {
                result.UserName = contextInfo.Substring(positionUser);
                result.Workstation = "";
            }

            result.UserName = result.UserName.Trim();
            if (result.UserName == "") result.UserName = null;
            result.Workstation = result.Workstation.Trim();
            if (result.Workstation == "") result.Workstation = null;

            result.IsUserRecognized = result.UserName != null;
            return result;
        }

        private class ReconstructedUserInfo : IUserInfo
        {
            public bool IsUserRecognized { get; set; }
            public string UserName { get; set; }
            public string Workstation { get; set; }
            public string Report() { return UserName + "," + Workstation; }
        }

        /// <summary>
        /// Throws an exception if 'name' is not a valid SQL database object name.
        /// Function returns given argument so it can be used as fluent interface.
        /// In some cases the function may change the identifier (limit identifier length to 30 on Oracle database, e.g.).
        /// </summary>
        public static string Identifier(string name)
        {
            string error = CsUtility.GetIdentifierError(name);
            if (error != null)
                throw new FrameworkException("Invalid database object name: " + error);

            //if (DatabaseLanguageIsOracle.Value)
            //    name = OracleSqlUtility.LimitIdentifierLength(name);

            return name;
        }

        public string QuoteText(string value)
        {
            return value != null
                ? "'" + value.Replace("'", "''") + "'"
                : "NULL";
        }

        public string QuoteIdentifier(string sqlIdentifier)
        {
            if (DatabaseLanguage == "MsSql")
            {
                sqlIdentifier = sqlIdentifier.Replace("]", "]]");
                return "[" + sqlIdentifier + "]";
            }
            throw new FrameworkException("Database language " + DatabaseLanguage + " not supported.");
        }

        public string GetSchemaName(string fullObjectName)
        {
            int dotPosition = fullObjectName.IndexOf('.');
            if (dotPosition == -1)
                if (DatabaseLanguage == "MsSql")
                    return "dbo";
                else if (DatabaseLanguage == "Oracle")
                    throw new FrameworkException("Missing schema name for database object '" + fullObjectName + "'.");
                else
                    throw new FrameworkException(UnsupportedLanguageError);

            var schema = fullObjectName.Substring(0, dotPosition);
            return SqlUtility.Identifier(schema);
        }

        public static string GetShortName(string fullObjectName)
        {
            int dotPosition = fullObjectName.IndexOf('.');
            if (dotPosition == -1)
                return fullObjectName;

            var shortName = fullObjectName.Substring(dotPosition + 1);

            int secondDot = shortName.IndexOf('.');
            if (secondDot != -1 || string.IsNullOrEmpty(shortName))
                throw new FrameworkException("Invalid database object name: '" + fullObjectName + "'. Expected format is 'schema.name' or 'name'.");
            return SqlUtility.Identifier(shortName);
        }

        public string GetFullName(string objectName)
        {
            var schema = GetSchemaName(objectName);
            var name = GetShortName(objectName);
            return schema + "." + name;
        }

        private string UnsupportedLanguageError
        {
            get
            {
                return "SqlUtility functions are not supported for database language '" + DatabaseLanguage + "'."
                    + " Supported database languages are: 'MsSql', 'Oracle'.";
            }
        }

        /// <summary>
        /// Vendor-independent database reader.
        /// </summary>
        public Guid ReadGuid(DbDataReader dataReader, int column)
        {
            if (DatabaseLanguage.Equals("MsSql"))
                return dataReader.GetGuid(column);
            else
                throw new FrameworkException(UnsupportedLanguageError);
        }

        /// <summary>
        /// Vendor-independent database reader.
        /// </summary>
        public int ReadInt(DbDataReader dataReader, int column)
        {
            if (DatabaseLanguage.Equals("MsSql"))
                return dataReader.GetInt32(column);
            else if (DatabaseLanguage.Equals("Oracle"))
                return Convert.ToInt32(dataReader.GetInt64(column)); // On some systems, reading from NUMERIC(10) column will return Int64, and GetInt32 would fail.
            else
                throw new FrameworkException(UnsupportedLanguageError);
        }

        public Guid StringToGuid(string guid)
        {
            if (DatabaseLanguage.Equals("MsSql"))
                return Guid.Parse(guid);
            else if (DatabaseLanguage.Equals("Oracle"))
                return new Guid(StringToByteArray(guid));
            else
                throw new FrameworkException(UnsupportedLanguageError);
        }

        public string QuoteGuid(Guid guid)
        {
            return "'" + GuidToString(guid) + "'";
        }

        public string QuoteGuid(Guid? guid)
        {
            return guid.HasValue
                ? "'" + GuidToString(guid.Value) + "'"
                : "NULL";
        }

        public string GuidToString(Guid? guid)
        {
            return guid.HasValue ? GuidToString(guid.Value) : null;
        }

        public string GuidToString(Guid guid)
        {
            if (DatabaseLanguage.Equals("MsSql"))
                return guid.ToString().ToUpper();
            else if (DatabaseLanguage.Equals("Oracle"))
                return ByteArrayToString(guid.ToByteArray());
            else
                throw new FrameworkException(UnsupportedLanguageError);
        }

        public string QuoteDateTime(DateTime? dateTime)
        {
            return dateTime.HasValue
                ? "'" + DateTimeToString(dateTime.Value) + "'"
                : "NULL";
        }

        public string DateTimeToString(DateTime? dateTime)
        {
            return dateTime.HasValue ? DateTimeToString(dateTime.Value) : null;
        }

        public string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff");
        }

        public string QuoteBool(bool? b)
        {
            return b.HasValue ? BoolToString(b.Value) : "NULL";
        }

        public string BoolToString(bool? b)
        {
            return b.HasValue ? BoolToString(b.Value) : null;
        }

        public string BoolToString(bool b)
        {
            return b ? "0" : "1";
        }

        private string ByteArrayToString(byte[] ba)
        {
            if (ba == null)
                return null;
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }

        private byte[] StringToByteArray(String hex)
        {
            if (hex == null)
                return null;
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            StringReader sr = new StringReader(hex);
            for (int i = 0; i < NumberChars; i++)
                bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            sr.Dispose();
            return bytes;
        }

        /// <summary>
        /// Returns empty string if the string value is null.
        /// This function is used for compatibility between MsSql and Oracle string behavior.
        /// </summary>
        public string EmptyNullString(DbDataReader dataReader, int column)
        {
            if (DatabaseLanguage.Equals("MsSql"))
                return dataReader.GetString(column) ?? "";
            else
                throw new FrameworkException(UnsupportedLanguageError);
        }

        public string MaskPassword(string connectionString)
        {
            var passwordSearchRegex = new[]
            {
                @"\b(password|pwd)\s*=(?<pwd>[^;]*)",
                @"\b/(?<pwd>[^/;=]*)@"
            };

            foreach (var regex in passwordSearchRegex)
            {
                var matches = new Regex(regex, RegexOptions.IgnoreCase).Matches(connectionString);
                for (int i = matches.Count - 1; i >= 0; i--)
                {
                    var pwdGroup = matches[i].Groups["pwd"];
                    if (pwdGroup.Success)
                        connectionString = connectionString
                            .Remove(pwdGroup.Index, pwdGroup.Length)
                            .Insert(pwdGroup.Index, "*");
                }
            }
            return connectionString;
        }

        /// <summary>
        /// Used in DatabaseGenerator to split SQL script generated by IConceptDatabaseDefinition plugins.
        /// </summary>
        public const string ScriptSplitterTag = "/* database generator splitter */";

        /// <summary>
        /// Add this tag to the beginning of the DatabaseGenerator SQL script to execute it without transaction.
        /// Used for special database changes that must be executed without transaction, for example
        /// creating full-text search index.
        /// </summary>
        public const string NoTransactionTag = "/*DatabaseGenerator:NoTransaction*/";

        private static TimeSpan DatabaseTimeDifference = TimeSpan.Zero;
        private static DateTime DatabaseTimeObsoleteAfter = DateTime.MinValue;

        public DateTime GetDatabaseTime(ISqlExecuter sqlExecuter)
        {
            var now = DateTime.Now;
            if (now <= DatabaseTimeObsoleteAfter)
                return now + DatabaseTimeDifference;
            else
            {
                var databaseTime = GetDatabaseTimeFromDatabase(sqlExecuter);
                now = DateTime.Now; // Refreshing current time to avoid including initial SQL connection time.
                DatabaseTimeDifference = databaseTime - now;
                DatabaseTimeObsoleteAfter = now.AddMinutes(1); // Short expiration time to minimize errors on local or database time updates, daylight savings and other.
                return databaseTime;
            }
        }

        private DateTime GetDatabaseTimeFromDatabase(ISqlExecuter sqlExecuter)
        {
            DateTime now;
            if (DatabaseLanguage.Equals("MsSql"))
                now = MsSqlUtility.GetDatabaseTime(sqlExecuter);
            else if (DatabaseLanguage.Equals("Oracle"))
                throw new FrameworkException("GetDatabaseTime function is not yet supported in Rhetos for Oracle database.");
            else
                throw new FrameworkException(UnsupportedLanguageError);
            return DateTime.SpecifyKind(now, DateTimeKind.Local);
        }

        /// <summary>
        /// Splits the script to multiple batches, separated by the GO command.
        /// It emulates the behavior of Microsoft SQL Server utilities, sqlcmd and osql,
        /// but it does not work perfectly: comments near GO and the repeat count are currently not supported.
        /// </summary>
        public static string[] SplitBatches(string sql)
        {
            return batchSplitter.Split(sql).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
        }

        private static readonly Regex batchSplitter = new Regex(@"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }
}
