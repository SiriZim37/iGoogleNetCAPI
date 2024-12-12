using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using iGoogleNetCAPI.Class.MainClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static iGoogleNetCAPI.Class.Entities.EntityClass;

namespace iGoogleNetCAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        Notifications notification = new Notifications();
        MainClass mainClass = new MainClass();

        [HttpPost]
        [ActionName("PushNotification")]
        public async Task<IActionResult> PushNotification([FromBody] NotifyModel _syncData)
        {
            string res = "";
            //check header
            var header = "";
            string keyCust = "";
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (!authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized();
                }
                header = authHeader.Parameter;
                keyCust = mainClass.DecryptionToken(authHeader.Parameter);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
            try
            {
                if (_syncData != null)
                {
                    res = await notification.MyPushNotify(_syncData);
                }
                else
                {
                    string err = string.Format(Resource.respinvalid, header, "", "", "").Replace("(", "{").Replace(")", "}");
                }
          
            }
            catch (Exception ex)
            {
                string err = string.Format(Resource.resperror, header, "", "", "").Replace("(", "{").Replace(")", "}");
            }
            return Ok(res);
        }


        [HttpPost]
        [ActionName("PushNotificationHuawei")]
        public async Task<IActionResult> PushNotificationHuawei([FromBody] NotifyModel _syncData)
        {
            string res = "";
            //check header
            var header = "";
            string keyCust = "";
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (!authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized();
                }
                header = authHeader.Parameter;
                keyCust = mainClass.DecryptionToken(authHeader.Parameter);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
            try
            {
                if (_syncData != null)
                {
                    res = await notification.MyPushNotify(_syncData);
                }
                else
                {
                    string err = string.Format(Resource.respinvalid, header, "", "", "").Replace("(", "{").Replace(")", "}");
                }

            }
            catch (Exception ex)
            {
                string err = string.Format(Resource.resperror, header, "", "", "").Replace("(", "{").Replace(")", "}");
            }
            return Ok(res);
        }
    }
}
