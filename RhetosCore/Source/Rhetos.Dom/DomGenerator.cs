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
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Rhetos.Compiler;
using System.Reflection;
using Rhetos.Extensibility;
using Rhetos.Utilities;
using Rhetos.Logging;
using ICodeGenerator = Rhetos.Compiler.ICodeGenerator;
using Rhetos.Compiler.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Rhetos.Dom
{
    public class DomGenerator : IDomainObjectModel, IDomGenerator
    {
        private readonly IPluginsContainer<IConceptCodeGenerator> _pluginRepository;
        private Assembly _objectModel;
        private readonly ICodeGenerator _codeGenerator; //********????***********/
        private readonly ILogProvider _log;
        private readonly IAssemblyGenerator _assemblyGenerator;
        private readonly DomGeneratorOptions _domGeneratorOptions;

        /// <summary>
        /// If assemblyName is not null, the assembly will be saved on disk.
        /// If assemblyName is null, the assembly will be generated in memory.
        /// </summary>
        public DomGenerator(
            IPluginsContainer<IConceptCodeGenerator> plugins,
            ICodeGenerator codeGenerator,
            ILogProvider logProvider,
            IAssemblyGenerator assemblyGenerator,
            DomGeneratorOptions domGeneratorOptions)
        {
            _domGeneratorOptions = domGeneratorOptions;
            _pluginRepository = plugins;
            _codeGenerator = codeGenerator;
            _log = logProvider;
            _assemblyGenerator = assemblyGenerator;
        }

        public Assembly Assembly
        {
            get
            {
                if (_objectModel == null)
                    GenerateObjectModel();
                return _objectModel;
            }
        }

        private void GenerateObjectModel()
        {
            IAssemblySource assemblySource = _codeGenerator.ExecutePlugins(_pluginRepository, "/*", "*/", null);
            _log.GetLogger("Domain Object Model references").Trace(() => string.Join(", ", assemblySource.RegisteredReferences));
            _log.GetLogger("Domain Object Model source").Trace(assemblySource.GeneratedCode);

            CompilerParameter parameter = new CompilerParameter
            {
                OutputAssemblyPath = string.IsNullOrEmpty(Paths.DomAssemblyName) ? null : Path.Combine(Paths.GeneratedFolder, Paths.DomAssemblyName + ".dll"),
                OutputPdbPath = string.IsNullOrEmpty(Paths.DomAssemblyName) ? null : Path.Combine(Paths.GeneratedFolder, Paths.DomAssemblyName + ".pdb"),
                Options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                AssemblyName = Paths.DomAssemblyName
            };

            _objectModel = _assemblyGenerator.Generate(assemblySource, parameter);
        }
    }
}
