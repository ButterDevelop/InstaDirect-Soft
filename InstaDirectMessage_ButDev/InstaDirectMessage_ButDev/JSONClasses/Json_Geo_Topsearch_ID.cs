using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.JSONClasses
{
    class Json_Geo_Topsearch_ID
    {
        public class User2
        {
            public string pk { get; set; }
            public string username { get; set; }
            public string full_name { get; set; }
            public bool is_private { get; set; }
            public string profile_pic_url { get; set; }
            public string profile_pic_id { get; set; }
            public bool is_verified { get; set; }
            public bool has_anonymous_profile_picture { get; set; }
            public int mutual_followers_count { get; set; }
            public int latest_reel_media { get; set; }
        }

        public class User
        {
            public int position { get; set; }
            public User2 user { get; set; }
        }

        public class Location
        {
            public string pk { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string short_name { get; set; }
            public double lng { get; set; }
            public double lat { get; set; }
            public string external_source { get; set; }
            public object facebook_places_id { get; set; }
        }

        public class Place2
        {
            public Location location { get; set; }
            public string title { get; set; }
            public string subtitle { get; set; }
            public List<object> media_bundles { get; set; }
            public string slug { get; set; }
        }

        public class Place
        {
            public Place2 place { get; set; }
            public int position { get; set; }
        }

        public class Hashtag2
        {
            public string name { get; set; }
            public object id { get; set; }
            public int media_count { get; set; }
            public bool use_default_avatar { get; set; }
            public string profile_pic_url { get; set; }
            public string search_result_subtitle { get; set; }
        }

        public class Hashtag
        {
            public int position { get; set; }
            public Hashtag2 hashtag { get; set; }
        }

        public class RootObject
        {
            public List<User> users { get; set; }
            public List<Place> places { get; set; }
            public List<Hashtag> hashtags { get; set; }
            public bool has_more { get; set; }
            public string rank_token { get; set; }
            public bool clear_client_cache { get; set; }
            public string status { get; set; }
        }
    }
}
