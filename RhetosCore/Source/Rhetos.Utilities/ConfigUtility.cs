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

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rhetos.Utilities
{
    public static class ConfigUtility
    {
        private static IConfigurationRoot Configuration;

        public static void SetConfiguration(string settingFileName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingFileName);

            //Configuration = builder.Build();
            Configuration = builder.Build();
        }
        /// <summary>
        /// Use "Configuration.GetInt" or "Configuration.GetBool" instead.
        /// Reads the web service configuration from appSettings group in web.config file.
        /// When used in another application (for example, DeployPackages.exe),
        /// the application's ".config" file can be used to override the default settings from the web.config.
        /// </summary>
        public static string GetAppSetting(string key)
        {
            string settingValue = Configuration.GetValue<string>("AppSettings:" + key);

            return settingValue;
        }

        private const string ServerConnectionStringName = "ServerConnectionString";

        public static string GetConnectionStringProvider()
        {
            return Configuration.GetValue<string>(ServerConnectionStringName + ":Provider");
        }

        public static string GetConnectionStringValue()
        {
            return Configuration.GetValue<string>(ServerConnectionStringName + ":Value");
        }

        public static T GetPluginSetting<T>(string pluginName, string path, T defaultValue)
        {
            var value = Configuration.GetValue<T>(pluginName + ":" + path);
            return value != null ? value : defaultValue;
        }
    }
}
