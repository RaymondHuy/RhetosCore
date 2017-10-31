using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.AspNetFormsAuth.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsPersisted { get; set; }
    }
}
