using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static iGoogleNetCAPI.Class.Entities.EntityClass;

namespace iGoogleNetCAPI.Class.MainClass
{
    public class Application
    {
        ConnectionClass conn = new ConnectionClass();

        public async Task<string> MyGetDirection(Direction objSyncData, string authen)
        {
       
            string resp = "";
            string resMsg = "success";
            try
            {
                var res = conn.GetDirection(objSyncData); // { "DEST_LAT": "Dest_Lat","DEST_LNG": "Dest_Lat","PLAT": "Start_Lat","PLONG:"Start_lng"}
               resp = string.Format(Resource.respsuccess, resMsg, res, "", "", "", "").Replace("(", "{").Replace(")", "}");
            }
            catch (Exception ex)
            {
                resp = string.Format(Resource.resperror, authen, "").Replace("(", "{").Replace(")", "}");
            }
            return resp;
        }

       


    }
}
