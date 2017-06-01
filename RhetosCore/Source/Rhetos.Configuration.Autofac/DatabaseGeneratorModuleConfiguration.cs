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

using Autofac;
using Rhetos.DatabaseGenerator;
using Rhetos.Extensibility;

namespace Rhetos.Configuration.Autofac
{
    public class DatabaseGeneratorModuleConfiguration : Module
    {
        private readonly bool _shortTransactions;

        public DatabaseGeneratorModuleConfiguration(bool shortTransactions)
        {
            _shortTransactions = shortTransactions;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConceptApplicationRepository>().As<IConceptApplicationRepository>();
            builder.RegisterType<DatabaseGenerator.DatabaseGenerator>().As<IDatabaseGenerator>();
            builder.RegisterInstance(new DatabaseGeneratorOptions { ShortTransactions = _shortTransactions });
            Plugins.FindAndRegisterPlugins<IConceptDatabaseDefinition>(builder);
            builder.RegisterType<NullImplementation>().As<IConceptDatabaseDefinition>();

            base.Load(builder);
        }
    }
}
