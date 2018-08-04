using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Service.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GlobalExceptionFilter(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public override void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                return;
            }

            var response = new ErrorResponseModel()
            {
                Message = context.Exception.Message,
                StackTrace = context.Exception.StackTrace,
                ErrorCode = 500
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = response.ErrorCode,
                DeclaredType = typeof(ErrorResponseModel)
            };
        }

        public class ErrorResponseModel
        {
            public string Message { get; set; }

            public string StackTrace { get; set; }

            public int ErrorCode { get; set; }
        }
    }
}
