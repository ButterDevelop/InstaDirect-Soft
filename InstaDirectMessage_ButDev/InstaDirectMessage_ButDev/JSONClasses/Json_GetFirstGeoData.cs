using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.JSONClasses
{
    class Json_GetFirstGeoData
    {
        public class PageInfo
        {
            public bool has_next_page { get; set; }
            public string end_cursor { get; set; }
        }

        public class EdgeMediaToCaption
        {
            public List<object> edges { get; set; }
        }

        public class EdgeMediaToComment
        {
            public int count { get; set; }
        }

        public class Dimensions
        {
            public int height { get; set; }
            public int width { get; set; }
        }

        public class EdgeLikedBy
        {
            public int count { get; set; }
        }

        public class EdgeMediaPreviewLike
        {
            public int count { get; set; }
        }

        public class Owner
        {
            public string id { get; set; }
        }

        public class ThumbnailResource
        {
            public string src { get; set; }
            public int config_width { get; set; }
            public int config_height { get; set; }
        }

        public class Node
        {
            public bool comments_disabled { get; set; }
            public string id { get; set; }
            public EdgeMediaToCaption edge_media_to_caption { get; set; }
            public string shortcode { get; set; }
            public EdgeMediaToComment edge_media_to_comment { get; set; }
            public long taken_at_timestamp { get; set; }
            public Dimensions dimensions { get; set; }
            public string display_url { get; set; }
            public EdgeLikedBy edge_liked_by { get; set; }
            public EdgeMediaPreviewLike edge_media_preview_like { get; set; }
            public Owner owner { get; set; }
            public string thumbnail_src { get; set; }
            public List<ThumbnailResource> thumbnail_resources { get; set; }
            public bool is_video { get; set; }
            public object accessibility_caption { get; set; }
            public long? video_view_count { get; set; }
        }

        public class Edge
        {
            public Node node { get; set; }
        }

        public class EdgeLocationToMedia
        {
            public int count { get; set; }
            public PageInfo page_info { get; set; }
            public List<Edge> edges { get; set; }
        }

        public class PageInfo2
        {
            public bool has_next_page { get; set; }
            public object end_cursor { get; set; }
        }

        public class EdgeMediaToCaption2
        {
            public List<object> edges { get; set; }
        }

        public class EdgeMediaToComment2
        {
            public int count { get; set; }
        }

        public class Dimensions2
        {
            public int height { get; set; }
            public int width { get; set; }
        }

        public class EdgeLikedBy2
        {
            public int count { get; set; }
        }

        public class EdgeMediaPreviewLike2
        {
            public int count { get; set; }
        }

        public class Owner2
        {
            public string id { get; set; }
        }

        public class ThumbnailResource2
        {
            public string src { get; set; }
            public int config_width { get; set; }
            public int config_height { get; set; }
        }

        public class Node2
        {
            public string id { get; set; }
            public EdgeMediaToCaption2 edge_media_to_caption { get; set; }
            public string shortcode { get; set; }
            public EdgeMediaToComment2 edge_media_to_comment { get; set; }
            public long taken_at_timestamp { get; set; }
            public Dimensions2 dimensions { get; set; }
            public string display_url { get; set; }
            public EdgeLikedBy2 edge_liked_by { get; set; }
            public EdgeMediaPreviewLike2 edge_media_preview_like { get; set; }
            public Owner2 owner { get; set; }
            public string thumbnail_src { get; set; }
            public List<ThumbnailResource2> thumbnail_resources { get; set; }
            public bool is_video { get; set; }
            public object accessibility_caption { get; set; }
            public long? video_view_count { get; set; }
        }

        public class Edge2
        {
            public Node2 node { get; set; }
        }

        public class EdgeLocationToTopPosts
        {
            public int count { get; set; }
            public PageInfo2 page_info { get; set; }
            public List<Edge2> edges { get; set; }
        }

        public class Country
        {
            public string id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class City
        {
            public string id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class Directory
        {
            public Country country { get; set; }
            public City city { get; set; }
        }

        public class Location
        {
            public string id { get; set; }
            public string name { get; set; }
            public bool has_public_page { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public string slug { get; set; }
            public string blurb { get; set; }
            public string website { get; set; }
            public string phone { get; set; }
            public string primary_alias_on_fb { get; set; }
            public string address_json { get; set; }
            public string profile_pic_url { get; set; }
            public EdgeLocationToMedia edge_location_to_media { get; set; }
            public EdgeLocationToTopPosts edge_location_to_top_posts { get; set; }
            public Directory directory { get; set; }
        }

        public class Graphql
        {
            public Location location { get; set; }
        }

        public class RootObject
        {
            public Graphql graphql { get; set; }
            public string logging_page_id { get; set; }
            public bool photos_and_videos_header { get; set; }
            public bool recent_pictures_and_videos_subheader { get; set; }
            public bool top_images_and_videos_subheader { get; set; }
        }
    }
}
