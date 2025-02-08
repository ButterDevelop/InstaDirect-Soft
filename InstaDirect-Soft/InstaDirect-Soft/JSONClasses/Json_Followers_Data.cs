using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.JSONClasses
{
    class Json_Followers_Data
    {
        public class PageInfo
        {
            public bool has_next_page { get; set; }
            public string end_cursor { get; set; }
        }

        public class Node
        {
            public string id { get; set; }
            public string username { get; set; }
            public string full_name { get; set; }
            public string profile_pic_url { get; set; }
            public bool is_private { get; set; }
            public bool is_verified { get; set; }
            public bool followed_by_viewer { get; set; }
            public bool requested_by_viewer { get; set; }
        }

        public class Edge
        {
            public Node node { get; set; }
        }

        public class EdgeFollowedBy
        {
            public int count { get; set; }
            public PageInfo page_info { get; set; }
            public List<Edge> edges { get; set; }
        }

        public class EdgeMutualFollowedBy
        {
            public int count { get; set; }
            public List<object> edges { get; set; }
        }

        public class User
        {
            public EdgeFollowedBy edge_followed_by { get; set; }
            public EdgeMutualFollowedBy edge_mutual_followed_by { get; set; }
        }

        public class Data
        {
            public User user { get; set; }
        }

        public class RootObject
        {
            public Data data { get; set; }
            public string status { get; set; }
        }
    }
}
