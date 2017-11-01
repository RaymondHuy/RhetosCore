using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.Extensibility
{
    /// <summary>
    /// This interface is used for initializing external database. 
    /// Consider using it when you want to create more tables by yourself without using Rhetos DSL scripts.
    /// </summary>
    public interface IDatabaseInitializer
    {
        void InitializeAndMigrateData();
    }
}
