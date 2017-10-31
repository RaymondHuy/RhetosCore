using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.Security
{
    public class WindowsSecurity : IWindowsSecurity
    {
        public string GetClientWorkstation()
        {
            return "";
        }

        public IEnumerable<string> GetIdentityMembership(string username)
        {
            return null;
        }

        public bool IsBuiltInAdministrator(IWindowsUserInfo userInfo)
        {
            return false;
        }
    }
}
