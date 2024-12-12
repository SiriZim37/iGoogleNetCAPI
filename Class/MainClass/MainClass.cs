using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static iGoogleNetCAPI.Class.Entities.EntityClass;

namespace iGoogleNetCAPI.Class.MainClass
{
    public class MainClass
    {
        private static CultureInfo cultures = new CultureInfo("en-US");
        private DateTimeFormatInfo dateFormat = cultures.DateTimeFormat;
        public string secretkey_forgen = "secretkey_forgen";


        public string GetData(string url, string parameter)
        {

            string Data = "";
            try
            {
                Uri uri = new Uri(url + parameter);
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myWebRequest.ContentType = "application/json";
                myWebRequest.Method = WebRequestMethods.Http.Get;
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                System.IO.Stream dataStream = myWebRequest.GetRequestStream();
                dataStream.Close();
                WebResponse myWebResponse = myWebRequest.GetResponse();
                dataStream = myWebResponse.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Data = responseFromServer;
                reader.Close();
                dataStream.Close();
                myWebResponse.Close();
            }
            catch (Exception ex)
            {
                Data = "error---" + ex.Message;
            }
            return Data;
        }


        public string postData(string data, string url, string header)
        {
            string Data = "";
            try
            {
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myWebRequest.ContentType = "application/json";  
                myWebRequest.ContentLength = byteArray.Length;
                myWebRequest.Headers.Add("Authorization", header);
                myWebRequest.Method = WebRequestMethods.Http.Post;
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                System.IO.Stream dataStream = myWebRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse myWebResponse = myWebRequest.GetResponse();
                dataStream = myWebResponse.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Data = responseFromServer;
                reader.Close();
                dataStream.Close();
                myWebResponse.Close();
            }
            catch (Exception ex)
            {
                Data = "error---" + ex.Message;
            }
            return Data;
        }


        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public DataSet ExecuteDataset(string strConn, string str_store, SqlParameter[] para)
        {
            var cnn = new SqlConnection(strConn);
            var cmd = new SqlCommand(str_store, cnn);
            var ds = new DataSet();
            var da = new SqlDataAdapter();
            try
            {
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = str_store;
                int i = 0;
                while (i <= para.Length - 1)
                {
                    cmd.Parameters.Add(para[i]);
                    i = i + 1;
                }

                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
            }
        }



