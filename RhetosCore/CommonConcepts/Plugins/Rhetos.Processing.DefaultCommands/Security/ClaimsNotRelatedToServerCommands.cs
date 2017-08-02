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

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;
using Rhetos.Security;

namespace Rhetos.Processing.DefaultCommands
{
    [Export(typeof(IClaimProvider))]
    [ExportMetadata(MefProvider.Implements, typeof(DummyCommandInfo))]
    public class ClaimsNotRelatedToServerCommands : IClaimProvider
    {
        public IList<Claim> GetRequiredClaims(ICommandInfo info)
        {
            throw new FrameworkException("Unexpected method call. This class is not based on an executable server command.");
        }

        public IList<Claim> GetAllClaims(IDslModel dslModel)
        {
            var claims = new List<Claim>();

            claims.AddRange(dslModel.Concepts.OfType<CustomClaimInfo>()
                .Select(item => new Claim(item.ClaimResource, item.ClaimRight)));

            return claims;
        }
    }

    [Export(typeof(ICommandInfo))]
    public class DummyCommandInfo : ICommandInfo
    {
    }
}