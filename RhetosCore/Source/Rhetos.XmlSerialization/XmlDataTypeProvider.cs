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
using Rhetos.Dom;
using System.Diagnostics.Contracts;
using Rhetos.Processing;

namespace Rhetos.XmlSerialization
{
    public class XmlDataTypeProvider : IDataTypeProvider
    {
        private readonly IDomainObjectModel _domainObjectModel;

        public XmlDataTypeProvider(IDomainObjectModel domainObjectModel)
        {
            _domainObjectModel = domainObjectModel;
        }

        public IBasicData<T> CreateBasicData<T>(T value)
        {
            return new XmlBasicData<T> { Data = value };
        }

        public IDataArray CreateDataArray<T>(Type type, T[] data) where T : class
        {
            return new XmlDataArray(_domainObjectModel, type, data);
        }

        public IDataArray CreateDomainDataArray(string domainType) 
        {
            var type = _domainObjectModel.Assembly.GetType(domainType);

            if (type == null)
                throw new Exception("DomainObjectModel does not contain type " + domainType + ".");

            return new XmlDataArray(_domainObjectModel, type);
        }
    }
}
