using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeAssignment.DBConnection;

namespace TicTacToeAssignment.Authorization
{
    public class LogAttribute : ResultFilterAttribute, IActionFilter
    {
        Log log = new Log();
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                log.Response = "Success";
                log.Exception = "N/A";
                log.Comment = "Request successfully executed!";
                SqlAccess.LogEntryForRequest(log);
            } 
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { 
            var routeData = context.RouteData;
            log.Request = "Controller : "+routeData.Values["controller"].ToString() + " Action Name : "+ routeData.Values["action"].ToString(); 
             
        }
    }
}
