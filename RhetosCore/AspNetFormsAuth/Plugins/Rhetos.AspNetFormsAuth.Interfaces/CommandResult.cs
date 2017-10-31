using System;
using System.Collections.Generic;
using System.Text;

namespace Rhetos.AspNetFormsAuth.Interfaces
{
    public class CommandResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public static CommandResult Success()
        {
            return new CommandResult()
            {
                IsSuccess = true,
                Message = string.Empty
            };
        }

        public static CommandResult Failed(string message)
        {
            return new CommandResult()
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
}
