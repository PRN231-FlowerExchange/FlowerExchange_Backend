using Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExceptionHandlers
{
    public class GlobalExceptionHandlerOptions
    {
        public GlobalExceptionDetailLevel DetailLevel { get; set; }

        public string GetErrorMessage(Exception ex)
        {
            return DetailLevel switch
            {
                GlobalExceptionDetailLevel.None => "An internal exception has occurred.",
                GlobalExceptionDetailLevel.Message => ex.Message,
                GlobalExceptionDetailLevel.StackTrace => ex.StackTrace,
                GlobalExceptionDetailLevel.ToString => ex.ToString(),
                _ => "An internal exception has occurred.",
            };
        }
    }

    
}
