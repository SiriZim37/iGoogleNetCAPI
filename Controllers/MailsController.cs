using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using iGoogleNetCAPI.Class;
using iGoogleNetCAPI.Class.Entities;
using iGoogleNetCAPI.Class.MainClass;
using Microsoft.AspNetCore.Mvc;

namespace iGoogleNetCAPI.Controllers
{

    [Route("api/[Controller]/[action]")]
    [ApiController]
    public class MailsController : Controller
    {

        MainClass mainClass = new MainClass();

        private readonly Class.IMailService mailsService;
        public MailsController(IMailService mailsService)
        {
            this.mailsService = mailsService;
        }

        [HttpPost]
        [ActionName("CenterSendMail")]
        public async Task<IActionResult> CenterSendMail([FromBody] MailModel request)
        {
  
            //Block for Authen  
            string res = string.Empty;
            //check header
            var header = string.Empty;
            string keyCust = string.Empty;
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
                await mailsService.SendEmailAsync(request);
                res = "success";
            }
            catch (Exception ex)
            {
                string err = string.Format(Resource.resperror, header, "", "", "").Replace("(", "{").Replace(")", "}");
             
            }

            return Ok(res);
        }
    
    }
}
