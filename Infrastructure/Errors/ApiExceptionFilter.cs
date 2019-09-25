namespace ELOTEC.Infrastructure.Errors
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using ELOTEC.Models;
    using System.Diagnostics;
    using ELOTEC.Infrastructure.Common;
    using ELOTEC.Infrastructure.Constants;
    using ELOTEC.Infrastructure.Helpers;
    using System.Data.SqlClient;

    public class ApiExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ResultObject result = new ResultObject();
            string message = string.Empty;


            if (context.Exception is UnauthorizedAccessException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                message = "Unauthorized Access";
            }
            else
            {
                message = string.IsNullOrWhiteSpace(context.Exception.Message) ? "Error" : context.Exception.Message;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            Log log = new Log
            {
                LogGuid = Guid.NewGuid(),
                LogDate = DateTime.Now,
                Machine = Environment.MachineName,
                Application = context.Exception.GetType().FullName,
                ProcessName = new StackTrace(context.Exception).GetFrame(0).GetMethod().Name,
                LogText = context.Exception.ToString()
            };

            if (context.Exception is SqlException)
            {
                Helper.WriteLog(context.Exception);
                message = "Database connection failed.";
            }
            //else
            //{
            //    _logProvider.SaveLog(log);
            //}

            result[ResultKey.Success] = false;
            result[ResultKey.Error] = message;
            context.Result = new JsonResult(result);

            base.OnException(context);
        }
    }
}
