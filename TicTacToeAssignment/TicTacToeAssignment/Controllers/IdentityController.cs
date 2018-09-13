using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToeAssignment.DBConnection;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using TicTacToeAssignment.Authorization;

namespace TicTacToeAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class IdentityController : ControllerBase
    { 
        // Add new player
        [Log]
        [HttpPost]
        public void CreateNewUser([FromBody]JObject jsonFormatInput)
        { 

            string FName = jsonFormatInput.GetValue("FName").ToString();
            string LName = jsonFormatInput.GetValue("LName").ToString();
            string UserName = jsonFormatInput.GetValue("UserName").ToString();

            SqlAccess.InsertIntoUserTable(FName, LName, UserName); 
            
        } 
       
    }
}
