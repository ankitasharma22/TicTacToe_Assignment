using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeAssignment
{
    public class GenerateToken
    {
        public static int GetUniqueToken()
        {
            Guid guid = new Guid();

            string tempGuid = guid.ToString();
            int token = 0;
            for (int i = 0; i < tempGuid.Length; i++)
            {
                token = token + tempGuid[i];
            }
            return token;
        }
    }
}
