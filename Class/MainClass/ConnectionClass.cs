using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static iGoogleNetCAPI.Class.Entities.EntityClass;

namespace iGoogleNetCAPI.Class.MainClass
{
    public class ConnectionClass
    {
        MainClass mainClass = new MainClass();

        public string GetDirection(Direction jsonStr)
        {
            try
            {

                string Start_Lat = jsonStr.PLAT; // "13.7224192" '"13.7224192" ' "18.3170581"
                string Start_lng = jsonStr.PLONG; // "100.5405307" '"100.5405307" ' "99.3986862"
                string Dest_Lat = jsonStr.DEST_LAT; // "18.3170581"
                string Dest_lng = jsonStr.DEST_LNG; // "99.3986862"
                string Web_key = "KEYAPI";
                string param = "origin=" + Start_Lat + "," + Start_lng + "&destination=" + Dest_Lat + "," + Dest_lng;
                param += "&sensor=false&mode=driving&key=" + Web_key;
                string Url = "https://maps.googleapis.com/maps/api/directions/json?";
                string resp = mainClass.GetData(Url, param);
                return resp;
            }
            catch (Exception ex)
            {
                 string data = "error---" + ex.Message;
                 return data;
            }

        }

        public string GetDistanceMatrix(DataTable dt, string origins, string destinations)
        {
            try
            {
                string Web_key = "AIzaSyBVsHt_TENoEhrqwYwvrM_tn8izXq5GWdw";
                string param = "origins=" + origins + "&destinations=" + destinations;
                param += "&mode=driving&key=" + Web_key;
                string Url = "https://maps.googleapis.com/maps/api/distancematrix/json?";
                string jResult = mainClass.GetData(Url, param);
        
                return jResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
