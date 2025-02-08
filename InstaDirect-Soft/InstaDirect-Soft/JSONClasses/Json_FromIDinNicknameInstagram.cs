using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.JSONClasses
{
    class Json_FromIDinNicknameInstagram
    {
        public class User
        {
            public string username { get; set; }
            public long pk { get; set; }
            public string profile_pic_url { get; set; }
        }

        public class RootObject
        {
            public User user { get; set; }
            public string status { get; set; }
        }
    }
}
