using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeAssignment.DBConnection;

namespace TicTacToeAssignment.Authorization
{
    public class AuthorizeAttribute : ResultFilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
          //  throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var tokenId = context.HttpContext.Request.Headers["tokenId"].ToString();
            bool authorizedUser = SqlAccess.AuthenticateUser(Convert.ToInt32(tokenId));
            if (authorizedUser != true)
                throw new Exception("Invalid user! Wrong TokenId");
        }
    }
}
