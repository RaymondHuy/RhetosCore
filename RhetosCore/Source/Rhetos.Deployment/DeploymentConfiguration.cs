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

using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rhetos.Deployment
{
    public class DeploymentConfiguration
    {
        private readonly ILogger _logger;

        public DeploymentConfiguration(ILogProvider logProvider)
        {
            _logger = logProvider.GetLogger(GetType().Name);
        }

        private List<PackageRequest> _packageRequests;
        public IEnumerable<PackageRequest> PackageRequests { get { Initialize(); return _packageRequests; } }

        private List<PackageSource> _packageSources;
        public IEnumerable<PackageSource> PackageSources { get { Initialize(); return _packageSources; } }

        private object _initializationLock = new object();

        private void Initialize()
        {
            lock (_initializationLock)
            {
                _packageRequests = LoadPackageRequest();
                _packageSources = LoadPackageSources();
            }
        }

        private List<PackageRequest> LoadPackageRequest()
        {
            const string configFileUsage = "Edit the file to set which Rhetos packages will be deployed. Note that removing a Rhetos package from the file will uninstall the package.";
            string xml = ReadConfigFileOrCreateTemplate(PackagesConfigurationFileName, PackagesConfigurationTemplateFileName, configFileUsage);
            var xdoc = XDocument.Parse(xml);
            var requests = xdoc.Root.Elements().Select(packageXml =>
                new PackageRequest
                {
                    Id = (string)(packageXml.Attribute("id")),
                    VersionsRange = (string)(packageXml.Attribute("version")),
                    Source = (string)(packageXml.Attribute("source")),
                    RequestedBy = "configuration file " + PackagesConfigurationFileName,
                }).ToList();

            if (requests.Any(r => string.IsNullOrEmpty(r.Id)))
                throw new UserException("Invalid configuration file format '" + PackagesConfigurationFileName + "'. Missing attribute 'id'.");
            if (requests.Count == 0)
                _logger.Error("No packages defined in '" + PackagesConfigurationFileName + "'. " + configFileUsage);
            Version dummy;
            // Simple version format in config file will be converted to a specific version "[ver,ver]", instead of being used as a minimal version (as in NuGet dependencies) in order to conform to NuGet packages.config convention.
            foreach (var request in requests)
                if (request.VersionsRange != null && Version.TryParse(request.VersionsRange, out dummy))
                    request.VersionsRange = string.Format("[{0},{0}]", request.VersionsRange);
            return requests;
        }

        private List<PackageSource> LoadPackageSources()
        {
            const string configFileUsage = "Edit the file to add locations where Rhetos packages can be found.";
            string xml = ReadConfigFileOrCreateTemplate(SourcesConfigurationFileName, SourcesConfigurationTemplateFileName, configFileUsage);
            var xdoc = XDocument.Parse(xml);
            var sources = xdoc.Root.Elements()
                .Select(sourceXml => new PackageSource((string)sourceXml.Attribute("location").Value))
                .ToList();

            if (sources.Count == 0)
                _logger.Info("No sources defined in '" + SourcesConfigurationFileName + "'. " + configFileUsage);
            return sources;
        }

        /// <summary>The file is placed in GetConfigurationFolder().</summary>
        public const string PackagesConfigurationFileName = "RhetosPackages.config";
        private const string PackagesConfigurationTemplateFileName = "Template.RhetosPackages.config";

        /// <summary>The file is placed in GetConfigurationFolder().</summary>
        public const string SourcesConfigurationFileName = "RhetosPackageSources.config";
        private const string SourcesConfigurationTemplateFileName = "Template.RhetosPackageSources.config";

        /// <summary>Folder where the config files are placed.</summary>
        public static string GetConfigurationFolder()
        {
            return Paths.RhetosServerRootPath;
        }

        private string ReadConfigFileOrCreateTemplate(string configFileName, string templateFileName, string configFileUsage)
        {
            string configFilePath = Path.Combine(GetConfigurationFolder(), configFileName);
            string xml;

            if (File.Exists(configFilePath))
                xml = File.ReadAllText(configFilePath, Encoding.Default);
            else
            {
                _logger.Trace(() => "Missing configuration file '" + configFilePath + "'.");

                string templateResourceName = "Rhetos.Deployment." + templateFileName;
                string templateFilePath = Path.Combine(GetConfigurationFolder(), templateFileName);

                var resourceStream = GetType().Assembly.GetManifestResourceStream(templateResourceName);
                if (resourceStream == null)
                    throw new FrameworkException("Cannot find resource '" + templateResourceName + "'.");
                xml = new StreamReader(resourceStream).ReadToEnd();

                File.WriteAllText(templateFilePath, xml, Encoding.UTF8);

                throw new UserException("Missing configuration file '" + configFilePath
                    + "'. Rename the created template file (" + templateFileName
                    + ") to match the expected file name. " + configFileUsage);
            }

            return xml;
        }
    }
}
