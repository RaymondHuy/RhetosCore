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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhetos.Utilities
{
    public interface ISqlUtility
    {
        /// <summary>
        /// Checks the exception for database errors and attempts to transform it to a RhetosException.
        /// It the function returns null, the original exception should be used.
        /// </summary>
        RhetosException InterpretSqlException(Exception exception);

        /// <summary>
        /// Simplifies ORM exception by detecting the SQL exception that caused it.
        /// </summary>
        Exception ExtractSqlException(Exception exception);
    }
}
