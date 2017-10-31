using Microsoft.AspNetCore.Http;
using Rhetos.Security;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.AspNetFormsAuth
{
    public class AspNetUserInfo : IUserInfo
    {
        #region IUserInfo implementation

        public bool IsUserRecognized { get { return _isUserRecognized.Value; } }
        public string UserName { get { CheckIfUserRecognized(); return _userName.Value; } }
        public string Workstation { get { CheckIfUserRecognized(); return _workstation.Value; } }
        public string Report() { return UserName + "," + Workstation; }

        #endregion

        private Lazy<bool> _isUserRecognized;
        private Lazy<string> _userName;
        private Lazy<string> _workstation;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetUserInfo(IWindowsSecurity windowsSecurity, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _isUserRecognized = new Lazy<bool>(() =>
                _httpContextAccessor.HttpContext.User != null
                && _httpContextAccessor.HttpContext.User.Identity != null
                && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated);

            _userName = new Lazy<string>(() => _httpContextAccessor.HttpContext.User.Identity.Name);
            _workstation = new Lazy<string>(() => windowsSecurity.GetClientWorkstation());
        }

        private void CheckIfUserRecognized()
        {
            if (!IsUserRecognized)
                throw new ClientException("User is not authenticated.");
        }
    }
}
