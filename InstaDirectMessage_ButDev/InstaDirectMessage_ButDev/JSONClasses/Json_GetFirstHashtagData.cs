using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.JSONClasses
{
    public class Json_GetFirstHashtagData
    {
        public class Candidate
        {
            public long width { get; set; }
            public long height { get; set; }
            public string url { get; set; }
        }

        public class Caption
        {
            public string pk { get; set; }
            public object user_id { get; set; }
            public string text { get; set; }
            public long type { get; set; }
            public long created_at { get; set; }
            public long created_at_utc { get; set; }
            public string content_type { get; set; }
            public string status { get; set; }
            public long bit_flags { get; set; }
            public bool did_report_as_spam { get; set; }
            public bool share_enabled { get; set; }
            public User user { get; set; }
            public bool is_covered { get; set; }
            public bool is_ranked_comment { get; set; }
            public string media_id { get; set; }
            public long private_reply_status { get; set; }
            public bool? has_translation { get; set; }
        }

        public class CarouselMedium
        {
            public string id { get; set; }
            public long media_type { get; set; }
            public ImageVersions2 image_versions2 { get; set; }
            public long original_width { get; set; }
            public long original_height { get; set; }
            public string accessibility_caption { get; set; }
            public string pk { get; set; }
            public string carousel_parent_id { get; set; }
            public Usertags usertags { get; set; }
            public string commerciality_status { get; set; }
            public List<VideoVersion> video_versions { get; set; }
            public double? video_duration { get; set; }
            public int? is_dash_eligible { get; set; }
            public string video_dash_manifest { get; set; }
            public string video_codec { get; set; }
            public int? number_of_qualities { get; set; }
        }

        public class CoauthorProducer
        {
            public long pk { get; set; }
            public string username { get; set; }
            public string full_name { get; set; }
            public bool is_private { get; set; }
            public string profile_pic_url { get; set; }
            public string profile_pic_id { get; set; }
            public bool is_verified { get; set; }
        }

        public class Comment
        {
            public string pk { get; set; }
            public object user_id { get; set; }
            public string text { get; set; }
            public long type { get; set; }
            public long created_at { get; set; }
            public long created_at_utc { get; set; }
            public string content_type { get; set; }
            public string status { get; set; }
            public long bit_flags { get; set; }
            public bool did_report_as_spam { get; set; }
            public bool share_enabled { get; set; }
            public User user { get; set; }
            public bool is_covered { get; set; }
            public bool is_ranked_comment { get; set; }
            public string media_id { get; set; }
            public long private_reply_status { get; set; }
            public string parent_comment_id { get; set; }
            public bool? has_translation { get; set; }
        }

        public class CommentInformTreatment
        {
            public bool should_have_inform_treatment { get; set; }
            public string text { get; set; }
            public object url { get; set; }
            public object action_type { get; set; }
        }

        public class Data
        {
            public string id { get; set; }
            public string name { get; set; }
            public long media_count { get; set; }
            public long follow_status { get; set; }
            public long following { get; set; }
            public long allow_following { get; set; }
            public bool allow_muting_story { get; set; }
            public string profile_pic_url { get; set; }
            public long non_violating { get; set; }
            public object related_tags { get; set; }
            public string subtitle { get; set; }
            public string social_context { get; set; }
            public List<object> social_context_profile_links { get; set; }
            public List<object> social_context_facepile_users { get; set; }
            public object follow_button_text { get; set; }
            public bool show_follow_drop_down { get; set; }
            public string formatted_media_count { get; set; }
            public object challenge_id { get; set; }
            public object destination_info { get; set; }
            public object description { get; set; }
            public object debug_info { get; set; }
            public object fresh_topic_metadata { get; set; }
            public object promo_banner { get; set; }
            public Top top { get; set; }
            public Recent recent { get; set; }
            public object content_advisory { get; set; }
            public object warning_message { get; set; }
        }

        public class ExploreItemInfo
        {
            public long num_columns { get; set; }
            public long total_num_columns { get; set; }
            public double aspect_ratio { get; set; }
            public bool autoplay { get; set; }
        }

        public class FanClubInfo
        {
            public object fan_club_id { get; set; }
            public object fan_club_name { get; set; }
            public object is_fan_club_referral_eligible { get; set; }
        }

        public class FriendshipStatus
        {
            public bool following { get; set; }
            public bool outgoing_request { get; set; }
            public bool is_bestie { get; set; }
            public bool is_restricted { get; set; }
            public bool is_feed_favorite { get; set; }
        }

        public class ImageVersions2
        {
            public List<Candidate> candidates { get; set; }
        }

        public class In
        {
            public User user { get; set; }
            public List<double> position { get; set; }
            public object start_time_in_video_in_sec { get; set; }
            public object duration_in_video_in_sec { get; set; }
        }

        public class LayoutContent
        {
            public List<Media> medias { get; set; }
        }

        public class Location
        {
            public long pk { get; set; }
            public string short_name { get; set; }
            public object facebook_places_id { get; set; }
            public string external_source { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public bool has_viewer_saved { get; set; }
            public double lng { get; set; }
            public double lat { get; set; }
            public bool is_eligible_for_guides { get; set; }
        }

        /*public class Media
        {
            public Media2 media { get; set; }
        }*/

        public class Media
        {
            public long taken_at { get; set; }
            public string pk { get; set; }
            public string id { get; set; }
            public object device_timestamp { get; set; }
            public long media_type { get; set; }
            public string code { get; set; }
            public string client_cache_key { get; set; }
            public long filter_type { get; set; }
            public string accessibility_caption { get; set; }
            public bool is_unified_video { get; set; }
            public bool should_request_ads { get; set; }
            public bool original_media_has_visual_reply_media { get; set; }
            public bool caption_is_edited { get; set; }
            public bool like_and_view_counts_disabled { get; set; }
            public string commerciality_status { get; set; }
            public bool is_paid_partnership { get; set; }
            public bool is_visual_reply_commenter_notice_enabled { get; set; }
            public List<long> timeline_pinned_user_ids { get; set; }
            public bool has_delayed_metadata { get; set; }
            public Location location { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public bool comment_likes_enabled { get; set; }
            public bool comment_threading_enabled { get; set; }
            public long max_num_visible_preview_comments { get; set; }
            public bool has_more_comments { get; set; }
            public string next_max_id { get; set; }
            public List<PreviewComment> preview_comments { get; set; }
            public List<Comment> comments { get; set; }
            public long comment_count { get; set; }
            public bool can_view_more_preview_comments { get; set; }
            public bool hide_view_all_comment_entrypoint { get; set; }
            public bool photo_of_you { get; set; }
            public bool is_organic_product_tagging_eligible { get; set; }
            public bool can_see_insights_as_brand { get; set; }
            public User user { get; set; }
            public bool can_viewer_reshare { get; set; }
            public long like_count { get; set; }
            public bool has_liked { get; set; }
            public List<string> top_likers { get; set; }
            public List<object> facepile_top_likers { get; set; }
            public ImageVersions2 image_versions2 { get; set; }
            public long original_width { get; set; }
            public long original_height { get; set; }
            public Caption caption { get; set; }
            public CommentInformTreatment comment_inform_treatment { get; set; }
            public SharingFrictionInfo sharing_friction_info { get; set; }
            public bool can_viewer_save { get; set; }
            public bool is_in_profile_grid { get; set; }
            public bool profile_grid_control_enabled { get; set; }
            public string organic_tracking_token { get; set; }
            public long has_shared_to_fb { get; set; }
            public string product_type { get; set; }
            public long deleted_reason { get; set; }
            public string integrity_review_decision { get; set; }
            public object commerce_integrity_review_decision { get; set; }
            public MusicMetadata music_metadata { get; set; }
            public bool is_artist_pick { get; set; }
            public int? carousel_media_count { get; set; }
            public List<CarouselMedium> carousel_media { get; set; }
            public Usertags usertags { get; set; }
            public List<CoauthorProducer> coauthor_producers { get; set; }
            public object coauthor_producer_can_see_organic_insights { get; set; }
        }

        public class MusicMetadata
        {
            public string music_canonical_id { get; set; }
            public object audio_type { get; set; }
            public object music_info { get; set; }
            public object original_sound_info { get; set; }
            public object pinned_media_ids { get; set; }
        }

        public class PreviewComment
        {
            public string pk { get; set; }
            public object user_id { get; set; }
            public string text { get; set; }
            public long type { get; set; }
            public long created_at { get; set; }
            public long created_at_utc { get; set; }
            public string content_type { get; set; }
            public string status { get; set; }
            public long bit_flags { get; set; }
            public bool did_report_as_spam { get; set; }
            public bool share_enabled { get; set; }
            public User user { get; set; }
            public bool is_covered { get; set; }
            public bool is_ranked_comment { get; set; }
            public string media_id { get; set; }
            public long private_reply_status { get; set; }
            public string parent_comment_id { get; set; }
            public bool? has_translation { get; set; }
        }

        public class Recent
        {
            public List<Section> sections { get; set; }
            public bool more_available { get; set; }
            public string next_max_id { get; set; }
            public long next_page { get; set; }
            public List<string> next_media_ids { get; set; }
        }

        public class RootObject
        {
            public long count { get; set; }
            public Data data { get; set; }
            public string status { get; set; }
        }

        public class Section
        {
            public string layout_type { get; set; }
            public LayoutContent layout_content { get; set; }
            public string feed_type { get; set; }
            public ExploreItemInfo explore_item_info { get; set; }
        }

        public class SharingFrictionInfo
        {
            public bool should_have_sharing_friction { get; set; }
            public object bloks_app_url { get; set; }
            public object sharing_friction_payload { get; set; }
        }

        public class Top
        {
            public List<Section> sections { get; set; }
            public bool more_available { get; set; }
            public string next_max_id { get; set; }
            public long next_page { get; set; }
            public List<string> next_media_ids { get; set; }
        }

        public class User
        {
            public long pk { get; set; }
            public string username { get; set; }
            public string full_name { get; set; }
            public bool is_private { get; set; }
            public string profile_pic_url { get; set; }
            public string profile_pic_id { get; set; }
            public bool is_verified { get; set; }
            public FriendshipStatus friendship_status { get; set; }
            public bool has_anonymous_profile_picture { get; set; }
            public bool is_unpublished { get; set; }
            public bool is_favorite { get; set; }
            public bool has_highlight_reels { get; set; }
            public bool transparency_product_enabled { get; set; }
            public List<object> account_badges { get; set; }
            public FanClubInfo fan_club_info { get; set; }
        }

        public class Usertags
        {
            public List<In> @in { get; set; }
        }

        public class VideoVersion
        {
            public long type { get; set; }
            public long width { get; set; }
            public long height { get; set; }
            public string url { get; set; }
            public string id { get; set; }
        }
    }
    public class Json_GetFirstHashtagData2
    {
        public class PageInfo
        {
            public bool has_next_page { get; set; }
            public string end_cursor { get; set; }
        }

        public class Node2
        {
            public string text { get; set; }
        }

        public class Edge2
        {
            public Node2 node { get; set; }
        }

        public class EdgeMediaToCaption
        {
            public List<Edge2> edges { get; set; }
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
            public string __typename { get; set; }
            public string id { get; set; }
            public EdgeMediaToCaption edge_media_to_caption { get; set; }
            public string shortcode { get; set; }
            public EdgeMediaToComment edge_media_to_comment { get; set; }
            public int taken_at_timestamp { get; set; }
            public Dimensions dimensions { get; set; }
            public string display_url { get; set; }
            public EdgeLikedBy edge_liked_by { get; set; }
            public EdgeMediaPreviewLike edge_media_preview_like { get; set; }
            public Owner owner { get; set; }
            public string thumbnail_src { get; set; }
            public List<ThumbnailResource> thumbnail_resources { get; set; }
            public bool is_video { get; set; }
            public string accessibility_caption { get; set; }
            public string product_type { get; set; }
            public int? video_view_count { get; set; }
        }

        public class Edge
        {
            public Node node { get; set; }
        }

        public class EdgeHashtagToMedia
        {
            public int count { get; set; }
            public PageInfo page_info { get; set; }
            public List<Edge> edges { get; set; }
        }

        public class Node4
        {
            public string text { get; set; }
        }

        public class Edge4
        {
            public Node4 node { get; set; }
        }

        public class EdgeMediaToCaption2
        {
            public List<Edge4> edges { get; set; }
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

        public class Node3
        {
            public string __typename { get; set; }
            public string id { get; set; }
            public EdgeMediaToCaption2 edge_media_to_caption { get; set; }
            public string shortcode { get; set; }
            public EdgeMediaToComment2 edge_media_to_comment { get; set; }
            public int taken_at_timestamp { get; set; }
            public Dimensions2 dimensions { get; set; }
            public string display_url { get; set; }
            public EdgeLikedBy2 edge_liked_by { get; set; }
            public EdgeMediaPreviewLike2 edge_media_preview_like { get; set; }
            public Owner2 owner { get; set; }
            public string thumbnail_src { get; set; }
            public List<ThumbnailResource2> thumbnail_resources { get; set; }
            public bool is_video { get; set; }
            public string accessibility_caption { get; set; }
        }

        public class Edge3
        {
            public Node3 node { get; set; }
        }

        public class EdgeHashtagToTopPosts
        {
            public List<Edge3> edges { get; set; }
        }

        public class EdgeHashtagToContentAdvisory
        {
            public int count { get; set; }
            public List<object> edges { get; set; }
        }

        public class Node5
        {
            public string name { get; set; }
        }

        public class Edge5
        {
            public Node5 node { get; set; }
        }

        public class EdgeHashtagToRelatedTags
        {
            public List<Edge5> edges { get; set; }
        }

        public class EdgeHashtagToNullState
        {
            public List<object> edges { get; set; }
        }

        public class Hashtag
        {
            public string id { get; set; }
            public string name { get; set; }
            public bool allow_following { get; set; }
            public string description { get; set; }
            public bool is_following { get; set; }
            public bool is_top_media_only { get; set; }
            public string profile_pic_url { get; set; }
            public EdgeHashtagToMedia edge_hashtag_to_media { get; set; }
            public EdgeHashtagToTopPosts edge_hashtag_to_top_posts { get; set; }
            public EdgeHashtagToContentAdvisory edge_hashtag_to_content_advisory { get; set; }
            public EdgeHashtagToRelatedTags edge_hashtag_to_related_tags { get; set; }
            public EdgeHashtagToNullState edge_hashtag_to_null_state { get; set; }
        }

        public class Graphql
        {
            public Hashtag hashtag { get; set; }
        }

        public class RootObject
        {
            public Graphql graphql { get; set; }
        }
    }
}
