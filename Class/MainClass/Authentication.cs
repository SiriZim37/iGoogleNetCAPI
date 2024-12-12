using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static iGoogleNetCAPI.Class.Entities.EntityClass;

namespace iGoogleNetCAPI.Class.MainClass
{
    public class Authentication
    {
        MainClass mainClass = new MainClass();

        public async Task<string> AuthenJWT(AuthorizationModel model, string header)
        {
            string resp = "";
            string resMsg = "success";
            string result = "";
            try
            {

                string jwt = mainClass.GenerateToken(model.UNIQUE_ID);
                resp = string.Format(Resource.respsuccess, resMsg, result, jwt, "").Replace("(", "{").Replace(")", "}");
            }
            catch (Exception ex)
            {
                resp = string.Format(Resource.resperror, "", "").Replace("(", "{").Replace(")", "}");
            }
            return resp;
        }

    }
}
