using System; 
using Microsoft.AspNetCore.Mvc.Filters;
using TicTacToeAssignment.DBConnection;

namespace TicTacToeAssignment.Authorization
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        Log log = new Log();

        public override void OnException(ExceptionContext actionExecutedContext)
        {
            string abc = log.Request;
            if (actionExecutedContext.Exception is Exception)
            {
                var routeData = actionExecutedContext.RouteData;
                log.Request = "Controller : " + routeData.Values["controller"].ToString() + " Action Name : " + routeData.Values["action"].ToString();
                log.Response = "Failure";
                string tempException = actionExecutedContext.Exception.ToString();
                int index = tempException.IndexOf("\r"); 
                log.Exception = tempException.Substring(0, index);
                log.Comment = "Exception occured!!!";
                SqlAccess.LogEntryForRequest(log);
            }
        }
    }
}
