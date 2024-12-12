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
    public class ApplicationsController : Controller
    {
        MainClass mainClass = new MainClass();


        Application application = new Application();

        [HttpPost]
        [ActionName("GetDirection")]
        public async Task<IActionResult> GetDirection([FromBody] Direction _syncData)
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
                keyCust = mainClass.DecryptionToken(header);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
            try
            {

                res = await application.MyGetDirection(_syncData, header);

            }
            catch (Exception ex)
            {

                string err = string.Format(Resource.resperror, header, keyCust, "", "").Replace("(", "{").Replace(")", "}");
            }
            return Ok(res);
        }
    }
}
