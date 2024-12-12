using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iGoogleNetCAPI.Class.Entities
{
    public class EntityClass
    {
        public class NotifyModel
        {
            public string FCM { get; set; }
        }

        public class AuthorizationModel
        {
            public string UNIQUE_ID { get; set; }
        }

        public class ExcuteModel
        {
            public string PARAM1 { get; set; }
        }

        public class Direction
        {
            public string DEST_LAT { get; set; }
            public string DEST_LNG { get; set; }
            public string PLAT { get; set; }
            public string PLONG { get; set; }
        }



    public class DistanceMatrix
        {
            public string[] destination_addresses { get; set; }
            public string[] origin_addresses { get; set; }
            public Row[] rows { get; set; }
            public string status { get; set; }
        }

        public class Row
        {
            public Element[] elements { get; set; }
        }

        public class Element
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public string status { get; set; }
        }


        public class Distance
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Duration
        {
            public string text { get; set; }
            public int value { get; set; }
        }
    }

    public class MailModel
    {
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string MSubject { get; set; }
        public string MBody { get; set; }

        public List<Attachment> Attachments { get; set; }

        public class Attachment
        {
            public string filename { get; set; }
            public string filetype { get; set; }
            public string filedata { get; set; }
        }
    }

    public class MailConfigModel
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }


}
