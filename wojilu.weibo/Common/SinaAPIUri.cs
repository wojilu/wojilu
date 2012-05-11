namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Defines the SINA micro-blog API's uris (in xml format if applicable).
    /// </summary>
    public static class SinaAPIUri
    {
        /// <remarks />
        public const string WeiboUriPrefix = "https://api.weibo.com/2/";

        #region OAuth(Login)

        /// <remarks />
        public static readonly string Authorize = string.Format("{0}oauth2/authorize", "https://api.weibo.com/");

        /// <remarks />
        public static readonly string AccessToken = string.Format("{0}oauth2/access_token", "https://api.weibo.com/");

        #endregion

        #region Account

        /// <remarks />
        public static readonly string GetUID = string.Format("{0}account/get_uid.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string EndSession = string.Format("{0}account/end_session.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetRateLimitStatus = string.Format("{0}account/rate_limit_status.json",
                                                                         WeiboUriPrefix);

        /// <remarks />
        public static readonly string UpdateProfileImage = string.Format("{0}account/update_profile_image.json",
                                                                         WeiboUriPrefix);

        /// <remarks />
        public static readonly string UpdateProfile = string.Format("{0}account/update_profile.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string UpdatePrivacy = string.Format("{0}account/update_privacy.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetPrivacy = string.Format("{0}account/get_privacy.json", WeiboUriPrefix);

        #endregion

        #region Status

        /// <remarks />
        public static readonly string PublicTimeline = string.Format("{0}statuses/public_timeline.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string FriendsTimeline = string.Format("{0}statuses/friends_timeline.json",
                                                                      WeiboUriPrefix);

        /// <remarks />
        public static readonly string HomeTimeline = string.Format("{0}statuses/home_timeline.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string UserTimeline = string.Format("{0}statuses/user_timeline.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetMentions = string.Format("{0}statuses/mentions.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetCountersOfCommentNForward = string.Format("{0}statuses/counts.json",
                                                                                   WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetUnreadInfo = "http://rm.api.weibo.com/2/remind/unread_count.json";

        /// <remarks />
        public static readonly string ResetCounter = string.Format("{0}statuses/reset_count.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetEmotions = string.Format("{0}emotions.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string ShowStatus = string.Format("{0}statuses/show.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetStatusUrl = WeiboUriPrefix + "{0}/statuses/{1}.json";

        /// <remarks />
        public static readonly string UpdateStatus = string.Format("{0}statuses/update.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string UpdateStatusWithPic = string.Format("{0}statuses/upload.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteStatus = string.Format("{0}statuses/destroy.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string RepostStatus = string.Format("{0}statuses/repost.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string SearchStatuses = string.Format("{0}statuses/search.json", WeiboUriPrefix);

        #endregion

        #region Comments

        /// <remarks />
        public static readonly string GetComments = string.Format("{0}comments/show.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string CommentsTimeline = string.Format("{0}comments/timeline.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string CommentsByMe = string.Format("{0}comments/by_me.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string CommentsToMe = string.Format("{0}comments/to_me.json", WeiboUriPrefix);


        /// <remarks />
        public static readonly string ReplyComment = string.Format("{0}comments/reply.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteComment = string.Format("{0}comments/destroy.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteComments = string.Format("{0}comments/destroy_batch.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string Comment = string.Format("{0}comments/create.json", WeiboUriPrefix);

        #endregion

        #region User

        /// <remarks />
        public static readonly string GetUserInfo = string.Format("{0}users/show", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetHotUsers = string.Format("{0}suggestions/users/hot.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetSuggestedUsers = string.Format("{0}users/suggestions.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string SearchUsers = string.Format("{0}users/search.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetUserSearchSuggestions = string.Format("{0}search/suggestions/at_users.json",
                                                                               WeiboUriPrefix);

        #endregion

        #region Friendship

        /// <remarks />
        public static readonly string UpdateRemark = string.Format("{0}friendships/remark/update.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetFollowers = string.Format("{0}friendships/followers.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetFriends = string.Format("{0}friendships/friends.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string CreateFriendship = string.Format("{0}friendships/create.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteFriendship = string.Format("{0}friendships/destroy.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string ExistsFriendship = string.Format("{0}friendships/exists.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetFriendshipInfo = string.Format("{0}friendships/show.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetFollowingUserIDs = string.Format("{0}friendships/friends/ids.json",
                                                                          WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetFollowerUserIDs = string.Format("{0}friendships/followers/ids.json",
                                                                         WeiboUriPrefix);

        #endregion

        #region Tag

        /// <remarks />
        public static readonly string GetTags = string.Format("{0}tags.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string CreateTags = string.Format("{0}tags/create.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteTag = string.Format("{0}tags/destroy.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteTags = string.Format("{0}tags/destroy_batch.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetSuggestedTags = string.Format("{0}tags/suggestions.json", WeiboUriPrefix);

        #endregion

        #region Favorite

        /// <remarks />
        public static readonly string GetFavorites = string.Format("{0}favorites.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string AddToFavorite = string.Format("{0}favorites/create.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteFromFavorite = string.Format("{0}favorites/destroy", WeiboUriPrefix);

        /// <remarks />
        public static readonly string DeleteMultipleFromFavorite = string.Format("{0}favorites/destroy_batch.json",
                                                                                 WeiboUriPrefix);

        #endregion

        #region Block

        /// <remarks />
        public static readonly string GetBlockingList = string.Format("{0}blocks/blocking.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetBlockingListIDs = string.Format("{0}blocks/blocking/ids.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string Block = string.Format("{0}blocks/create.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string Unblock = string.Format("{0}blocks/destroy.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string IsBlocked = string.Format("{0}blocks/exists.json", WeiboUriPrefix);

        #endregion

        #region Trends(Topics)

        /// <remarks />
        public static readonly string GetUserTrends = string.Format("{0}trends.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetHourTrends = string.Format("{0}trends/hourly.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetDayTrends = string.Format("{0}trends/daily.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetWeekTrends = string.Format("{0}trends/weekly.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetTrendStatuses = string.Format("{0}trends/statuses.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string FollowTrend = string.Format("{0}trends/follow.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string UnfollowTrend = string.Format("{0}trends/destroy.json", WeiboUriPrefix);

        #endregion

        #region Short Uri(Topics)

        /// <remarks />
        public static readonly string ConvertToShortUrls = string.Format("{0}short_url/shorten.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string ConvertToLongUrls = string.Format("{0}short_url/expand.json", WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetShortUrlSharedCount = string.Format("{0}short_url/share/counts.json",
                                                                             WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetShortUrlSharedStatuses = string.Format("{0}short_url/share/statuses.json",
                                                                                WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetShortUrlCommentCount = string.Format("{0}short_url/comment/counts.json",
                                                                              WeiboUriPrefix);

        /// <remarks />
        public static readonly string GetShortUrlComments = string.Format("{0}short_url/comment/comments.json",
                                                                          WeiboUriPrefix);

        #endregion
    }
}