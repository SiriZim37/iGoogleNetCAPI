using iGoogleNetCAPI.Class.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace iGoogleNetCAPI.Class.MailClass
{
    public class MailService : IMailService
    {


        MailConfigModel mailConfigModel = new MailConfigModel();

        public async Task SendEmailAsync(MailModel model)
        {
            try
            {
                using var smtp = new SmtpClient();
                smtp.Host = mailConfigModel.Host;
                smtp.EnableSsl = Convert.ToBoolean(true);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = mailConfigModel.UserName;
                NetworkCred.Password = mailConfigModel.Password;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = mailConfigModel.Port;

                MailMessage message = new MailMessage(model.MailFrom, model.MailTo);
                if (!string.IsNullOrEmpty(model.MailCC))
                {
                    message.CC.Add(model.MailCC);
                }
                if (!string.IsNullOrEmpty(model.MailBCC))
                {
                    message.Bcc.Add(model.MailBCC);
                }

                message.Subject = model.MSubject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Body = model.MBody;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                //for attach file with base64
                if (model.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in model.Attachments)
                    {
                        if (file.filedata.Length > 0)
                        {
                            fileBytes = Convert.FromBase64String(file.filedata);

                            Attachment att = new Attachment(new MemoryStream(fileBytes), file.filename);
                            message.Attachments.Add(att);

                        }
                    }
                }
                smtp.Send(message);
                message.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     


    }
}
