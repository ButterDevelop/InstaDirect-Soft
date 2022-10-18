using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDirectMessage_ButDev.JSONClasses
{
    public class Json_GetFirstGeoData
    {
        public class AdditionalAudioInfo
        {
            public object additional_audio_username { get; set; }
            public AudioReattributionInfo audio_reattribution_info { get; set; }
        }

        public class AdditionalCandidates
        {
            public IgtvFirstFrame igtv_first_frame { get; set; }
            public FirstFrame first_frame { get; set; }
        }

        public class AnimatedThumbnailSpritesheetInfoCandidates
        {
            public Default @default { get; set; }
        }

        public class AudioRankingInfo
        {
            public string best_audio_cluster_id { get; set; }
        }

        public class AudioReattributionInfo
        {
            public bool should_allow_restore { get; set; }
        }

        public class BrandedContentTagInfo
        {
            public bool can_add_tag { get; set; }
        }

        public class Candidate
        {
            public long width { get; set; }
            public long height { get; set; }
            public string url { get; set; }
        }

        public class Caption
        {
            public object pk { get; set; }
            public long user_id { get; set; }
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
            public object media_id { get; set; }
            public bool has_translation { get; set; }
            public long private_reply_status { get; set; }
        }

        public class CarouselMedium
        {
            public string id { get; set; }
            public long media_type { get; set; }
            public ImageVersions2 image_versions2 { get; set; }
            public long original_width { get; set; }
            public long original_height { get; set; }
            public string accessibility_caption { get; set; }
            public object pk { get; set; }
            public string carousel_parent_id { get; set; }
            public string commerciality_status { get; set; }
            public Usertags usertags { get; set; }
            public List<VideoVersion> video_versions { get; set; }
            public double? video_duration { get; set; }
            public int? is_dash_eligible { get; set; }
            public string video_dash_manifest { get; set; }
            public string video_codec { get; set; }
            public int? number_of_qualities { get; set; }
        }

        public class Clips
        {
            public string id { get; set; }
            public List<Item> items { get; set; }
            public bool more_available { get; set; }
            public string design { get; set; }
            public string label { get; set; }
            public string max_id { get; set; }
        }

        public class ClipsMetadata
        {
            public object music_info { get; set; }
            public OriginalSoundInfo original_sound_info { get; set; }
            public string audio_type { get; set; }
            public string music_canonical_id { get; set; }
            public object featured_label { get; set; }
            public MashupInfo mashup_info { get; set; }
            public object nux_info { get; set; }
            public object viewer_interaction_settings { get; set; }
            public BrandedContentTagInfo branded_content_tag_info { get; set; }
            public object shopping_info { get; set; }
            public AdditionalAudioInfo additional_audio_info { get; set; }
            public bool is_shared_to_fb { get; set; }
            public object breaking_content_info { get; set; }
            public object challenge_info { get; set; }
            public object reels_on_the_rise_info { get; set; }
            public object breaking_creator_info { get; set; }
            public object asset_recommendation_info { get; set; }
            public object contextual_highlight_info { get; set; }
            public string clips_creation_entry_point { get; set; }
            public AudioRankingInfo audio_ranking_info { get; set; }
            public object template_info { get; set; }
            public bool is_fan_club_promo_video { get; set; }
            public bool disable_use_in_clips_client_cache { get; set; }
            public object content_appreciation_info { get; set; }
            public bool show_achievements { get; set; }
            public bool show_tips { get; set; }
            public MerchandisingPillInfo merchandising_pill_info { get; set; }
            public bool is_public_chat_welcome_video { get; set; }
        }

        public class Comment
        {
            public long pk { get; set; }
            public long user_id { get; set; }
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
            public long media_id { get; set; }
            public long private_reply_status { get; set; }
            public long? parent_comment_id { get; set; }
            public bool? has_translation { get; set; }
        }

        public class CommentInformTreatment
        {
            public bool should_have_inform_treatment { get; set; }
            public string text { get; set; }
            public object url { get; set; }
            public object action_type { get; set; }
        }

        public class ConsumptionInfo
        {
            public bool is_bookmarked { get; set; }
            public string should_mute_audio_reason { get; set; }
            public bool is_trending_in_clips { get; set; }
            public object should_mute_audio_reason_type { get; set; }
            public object display_media_id { get; set; }
        }

        public class Default
        {
            public double video_length { get; set; }
            public long thumbnail_width { get; set; }
            public long thumbnail_height { get; set; }
            public double thumbnail_duration { get; set; }
            public List<string> sprite_urls { get; set; }
            public long thumbnails_per_row { get; set; }
            public long total_thumbnail_num_per_sprite { get; set; }
            public long max_thumbnails_per_sprite { get; set; }
            public long sprite_width { get; set; }
            public long sprite_height { get; set; }
            public long rendered_width { get; set; }
            public long file_size_kb { get; set; }
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

        public class FillItem
        {
            public Media media { get; set; }
        }

        public class FirstFrame
        {
            public long width { get; set; }
            public long height { get; set; }
            public string url { get; set; }
        }

        public class FriendshipStatus
        {
            public bool following { get; set; }
            public bool outgoing_request { get; set; }
            public bool is_bestie { get; set; }
            public bool is_restricted { get; set; }
            public bool is_feed_favorite { get; set; }
            public bool followed_by { get; set; }
            public bool blocking { get; set; }
            public bool muting { get; set; }
            public bool is_private { get; set; }
            public bool incoming_request { get; set; }
        }

        public class Hours
        {
            public string status { get; set; }
            public string current_status { get; set; }
            public string hours_today { get; set; }
            public List<object> schedule { get; set; }
            public bool is_open { get; set; }
        }

        public class IgArtist
        {
            public long pk { get; set; }
            public string username { get; set; }
            public string full_name { get; set; }
            public bool is_private { get; set; }
            public string profile_pic_url { get; set; }
            public string profile_pic_id { get; set; }
            public bool is_verified { get; set; }
        }

        public class IgBusiness
        {
        }

        public class IgtvFirstFrame
        {
            public long width { get; set; }
            public long height { get; set; }
            public string url { get; set; }
        }

        public class ImageVersions2
        {
            public List<Candidate> candidates { get; set; }
            public AdditionalCandidates additional_candidates { get; set; }
            public AnimatedThumbnailSpritesheetInfoCandidates animated_thumbnail_spritesheet_info_candidates { get; set; }
        }

        public class In
        {
            public User user { get; set; }
            public List<double> position { get; set; }
            public object start_time_in_video_in_sec { get; set; }
            public object duration_in_video_in_sec { get; set; }
        }

        public class Item
        {
            public Media media { get; set; }
        }

        public class LayoutContent
        {
            public OneByTwoItem one_by_two_item { get; set; }
            public List<FillItem> fill_items { get; set; }
            public List<Media> medias { get; set; }
        }

        public class Location
        {
            public long pk { get; set; }
            public string short_name { get; set; }
            public long facebook_places_id { get; set; }
            public string external_source { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public bool has_viewer_saved { get; set; }
            public double lng { get; set; }
            public double lat { get; set; }
            public bool is_eligible_for_guides { get; set; }
        }

        public class LocationInfo
        {
            public string location_id { get; set; }
            public string facebook_places_id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public string website { get; set; }
            public string category { get; set; }
            public long price_range { get; set; }
            public Hours hours { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public string location_address { get; set; }
            public string location_city { get; set; }
            public long location_region { get; set; }
            public string location_zip { get; set; }
            public IgBusiness ig_business { get; set; }
            public bool show_location_page_survey { get; set; }
            public object num_guides { get; set; }
            public bool has_menu { get; set; }
            public PageEffectInfo page_effect_info { get; set; }
        }

        public class MashupInfo
        {
            public bool mashups_allowed { get; set; }
            public bool can_toggle_mashups_allowed { get; set; }
            public bool has_been_mashed_up { get; set; }
            public object formatted_mashups_count { get; set; }
            public object original_media { get; set; }
            public object non_privacy_filtered_mashups_media_count { get; set; }
            public object mashup_type { get; set; }
            public bool is_creator_requesting_mashup { get; set; }
            public bool has_nonmimicable_additional_audio { get; set; }
        }

        public class Media
        {
            public long taken_at { get; set; }
            public long pk { get; set; }
            public string id { get; set; }
            public long device_timestamp { get; set; }
            public long media_type { get; set; }
            public string code { get; set; }
            public string client_cache_key { get; set; }
            public long filter_type { get; set; }
            public bool is_unified_video { get; set; }
            public bool should_request_ads { get; set; }
            public bool original_media_has_visual_reply_media { get; set; }
            public bool caption_is_edited { get; set; }
            public bool like_and_view_counts_disabled { get; set; }
            public string commerciality_status { get; set; }
            public bool is_paid_partnership { get; set; }
            public bool is_visual_reply_commenter_notice_enabled { get; set; }
            public bool has_delayed_metadata { get; set; }
            public Location location { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public bool comment_likes_enabled { get; set; }
            public bool comment_threading_enabled { get; set; }
            public long max_num_visible_preview_comments { get; set; }
            public bool has_more_comments { get; set; }
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
            public ImageVersions2 image_versions2 { get; set; }
            public long original_width { get; set; }
            public long original_height { get; set; }
            public object caption { get; set; }
            public CommentInformTreatment comment_inform_treatment { get; set; }
            public SharingFrictionInfo sharing_friction_info { get; set; }
            public long is_dash_eligible { get; set; }
            public string video_dash_manifest { get; set; }
            public string video_codec { get; set; }
            public long number_of_qualities { get; set; }
            public List<VideoVersion> video_versions { get; set; }
            public bool has_audio { get; set; }
            public double video_duration { get; set; }
            public long view_count { get; set; }
            public long play_count { get; set; }
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
            public ClipsMetadata clips_metadata { get; set; }
            public MediaCroppingInfo media_cropping_info { get; set; }
            public string logging_info_token { get; set; }
            public long carousel_media_count { get; set; }
            public List<CarouselMedium> carousel_media { get; set; }
            public List<object> top_likers { get; set; }
            public List<object> facepile_top_likers { get; set; }
            public string accessibility_caption { get; set; }
            public Usertags usertags { get; set; }
            public bool? commenting_disabled_for_viewer { get; set; }
            public long? next_max_id { get; set; }
            public int? fb_like_count { get; set; }
            public int? fb_play_count { get; set; }
            public double? video_subtitles_confidence { get; set; }
            public bool? nearly_complete_copyright_match { get; set; }
            public Thumbnails thumbnails { get; set; }
            public bool? igtv_exists_in_viewer_series { get; set; }
            public bool? is_post_live { get; set; }
            public string title { get; set; }
            public MashupInfo mashup_info { get; set; }
        }

        public class Media3
        {
            public Media media { get; set; }
        }

        public class MediaCroppingInfo
        {
            public object feed_preview_crop { get; set; }
            public SquareCrop square_crop { get; set; }
            public object three_by_four_preview_crop { get; set; }
        }

        public class MerchandisingPillInfo
        {
            public object merchandising_pill_types { get; set; }
            public long loop_time { get; set; }
        }

        public class MusicMetadata
        {
            public string music_canonical_id { get; set; }
            public string audio_type { get; set; }
            public object music_info { get; set; }
            public OriginalSoundInfo original_sound_info { get; set; }
            public List<object> pinned_media_ids { get; set; }
        }

        public class NativeLocationData
        {
            public LocationInfo location_info { get; set; }
            public Ranked ranked { get; set; }
            public Recent recent { get; set; }
        }

        public class OneByTwoItem
        {
            public Clips clips { get; set; }
        }

        public class OriginalSoundInfo
        {
            public long audio_asset_id { get; set; }
            public object music_canonical_id { get; set; }
            public string progressive_download_url { get; set; }
            public string dash_manifest { get; set; }
            public IgArtist ig_artist { get; set; }
            public bool should_mute_audio { get; set; }
            public long original_media_id { get; set; }
            public bool hide_remixing { get; set; }
            public long duration_in_ms { get; set; }
            public long time_created { get; set; }
            public string original_audio_title { get; set; }
            public ConsumptionInfo consumption_info { get; set; }
            public bool allow_creator_to_rename { get; set; }
            public bool can_remix_be_shared_to_fb { get; set; }
            public object formatted_clips_media_count { get; set; }
            public List<object> audio_parts { get; set; }
            public bool is_explicit { get; set; }
            public string original_audio_subtype { get; set; }
            public bool is_audio_automatically_attributed { get; set; }
            public bool is_reuse_disabled { get; set; }
            public bool is_xpost_from_fb { get; set; }
            public object xpost_fb_creator_info { get; set; }
        }

        public class PageEffectInfo
        {
            public long num_effects { get; set; }
            public object thumbnail_url { get; set; }
            public object effect { get; set; }
        }

        public class PreviewComment
        {
            public long pk { get; set; }
            public long user_id { get; set; }
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
            public long media_id { get; set; }
            public long private_reply_status { get; set; }
            public long? parent_comment_id { get; set; }
            public bool? has_translation { get; set; }
        }

        public class Ranked
        {
            public List<Section> sections { get; set; }
            public bool more_available { get; set; }
            public long next_page { get; set; }
            public List<object> next_media_ids { get; set; }
            public string next_max_id { get; set; }
        }

        public class Recent
        {
            public List<Section> sections { get; set; }
            public bool more_available { get; set; }
            public long next_page { get; set; }
            public List<object> next_media_ids { get; set; }
            public string next_max_id { get; set; }
        }

        public class Root
        {
            public NativeLocationData native_location_data { get; set; }
            public string logging_page_id { get; set; }
            public object photos_and_videos_header { get; set; }
            public object recent_pictures_and_videos_subheader { get; set; }
            public bool show_qr_modal { get; set; }
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

        public class SquareCrop
        {
            public double crop_bottom { get; set; }
            public double crop_left { get; set; }
            public double crop_right { get; set; }
            public double crop_top { get; set; }
        }

        public class Thumbnails
        {
            public double video_length { get; set; }
            public long thumbnail_width { get; set; }
            public long thumbnail_height { get; set; }
            public double thumbnail_duration { get; set; }
            public List<string> sprite_urls { get; set; }
            public long thumbnails_per_row { get; set; }
            public long total_thumbnail_num_per_sprite { get; set; }
            public long max_thumbnails_per_sprite { get; set; }
            public long sprite_width { get; set; }
            public long sprite_height { get; set; }
            public long rendered_width { get; set; }
            public long file_size_kb { get; set; }
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
    public class Json_GetFirstGeoData2
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
