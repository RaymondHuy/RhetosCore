﻿using NuGet.Packaging;
using NuGet.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Rhetos.Deployment
{
    public class ProjectContext : INuGetProjectContext
    {
        public void Log(MessageLevel level, string message, params object[] args)
        {
            // Do your logging here...
        }

        public FileConflictAction ResolveFileConflict(string message) => FileConflictAction.Ignore;

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public XDocument OriginalPackagesConfig { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider => null;

        public ExecutionContext ExecutionContext => null;

        public void ReportError(string message)
        {
        }

        public NuGetActionType ActionType { get; set; }

        public TelemetryServiceHelper TelemetryService
        {
            get { return new TelemetryServiceHelper(); }
            set { }
        }
    }
}
