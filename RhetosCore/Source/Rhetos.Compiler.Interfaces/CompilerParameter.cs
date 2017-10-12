using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.Compiler.Interfaces
{
    public class CompilerParameter
    {
        public CSharpCompilationOptions Options { get; set; }

        public string OutputAssemblyPath { get; set; }

        public string OutputPdbPath { get; set; }

        public string AssemblyName { get; set; }
    }
}
