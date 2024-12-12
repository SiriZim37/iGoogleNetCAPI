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
    [Route("api/[controller]/[action]")]
    [ApiController]

        public class AuthorizationController : ControllerBase
        {
            
            Authentication authentication = new Authentication();


            [HttpPost]
            [ActionName("GetJsonWebToken")]
            public async Task<IActionResult> GetJsonWebToken([FromBody] AuthorizationModel _syncData)
            {
                string res = "";
                var header = "";
                string keyCust = "";
                try
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    if (!authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase)
                      && !authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                    {
                        return Unauthorized();
                    }
                    header = authHeader.Parameter;
                }
                catch (Exception ex)
                {
                    return Unauthorized();
                }
                try
                {
                    res = await authentication.AuthenJWT(_syncData , header);
                }
                catch (Exception ex)
                {
                    string err = string.Format(Resource.resperror, header, keyCust, "", "").Replace("(", "{").Replace(")", "}");
                }
                return Ok(res);
            }
    }
}
