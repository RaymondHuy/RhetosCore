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

using Microsoft.CSharp.RuntimeBinder;
using Rhetos.Compiler;
using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;
using Rhetos.Utilities;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

//Important
namespace Rhetos.Dom.DefaultConcepts
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(InitializationConcept))]
    public class DomInitializationCodeGenerator : IConceptCodeGenerator
    {
        public static readonly string EntityFrameworkContextMembersTag = "/*EntityFrameworkContextMembers*/";
        public static readonly string EntityFrameworkContextInitializeTag = "/*EntityFrameworkContextInitialize*/";
        public static readonly string EntityFrameworkOnModelCreatingTag = "/*EntityFrameworkOnModelCreating*/";
        public static readonly string EntityFrameworkConfigurationTag = "/*EntityFrameworkConfiguration*/";
        public static readonly string CommonQueryableMemebersTag = "/*CommonQueryableMemebers*/";
        public static readonly string QueryExtensionsMembersTag = "/*QueryExtensionsMembers*/";

        public static readonly string StandardNamespacesSnippet =
@"using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using Rhetos.Dom.DefaultConcepts;
    using Rhetos.Utilities;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel;";
        

        public DomInitializationCodeGenerator()
        {
        }

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (InitializationConcept)conceptInfo;

            codeBuilder.InsertCode(GenerateCommonClassesSnippet());

            if (bool.Parse(ConfigUtility.GetAppSetting("EntityFramework.UseDatabaseNullSemantics")) == true)
                codeBuilder.InsertCode("this.Configuration.UseDatabaseNullSemantics = true;\r\n            ", EntityFrameworkContextInitializeTag);
            
            // Types used in the preceding code snippet:
            codeBuilder.AddReferencesFromDependency(typeof(Autofac.Module)); // Includes a reference to Autofac.dll.
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Extensibility.INamedPlugins<>));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Utilities.IUserInfo));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Utilities.ISqlExecuter));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Security.IAuthorizationManager));
            codeBuilder.AddReferencesFromDependency(typeof(System.Composition.ExportAttribute));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Dom.DefaultConcepts.GenericRepositories));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Logging.ILogProvider));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Security.IWindowsSecurity));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Utilities.SqlUtility));
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.EntityFrameworkCore.DbContext));
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions));
            codeBuilder.AddReferencesFromDependency(typeof(HashSet<>));
            codeBuilder.AddReferencesFromDependency(typeof(IQueryable));
            codeBuilder.AddReferencesFromDependency(typeof(Queryable));
            codeBuilder.AddReferencesFromDependency(typeof(System.ComponentModel.TypeConverter));
            codeBuilder.AddReferencesFromDependency(typeof(System.Console));
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.Extensions.DependencyInjection.ServiceCollection));
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.Extensions.DependencyInjection.IServiceCollection));
            codeBuilder.AddReferencesFromDependency(typeof(System.IServiceProvider));
            codeBuilder.AddReferencesFromDependency(typeof(Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions));
            codeBuilder.AddReferencesFromDependency(typeof(Autofac.Extensions.DependencyInjection.AutofacRegistration));

            //codeBuilder.AddReferencesFromDependency(typeof(System.Data.Entity.SqlServer.SqlProviderServices));
            //codeBuilder.AddReferencesFromDependency(typeof(System.Data.Entity.Core.EntityClient.EntityConnection));
            //codeBuilder.AddReferencesFromDependency(typeof(System.Data.Entity.Core.Metadata.Edm.MetadataWorkspace));
            //codeBuilder.AddReferencesFromDependency(typeof(System.Data.Entity.Infrastructure.IObjectContextAdapter));
            //codeBuilder.AddReferencesFromDependency(typeof(System.Data.Entity.Core.Objects.ObjectStateEntry));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Persistence.IPersistenceCache));
            codeBuilder.AddReferencesFromDependency(typeof(Rhetos.Persistence.IPersistenceTransaction));

            codeBuilder.AddReferencesFromDependency(typeof(System.Composition.ExportAttribute));
        }

        private static string GenerateCommonClassesSnippet()
        {
            return
@"namespace Common
{
    " + StandardNamespacesSnippet + @"

    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    " + ModuleCodeGenerator.CommonUsingTag + @"

    public class DomRepository
    {
        private readonly Rhetos.Extensibility.INamedPlugins<IRepository> _repositories;

        public DomRepository(Rhetos.Extensibility.INamedPlugins<IRepository> repositories)
        {
            _repositories = repositories;
        }

        " + ModuleCodeGenerator.CommonDomRepositoryMembersTag + @"
    }

    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(
            Rhetos.Persistence.IPersistenceTransaction persistenceTransaction,
            DbContextOptions<EntityFrameworkContext> options
            //Rhetos.Dom.DefaultConcepts.Persistence.EntityFrameworkMetadata metadata
        ) // EntityFrameworkConfiguration is provided as an IoC dependency for EntityFrameworkContext in order to initialize the global DbConfiguration before using DbContext.
            : base(options)
        {
            Initialize();
            Database.UseTransaction(persistenceTransaction.Transaction);
        }

        /// <summary>
        /// This constructor is used at deployment-time to create slow EntityFrameworkContext instance before the metadata files are generated.
        /// The instance is used by EntityFrameworkGenerateMetadataFiles to generate the metadata files.
        /// </summary>
        //protected EntityFrameworkContext(
        //    System.Data.Common.DbConnection connection,
        //    EntityFrameworkConfiguration configuration) // EntityFrameworkConfiguration is provided as an IoC dependency for EntityFrameworkContext in order to initialize the global DbConfiguration before using DbContext.
        //    : base(connection, true)
        //{
        //    Initialize();
        //}

        private void Initialize()
        {
            //System.Data.Entity.Database.SetInitializer<EntityFrameworkContext>(null); // Prevent EF from creating database objects.

            " + EntityFrameworkContextInitializeTag + @"
            //Database.SetCommandTimeout = Rhetos.Utilities.SqlUtility.SqlCommandTimeout;
        }

        public void ClearCache()
        {
        //    var objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext;

        //    SetDetaching(true);
        //    try
        //    {
        //        ChangeTracker.AutoDetectChangesEnabled = false;
        //        var trackedItems = ChangeTracker.Entries().ToList();
        //        foreach (var item in trackedItems)
        //            objectContext.Detach(item.Entity);
        //        ChangeTracker.AutoDetectChangesEnabled = true;
        //    }
        //    finally
        //    {
        //        SetDetaching(false);
        //    }
        }

        public void ClearCache(object item)
        {
        //    var objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext;
        //    System.Data.Entity.Core.Objects.ObjectStateEntry stateEntry;
        //    bool isCached = objectContext.ObjectStateManager.TryGetObjectStateEntry(item, out stateEntry);

        //    if (isCached)
        //    {
        //        SetDetaching(true);
        //        try
        //        {
        //            ChangeTracker.AutoDetectChangesEnabled = false;
        //            objectContext.Detach(item);
        //            ChangeTracker.AutoDetectChangesEnabled = true;
        //        }
        //        finally
        //        {
        //            SetDetaching(false);
        //        }
        //    }
        }

        private void SetDetaching(bool detaching)
        {
        //    foreach (var item in ChangeTracker.Entries().Select(entry => entry.Entity).OfType<IDetachOverride>())
        //        item.Detaching = detaching;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            " + EntityFrameworkOnModelCreatingTag + @"
        }

        " + EntityFrameworkContextMembersTag + @"
    }

    //public class EntityFrameworkConfiguration : System.Data.Entity.DbConfiguration
    //{
    //    public EntityFrameworkConfiguration()
    //    {
    //        SetProviderServices(""System.Data.SqlClient"", System.Data.Entity.SqlServer.SqlProviderServices.Instance);

    //        " + EntityFrameworkConfigurationTag + @"

    //        System.Data.Entity.DbConfiguration.SetConfiguration(this);
    //    }
    //}

    public static class Infrastructure
    {
        public static readonly RegisteredInterfaceImplementations RegisteredInterfaceImplementationName = new RegisteredInterfaceImplementations
        {
            " + ModuleCodeGenerator.RegisteredInterfaceImplementationNameTag + @"
        };

        public static readonly ApplyFiltersOnClientRead ApplyFiltersOnClientRead = new ApplyFiltersOnClientRead
        {
            " + ModuleCodeGenerator.ApplyFiltersOnClientReadTag + @"
        };

        public static void MaterializeItemsToSave<T>(ref IEnumerable<T> items) where T : IEntity, new()
        {
            if (items == null)
                items = Enumerable.Empty<T>();
            else if (items is System.Linq.IQueryable)
                throw new Rhetos.FrameworkException(""The Save method for '"" + typeof(T).FullName + ""' does not support the argument type '"" + items.GetType().Name + ""'. Use a List or an Array."");
            else if (!(items is System.Collections.IList))
                items = items.ToList();
        }

        public static void MaterializeItemsToDelete<T>(ref IEnumerable<T> items) where T : IEntity, new()
        {
            if (items == null)
                items = Enumerable.Empty<T>();
            if (items is IQueryable<IEntity>)
                // IQueryable Select will generate a better SQL query instead. IEnumerable Select would load all columns.
                items = ((IQueryable<IEntity>)items).Select(item => new T { ID = item.ID }).ToList();
            else if (!(items is System.Collections.IList))
                items = items.Select(item => new T { ID = item.ID }).ToList();
        }

        " + ModuleCodeGenerator.CommonInfrastructureMembersTag + @"
    }

    public class ExecutionContext
    {
        protected Lazy<Rhetos.Persistence.IPersistenceTransaction> _persistenceTransaction;
        public Rhetos.Persistence.IPersistenceTransaction PersistenceTransaction { get { return _persistenceTransaction.Value; } }

        protected Lazy<Rhetos.Utilities.IUserInfo> _userInfo;
        public Rhetos.Utilities.IUserInfo UserInfo { get { return _userInfo.Value; } }

        protected Lazy<Rhetos.Utilities.ISqlExecuter> _sqlExecuter;
        public Rhetos.Utilities.ISqlExecuter SqlExecuter { get { return _sqlExecuter.Value; } }

        protected Lazy<Rhetos.Security.IAuthorizationManager> _authorizationManager;
        public Rhetos.Security.IAuthorizationManager AuthorizationManager { get { return _authorizationManager.Value; } }

        protected Lazy<Rhetos.Dom.DefaultConcepts.GenericRepositories> _genericRepositories;
        public Rhetos.Dom.DefaultConcepts.GenericRepositories GenericRepositories { get { return _genericRepositories.Value; } }

        public Rhetos.Dom.DefaultConcepts.GenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class, IEntity
        {
            return GenericRepositories.GetGenericRepository<TEntity>();
        }

        public Rhetos.Dom.DefaultConcepts.GenericRepository<TEntity> GenericRepository<TEntity>(string entityName) where TEntity : class, IEntity
        {
            return GenericRepositories.GetGenericRepository<TEntity>(entityName);
        }

        public Rhetos.Dom.DefaultConcepts.GenericRepository<IEntity> GenericRepository(string entityName)
        {
            return GenericRepositories.GetGenericRepository(entityName);
        }

        protected Lazy<Common.DomRepository> _repository;
        public Common.DomRepository Repository { get { return _repository.Value; } }

        public Rhetos.Logging.ILogProvider LogProvider { get; private set; }

        protected Lazy<Rhetos.Security.IWindowsSecurity> _windowsSecurity;
        public Rhetos.Security.IWindowsSecurity WindowsSecurity { get { return _windowsSecurity.Value; } }

        public EntityFrameworkContext EntityFrameworkContext { get; private set; }

        " + ModuleCodeGenerator.ExecutionContextMemberTag + @"

        // This constructor is used for automatic parameter injection with autofac.
        public ExecutionContext(
            Lazy<Rhetos.Persistence.IPersistenceTransaction> persistenceTransaction,
            Lazy<Rhetos.Utilities.IUserInfo> userInfo,
            Lazy<Rhetos.Utilities.ISqlExecuter> sqlExecuter,
            Lazy<Rhetos.Security.IAuthorizationManager> authorizationManager,
            Lazy<Rhetos.Dom.DefaultConcepts.GenericRepositories> genericRepositories,
            Lazy<Common.DomRepository> repository,
            Rhetos.Logging.ILogProvider logProvider,
            Lazy<Rhetos.Security.IWindowsSecurity> windowsSecurity" + ModuleCodeGenerator.ExecutionContextConstructorArgumentTag + @",
            EntityFrameworkContext entityFrameworkContext)
        {
            _persistenceTransaction = persistenceTransaction;
            _userInfo = userInfo;
            _sqlExecuter = sqlExecuter;
            _authorizationManager = authorizationManager;
            _genericRepositories = genericRepositories;
            _repository = repository;
            LogProvider = logProvider;
            _windowsSecurity = windowsSecurity;
            EntityFrameworkContext = entityFrameworkContext;
            " + ModuleCodeGenerator.ExecutionContextConstructorAssignmentTag + @"
        }

        // This constructor is used for manual context creation (unit testing)
        //public ExecutionContext()
        //{
        //    Console.WriteLine(""Default ExecutionContext"");
        //}
    }

    [System.Composition.Export(typeof(Autofac.Module))]
    public class AutofacModuleConfiguration : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            Console.WriteLine(""AutofacModuleConfiguration serverdom loaded"");

            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            services.AddDbContext<EntityFrameworkContext>(
                options => options.UseSqlServer(ConfigUtility.GetConnectionStringValue()));
            builder.Populate(services);
            builder.RegisterType<DomRepository>().InstancePerLifetimeScope();

            //builder.RegisterType<EntityFrameworkContext>()
            //    .As<EntityFrameworkContext>()
            //    .As<DbContext>()
            //    .As<Rhetos.Persistence.IPersistenceCache>()
            //    .InstancePerLifetimeScope();
            builder.RegisterType<ExecutionContext>().InstancePerLifetimeScope();
            builder.RegisterInstance(Infrastructure.RegisteredInterfaceImplementationName).ExternallyOwned();
            builder.RegisterInstance(Infrastructure.ApplyFiltersOnClientRead).ExternallyOwned();
            " + ModuleCodeGenerator.CommonAutofacConfigurationMembersTag + @"

            base.Load(builder);
        }
    }

    " + ModuleCodeGenerator.CommonNamespaceMembersTag + @"
}

namespace Common.Queryable
{
    " + StandardNamespacesSnippet + @"

    " + CommonQueryableMemebersTag + @"
}

namespace Rhetos.Dom.DefaultConcepts
{
    " + StandardNamespacesSnippet + @"

    public static class QueryExtensions
    {
        " + QueryExtensionsMembersTag + @"
    }
}
";
        }
    }
}