        public string GenerateToken(string unique_id)
        {

            try
            {
                DateTime now = DateTime.Now;
                //var timeExp = 24;
                //var newTime = System.DateTime.Now.AddHours(timeExp);
                var newTime = System.DateTime.Now.AddMinutes(5);
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey_forgen + unique id"));
                var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
                var header = new JwtHeader(signingCredentials);
                var apiKeyData = generateJWT_API("KEYFORPREF");
                var payload = new JwtPayload { { "HEADER", apiKeyData }, { "PAYLOAD", unique_id }, { "FOOT", newTime } };
                var secToken = new JwtSecurityToken(header, payload);
                var handler = new JwtSecurityTokenHandler();
                var tokenString = handler.WriteToken(secToken);
                string[] parst = tokenString.Split(".");
                string encodeHeader = HeaderEncode(parst[0]);
                string encodePayload = PayloadEncode(parst[1]);
                return encodeHeader + "." + encodePayload + "." + parst[2];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DecryptionToken(string token)
        {
            try
            {
                string pre_add = "";
                string post_add = "";
                string key_gen = "";
                 string key = "secretkey_forgen" + "testA";

                var parts = token.Split('.');
                string header = parts[0];
                string payload = parts[1].Replace(pre_add, "");
                payload = payload.Replace(post_add, "");
                string foot = parts[2];

                // ************* CHECK VALIDATE ******** ' 

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters(key);
                var vertoken = header + "." + payload + "." + foot;
                SecurityToken validatedToken;
                var handler = tokenHandler.ValidateToken(vertoken, validationParameters, out validatedToken);
                bool isAuthen = handler.Identity.IsAuthenticated;

                if (isAuthen)
                {
                    var crypto = Base64UrlDecode(parts[2]);
                    string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
                    var headerData = JObject.Parse(headerJson);
                    string payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
                    var payloadData = JObject.Parse(payloadJson);
                    string k_ey = payloadData.GetValue("HEADER").ToString();
                    string cus_id_wcf = payloadData.GetValue("PAYLOAD").ToString();
                    DateTime timeEXP = Convert.ToDateTime(payloadData.GetValue("FOOT"));
                    string date_time = timeEXP.AddMinutes(-30).ToString("MM/dd/yyyy", dateFormat); // --- time for decrypt  // Check Validate P_KEY
                    var currTime = DateTime.Now; // Current time 
                    if (currTime >= timeEXP)
                        return "invalid";
                    else
                        return "invalid";
                }
                else
                {
                    return "invalid";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static TokenValidationParameters GetValidationParameters(string key)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "",
                ValidAudience = "",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        }

       

        public static byte[] Base64UrlDecode(string input)
        {
            string output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');
            switch (output.Length % 4)
            {
                case 0:  {      break; }
                case 1:  {      output += "==="; break; }
                case 2:  {      output += "==";  break;  }
                case 3:  {      output += "="; break; }
                default: {      throw new Exception ( "Illegal base64url string!" ) ;  break; }
            }

            var converted = Convert.FromBase64String(output);
            return converted;
        }


        public string generateJWT_API(string key_gen)
        {
            string eData = "";
            try
            {
                eData = Encrypt_keyAPI(key_gen, GenKey_JWT_API(Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"))));
                return eData;
            }
            catch (Exception ex)
            {
                return "invalid";
            }
        }

        public string GenKey_JWT_API(DateTime oldDate)
        {
            try
            {
                DateTime Current_Date = Convert.ToDateTime(oldDate.AddHours(7).ToString("MM/dd/yyyy", dateFormat));
                int Find_Date = Convert.ToInt32(Current_Date.Day);
                int Find_Month = Convert.ToInt32(Current_Date.Month);
                int Find_Year = Convert.ToInt32(Current_Date.Year);
                string GetKey = "";
                string TempKey = "";
                double TempDate = 0;
                double TempMonth = 0;
                double TempYear = 0;
                if (Find_Date % 2 == 0)
                {
                    TempKey = DateAndTime.DateAdd(DateInterval.Day, -25, Current_Date).ToString("dd/MM/yyyy");
                    if (Find_Month % 2 == 0)
                    {
                        TempDate = Convert.ToDouble(TempKey.Substring(0, 2));
                        TempMonth = Convert.ToDouble(TempKey.Substring(3, 2));
                        TempYear = Convert.ToDouble(TempKey.Substring(6, 4));
                        TempKey = ((TempDate + TempMonth + TempYear) / 2.22).ToString().Replace(".", "");
                    }
                    else
                    {
                        TempDate = Convert.ToDouble(TempKey.Substring(0, 2));
                        TempMonth = Convert.ToDouble(TempKey.Substring(3, 2));
                        TempYear = Convert.ToDouble(TempKey.Substring(6, 4));
                        TempKey = ((TempDate + TempMonth + TempYear) / 3.33).ToString().Replace(".", "");
                    }
                }
                else
                {
                    TempKey = DateAndTime.DateAdd(DateInterval.Day, -15, Current_Date).ToString("dd/MM/yyyy");
                    if (Find_Month % 2 == 0)
                    {
                        TempDate = Convert.ToDouble(TempKey.Substring(0, 2));
                        TempMonth = Convert.ToDouble(TempKey.Substring(3, 2));
                        TempYear = Convert.ToDouble(TempKey.Substring(6, 4));
                        TempKey = ((TempDate + TempMonth + TempYear) / 2.22).ToString().Replace(".", "");
                    }
                    else
                    {
                        TempDate = Convert.ToDouble(TempKey.Substring(0, 2));
                        TempMonth = Convert.ToDouble(TempKey.Substring(3, 2));
                        TempYear = Convert.ToDouble(TempKey.Substring(6, 4));
                        TempKey = ((TempDate + TempMonth + TempYear) / 3.33).ToString().Replace(".", "");
                    }
                }
                GetKey = TempKey.Substring(0, 4) + TempKey.Substring(4);
                return GetKey;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Encrypt_keyAPI(string text, string key)
        {
            try
            {
                var crp = new TripleDESCryptoServiceProvider();
                var uEncode = new UnicodeEncoding();
                var bytPlainText = uEncode.GetBytes(text);
                var stmCipherText = new MemoryStream();
                var slt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                var pdb = new Rfc2898DeriveBytes(key, slt);
                var bytDerivedKey = pdb.GetBytes(24);

                crp.Key = bytDerivedKey;
                crp.IV = pdb.GetBytes(8);

                var csEncrypted = new CryptoStream(stmCipherText, crp.CreateEncryptor(), CryptoStreamMode.Write);

                csEncrypted.Write(bytPlainText, 0, bytPlainText.Length);
                csEncrypted.FlushFinalBlock();
                return Convert.ToBase64String(stmCipherText.ToArray());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int RandomNumber()
        {
            Random random = new Random();
            return random.Next(0, 9);
        }

        private string HeaderEncode(string _Payload)
        {
            try
            {
                string titlejwt = "max";
                string numbertype = RandomNumber().ToString();
                string newpayload = "";

                switch (numbertype)
                {
                    case "0":
                        _Payload = _Payload.Replace(numbertype, numbertype + "hEd");
                        break;
                    case "1":
                        _Payload = _Payload.Replace(numbertype, numbertype + "sQa");
                        break;
                    case "2":
                        _Payload = _Payload.Replace(numbertype, numbertype + "POd");
                        break;
                    case "3":
                        _Payload = _Payload.Replace(numbertype, numbertype + "BiS");
                        break;
                    case "4":
                        _Payload = _Payload.Replace(numbertype, numbertype + "Eex");
                        break;
                    case "5":
                        _Payload = _Payload.Replace(numbertype, numbertype + "OoK");
                        break;
                    case "6":
                        _Payload = _Payload.Replace(numbertype, numbertype + "Qer");
                        break;
                    case "7":
                        _Payload = _Payload.Replace(numbertype, numbertype + "Sok");
                        break;
                    case "8":
                        _Payload = _Payload.Replace(numbertype, numbertype + "kAe");
                        break;
                    default:
                        throw new Exception("invalid");
                }

                newpayload = titlejwt + numbertype + _Payload.Remove(0, 3) + numbertype;
                return newpayload;
            }
            catch (Exception)
            {
                return "invalid";
            }
        }

        private string PayloadEncode(string _Payload)
        {
            try
            {
                string titlejwt = "tlc";
                string numbertype = RandomNumber().ToString();
                string newpayload = "";

                switch (numbertype)
                {
                    case "0":
                        _Payload = _Payload.Replace(numbertype, numbertype + "eCq");
                        break;
                    case "1":
                        _Payload = _Payload.Replace(numbertype, numbertype + "HbD");
                        break;
                    case "2":
                        _Payload = _Payload.Replace(numbertype, numbertype + "lOv");
                        break;
                    case "3":
                        _Payload = _Payload.Replace(numbertype, numbertype + "SeC");
                        break;
                    case "4":
                        _Payload = _Payload.Replace(numbertype, numbertype + "mGr");
                        break;
                    case "5":
                        _Payload = _Payload.Replace(numbertype, numbertype + "YsR");
                        break;
                    case "6":
                        _Payload = _Payload.Replace(numbertype, numbertype + "niD");
                        break;
                    case "7":
                        _Payload = _Payload.Replace(numbertype, numbertype + "ApO");
                        break;
                    case "8":
                        _Payload = _Payload.Replace(numbertype, numbertype + "cKa");
                        break;
                    default:
                        throw new Exception("invalid");
                }

                newpayload = titlejwt + numbertype + _Payload.Remove(0, 3) + numbertype;
                return newpayload;
            }
            catch (Exception)
            {
                return "invalid";
            }
        }


    }

}
