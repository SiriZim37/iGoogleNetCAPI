
using System;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using static iGoogleNetCAPI.Class.Entities.EntityClass;
using System.Text.Json;
using System.Collections;

namespace iGoogleNetCAPI.Class.MainClass
{
    public class Notifications
    {

            MainClass objMain = new MainClass();

            public async Task<string> MyPushNotify(NotifyModel model)
            {
            
                string resMsg = "success";
                string dataPush = "";
                string typeDevice = "ANDROID";
                int sendSuccess = 0;
                string fcm_token = model.FCM;
                string resp = "success";
            try
                {
                //call main screen

                    if (typeDevice == "ANDROID")
                    {
                        dataPush = "";//  { "to":"fcm_token","priority":"high","content_available":true,"mutable_content":true,,"data":{"msg_type":"","msg":"Test notification ", "navigation": "{schema}:{screen(host)}?{object}", "image":"","dateTime":"30/10/2020 09:16:55"} }
                    }
                    else if (typeDevice == "IOS")
                    {
                        dataPush = "";   //  { "to":"fcm_token","priority":"high","content_available":true,"mutable_content":true,"notification":{ "title":"title","body":"Test notification","sound":"default"},"data":{"msg_type":"","msg":"Test notification ", "navigation": "{schema}:{screen(host)}?{object}", "image":"","dateTime":"30/10/2020 09:16:55"} }
                    }

                    var pushStatus = "N";
                    var resPush = SendNotification(dataPush);
                    if (await resPush)
                    {
                        sendSuccess += 1;
                        pushStatus = "Y";
                    }
                    Console.WriteLine($"Notification sent");
              
                    resp = string.Format(Resource.respsuccess, resMsg,  "sent=" + sendSuccess, "", "", "", "").Replace("(", "{").Replace(")", "}");

                }
                catch (Exception ex)
                {
                    resp = string.Format(Resource.resperror, "", "").Replace("(", "{").Replace(")", "}");
                }
                return resp;
            }


        public async Task<string> MyPushNotifyHMS(NotifyModel model)
        {

            string resMsg = "success";
            string dataPush = "";
            string typeDevice = "HUAWEI";
            int sendSuccess = 0;
            string fcm_token = model.FCM;
            string resp = "success";
            try
            {
                //call main screen
              

                var hms_token = ("token").Trim();
                 dataPush = "data"; //{ "validate_only": false,"message": {"data": "{'title':'TLTSimply'}", "token": ["token""]}  }
                //string str = "{'data_message':'data_message'"  +  ",'dateTime':'" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "'}";
                //ArrayList token = new ArrayList();
                //token.Add(hms_token);
                //dataPush = "{}"
                //var serializer = new JavaScriptSerializer();
                //json = serializer.Serialize(data);
                //json = json.ToString.Replace(@"\u0027", "'");
                var pushStatus = "N";
                var resPush = SendNotificationHuawei(dataPush);
                if (await resPush)
                {
                    sendSuccess += 1;
                    pushStatus = "Y";
                }
                Console.WriteLine($"Notification sent");

                resp = string.Format(Resource.respsuccess, resMsg, "sent=" + sendSuccess, "", "", "", "").Replace("(", "{").Replace(")", "}");

            }
            catch (Exception ex)
            {
                resp = string.Format(Resource.resperror, "", "").Replace("(", "{").Replace(")", "}");
            }
            return resp;
        }


        public static async Task<bool> SendNotification(string jsonParam)
            {
                bool sent = false;

                var server_key = ("key=server_key").Trim();
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", server_key);
                    httpRequest.Content = new StringContent(jsonParam, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            // return true;
                            sent = true;
                        }
                        else
                        {
                            sent = false;
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            // _logger.LogError($"Error sending notification. Status Code: {result.StatusCode}");
                        }
                    }
                }

                return sent;
            }

            public static async Task<bool> SendNotificationHuawei(string jsonParam )
            {
                bool sent = false;
                var hToken = ("token").Trim();
                var server_key = ("server_key").Trim();
                var sender_id = ("sender_id").Trim();
            // Call to get key for send push //
            try
                {              
                    if (hToken != string.Empty)
                    {
                        var url = "https://push-api.cloud.huawei.com/v1/{0}/messages:send";
                        url = url.Replace("{0}", sender_id).Trim();
                        using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
                        {
                            httpRequest.Headers.TryAddWithoutValidation("Authorization", hToken);
                            httpRequest.Content = new StringContent(jsonParam, Encoding.UTF8, "application/json");

                            using (var httpClient = new HttpClient())
                            {
                                var result = await httpClient.SendAsync(httpRequest);

                                if (result.IsSuccessStatusCode)
                                {
                                    var data = await result.Content.ReadAsStringAsync();  // return true;
                                    HMSPushResponseClass resbody = JsonSerializer.Deserialize<HMSPushResponseClass>(data);
                                    if (resbody.code == "80000000" && resbody.msg == "Success")
                                    {
                                        sent = true;
                                    }
                                    else
                                    {
                                        sent = false;
                                    }
                                }
                                else
                                {
                                    sent = false;
                                }
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    sent = false;
                }


            return sent;
            }

    }

    public class HMSOAuthenClass
    {
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class HMSPushResponseClass
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string requestId { get; set; }
    }

}
