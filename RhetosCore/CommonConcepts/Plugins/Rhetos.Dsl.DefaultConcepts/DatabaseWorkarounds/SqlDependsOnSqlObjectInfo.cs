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
using System.Linq;
using System.Text;

namespace Rhetos.Dsl.DefaultConcepts
{
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("SqlDependsOnSqlObject")]
    public class SqlDependsOnSqlObjectInfo : IConceptInfo
    {
        [ConceptKey]
        public IConceptInfo Dependent { get; set; }
        [ConceptKey]
        public SqlObjectInfo DependsOn { get; set; }

        public override string ToString()
        {
            return Dependent + " depends on " + DependsOn;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
