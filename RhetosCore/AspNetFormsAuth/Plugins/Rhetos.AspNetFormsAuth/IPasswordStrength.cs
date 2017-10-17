using Rhetos.Dom.DefaultConcepts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.AspNetFormsAuth
{
    public interface IPasswordStrength : IEntity
    {
        string RegularExpression { get; set; }
        string RuleDescription { get; set; }
    }
}
