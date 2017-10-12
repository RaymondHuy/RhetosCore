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

using Autofac;
using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Rhetos.Extensibility
{
    internal static class PluginScanner
    {
        /// <summary>
        /// The key is FullName of the plugin's export type (it is usually the interface it implements).
        /// </summary>
        private static MultiDictionary<string, PluginInfo> _pluginsByExport = null;
        private static object _pluginsLock = new object();

        /// <summary>
        /// Returns plugins that are registered for the given interface, sorted by dependencies (MefPovider.DependsOn).
        /// </summary>
        internal static IEnumerable<PluginInfo> FindPlugins(ContainerBuilder builder, Type pluginInterface)
        {
            try
            {
                lock (_pluginsLock)
                {
                    if (_pluginsByExport == null)
                    {
                        var assemblies = ListAssemblies();
                        LoadExternalAssembliesToCurrentContext(assemblies);
                        _pluginsByExport = LoadPlugins(assemblies);
                    }

                    return _pluginsByExport.Get(pluginInterface.FullName);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                throw new FrameworkException(CsUtility.ReportTypeLoadException(ex, "Cannot load plugins."), ex);
            }
        }

        private static List<string> ListAssemblies()
        {
            var stopwatch = Stopwatch.StartNew();

            string[] pluginsPath = new[] { Paths.PluginsFolder, Paths.GeneratedFolder, Paths.ExternalApiModulesFolder };

            List<string> assemblies = new List<string>();
            foreach (var path in pluginsPath)
                if (File.Exists(path))
                    assemblies.Add(Path.GetFullPath(path));
                else if (Directory.Exists(path))
                    assemblies.AddRange(Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories));
            // If the path does not exist, it may be generated later (see DetectAndRegisterNewModulesAndPlugins).

            assemblies.Sort();

            foreach (var assembly in assemblies)
                InitializationLogging.Logger.Trace(() => "Found assembly: " + assembly);

            InitializationLogging.PerformanceLogger.Write(stopwatch, "MefPluginScanner: Listed assemblies (" + assemblies.Count + ").");
            return assemblies;
        }

        private static MultiDictionary<string, PluginInfo> LoadPlugins(List<string> assemblies)
        {
            var stopwatch = Stopwatch.StartNew();
            var pluginsByExport = new MultiDictionary<string, PluginInfo>();

            var assemblyReferences = AppDomain.CurrentDomain.GetAssemblies()
                                    .Where(a => !a.IsDynamic)
                                    .ToArray();

            foreach (var assembly in assemblies)
            {
                var types = assemblyReferences.Where(a => a.Location == assembly).FirstOrDefault()?.GetTypes();
                if (types == null)
                    continue;
                foreach (var type in types)
                {
                    string contractName = String.Empty;
                    var plugin = new PluginInfo();
                    var exportAttribute = (ExportAttribute)type.GetCustomAttribute(typeof(ExportAttribute));
                    if (exportAttribute != null)
                    {
                        plugin.Metadata = new Dictionary<string, object>();
                        if (exportAttribute.ContractType != null)
                        {
                            contractName = exportAttribute.ContractType.FullName;
                            plugin.Type = type;

                            if (type.GetCustomAttributes(typeof(ExportMetadataAttribute)) != null)
                            {
                                var exportMetadatas = (IEnumerable<ExportMetadataAttribute>)type.GetCustomAttributes(typeof(ExportMetadataAttribute));
                                foreach (var item in exportMetadatas)
                                {
                                    plugin.Metadata.Add(item.Name, item.Value);
                                }
                            }
                            plugin.Metadata.Add("ExportTypeIdentity", exportAttribute.ContractType);
                        }
                        else
                        {
                            contractName = type.FullName;
                            plugin.Type = type;
                            plugin.Metadata.Add("ExportTypeIdentity", exportAttribute.ContractType);
                        }
                        pluginsByExport.Add(contractName, plugin);
                    }
                }
            }

            foreach (var pluginsGroup in pluginsByExport)
                SortByDependency(pluginsGroup.Value);

            InitializationLogging.PerformanceLogger.Write(stopwatch, "MefPluginScanner: Loaded plugins (" + pluginsByExport.Count + ").");
            return pluginsByExport;
        }

        private static void SortByDependency(List<PluginInfo> plugins)
        {
            var dependencies = plugins
                .Where(p => p.Metadata.ContainsKey(MefProvider.DependsOn))
                .Select(p => Tuple.Create((Type)p.Metadata[MefProvider.DependsOn], p.Type))
                .ToList();

            var pluginTypes = plugins.Select(p => p.Type).ToList();
            Graph.TopologicalSort(pluginTypes, dependencies);
            Graph.SortByGivenOrder(plugins, pluginTypes, p => p.Type);
        }

        internal static void ClearCache()
        {
            lock (_pluginsLock)
                _pluginsByExport = null;
        }

        private static void LoadExternalAssembliesToCurrentContext(List<string> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(assembly);
            }
        }
    }
}
