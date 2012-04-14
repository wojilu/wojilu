using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using wojilu.weibo.Core;
using wojilu.weibo.Core.Sina;
using wojilu.weibo.Common;
using wojilu.weibo.Data.Comment;
using wojilu.weibo.Data.Sina.Account;
using wojilu.weibo.Data.Sina.Comment;
using wojilu.weibo.Data.Sina.Common;
using wojilu.weibo.Data.Sina.Counter;
using wojilu.weibo.Data.Sina.Emotion;
using wojilu.weibo.Data.Sina.Relationship;
using wojilu.weibo.Data.Sina.Status;
using wojilu.weibo.Data.Sina.Tag;
using wojilu.weibo.Data.Sina.Trend;
using wojilu.weibo.Data.Sina.User;

namespace wojilu.weibo.Data.Sina
{
    /// <summary>
    /// Provides APIs to access weibo.com.
    /// </summary>
    public class SinaWeibo
    {
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        private string appKey, appSerect, token;

        public SinaWeibo(string appKey,string appSerect)
        {
            this.appKey = appKey;
            this.appSerect = appSerect;
        }

        public void SetToken(string token)
        {
            this.token = token;
        }

        #region OAuth(Login)

        /// <summary>
        /// Returns the authorization uri.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/Oauth/authorize
        /// </remarks>
        /// <param name="redirectUri">The url which the browser will navigate to after a success authorization. It should match the callback url set in your app.</param>
        /// <param name="responseType">The expected type of response, available option: code, token.</param>
        /// <param name="state">A customized string to pass to authorization server and will be passed back after the authorization.</param>
        /// <param name="display">The authorization page display mode, available option: default,mobile,popup,wap1.2,wap2.0,js,apponweibo.</param>
        /// <returns>The authorization code(aka oauth_verifier).</returns>
        public string GetAuthorizationUri(string redirectUri, string responseType = null, string state = null, string display = null)
        {
            var requester = new SinaHttpGet(SinaAPIUri.Authorize);
            requester.Params.Add("client_id",appKey);
            requester.Params.Add("redirect_uri", redirectUri);
            if (null != responseType)
                requester.Params.Add("response_type", responseType);
            if (null != state)
                requester.Params.Add("state", state);
            if (null != display)
                requester.Params.Add("display", display);

            return requester.GetConstructedUri();
        }

        /// <summary>
        /// Exchanges the authorization code for an access token.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/Oauth/access_token
        /// Both Http-Get and Http-Post can work. In this API is implemented with Http-Get.
        /// </remarks>
        /// <param name="authorizationCode">The authorization code previously obtained.</param>
        /// <param name="redirectUri">The redirection uri.</param>
        /// <returns></returns>
        public  SinaOAuthAccessToken GetAccessTokenByAuthorizationCode(string authorizationCode, string redirectUri)
        {
            ValidateArgument(authorizationCode, "authorizationCode");

            var requester = new ObtainTokenPost(SinaAPIUri.AccessToken);
            requester.Params.Add("client_id", appKey);
            requester.Params.Add("client_secret", appSerect);
            requester.Params.Add("grant_type", "authorization_code");
            requester.Params.Add("code", authorizationCode);
            requester.Params.Add("redirect_uri", redirectUri);

            var response = requester.Request();

            var token = JsonSerializationHelper.JsonToObject<SinaOAuthAccessToken>(response);

            return token;
        }

        /// <summary>
        /// Refreshes the access token by a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
        public SinaOAuthAccessToken RefreshAccessToken(string refreshToken)
        {
            ValidateArgument(refreshToken, "refreshToken");

            var requester = new SinaHttpGet(SinaAPIUri.AccessToken);
            requester.Params.Add("client_id", appKey);
            requester.Params.Add("client_secret", appSerect);
            requester.Params.Add("grant_type", "refresh_token");
            requester.Params.Add("refresh_token", refreshToken);

            var response = requester.Request();

            var token = JsonSerializationHelper.JsonToObject<SinaOAuthAccessToken>(response);

            

            return token;
        }

        /// <summary>
        /// Performs a OAuth login with the specified <paramref name="userName"/> and <paramref name="password"/>.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/Oauth2
        /// The app must be verified before this API can work.
        /// </remarks>
        /// <param name="userName">The user name of a SINA passport.</param>
        /// <param name="password">The password of a SINA passport.</param>
        /// <returns>The access token.</returns>
        public SinaOAuthAccessToken GetAccessTokenByPassword(string userName, string password)
        {
            ValidateArgument("username", userName);
            ValidateArgument("password", password);

            var requester = new HttpPost(SinaAPIUri.AccessToken);
            requester.Params.Add("client_id", appKey);
            requester.Params.Add("client_secret", appSerect);
            requester.Params.Add("grant_type", "password");
            requester.Params.Add("username", userName);
            requester.Params.Add("password", password);

            var response = requester.Request();

            var token = JsonSerializationHelper.JsonToObject<SinaOAuthAccessToken>(response);

            

            return token;
        }

        /// <summary>
        /// Performs a local logout. Cleans the Environment data.
        /// </summary>
        public void Logout()
        {
            
        }
        
        #endregion

        #region Account

        /// <summary>
        /// Verifies whether the current user has SINA microblog service enabled.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Account/verify_credentials </remarks>
        /// <returns>The user info.</returns>
        public UserInfo GetUID()
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetUID,token);

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Terminates the web-login user's session.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Account/end_session </remarks>
        /// <returns></returns>
        public UserInfo EndSession()
        {
            var requester = new OAuthHttpGet(SinaAPIUri.EndSession,token);

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Retrieves the app's rate limit. 
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Account/rate_limit_status </remarks>
        /// <returns></returns>
        public RateLimitStatus GetRateLimitStatus()
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetRateLimitStatus,token);

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<RateLimitStatus>(response);

            return result;
        }

        /// <summary>
        /// Updates the profile image of the current user.
        /// </summary>
        /// <remarks>
        /// Extra permission required to call this API.
        /// Returns error "40070:Error: not enough permission to call the api, pls contact the app developer." if permission insufficient.
        /// See http://open.weibo.com/wiki/2/Account/update_profile_image
        /// </remarks>
        /// <param name="imageFileLocation">The file location of the image file to upload. Limit as 700KB</param>
        /// <returns>The posted status.</returns>
        public UserInfo UpdateProfileImage(string imageFileLocation)
        {
            // Validates arguments
            ValidateArgument(imageFileLocation, "imageFileLocation");

            FileInfo picInfo = new FileInfo(imageFileLocation);
            if (picInfo.Length > 700 * 1024) // 700KB limit
            {
                throw new SinaWeiboException(LocalErrorCode.ArgumentInvalid, "Profile imgae file too large to upload.");
            }

            var requester = new OAuthMultiPartHttpPost(SinaAPIUri.UpdateProfileImage,token);

            requester.PartFields.Add(new MultiPartField() { Name = "image", FilePath = imageFileLocation });            

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Updates the profile info of current user.
        /// </summary>
        /// <remarks>
        /// Extra permission required to call this API.
        /// Returns error "40070:Error: not enough permission to call the api, pls contact the app developer." if permission insufficient.
        /// See http://open.weibo.com/wiki/2/Account/update_profile
        /// </remarks>
        /// <param name="updateProfileInfo">The update profile info.</param>
        /// <returns>The user info.</returns>
        public UserInfo UpdateProfile(UpdateProfileInfo updateProfileInfo)
        {
            ValidateArgument(updateProfileInfo, "updateProfileInfo");

            var requester = new OAuthHttpPost(SinaAPIUri.UpdateProfileImage,token);

            if(!string.IsNullOrEmpty(updateProfileInfo.ScreenName))
                requester.Params.Add("name", updateProfileInfo.ScreenName);

            if (!string.IsNullOrEmpty(updateProfileInfo.Gender))
                requester.Params.Add("gender", updateProfileInfo.Gender);

            if (!string.IsNullOrEmpty(updateProfileInfo.Description))
                requester.Params.Add("description", updateProfileInfo.Description);

            if (updateProfileInfo.Province.HasValue)
                requester.Params.Add("province", updateProfileInfo.Province.Value.ToString(InvariantCulture));

            if (updateProfileInfo.City.HasValue)
                requester.Params.Add("city", updateProfileInfo.City.Value.ToString(InvariantCulture));

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Retrieves the privacy info of the current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Account/get_privacy </remarks>
        public PrivacyInfo GetPrivacy()
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetPrivacy,token);

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<PrivacyInfo>(response);

            return result;
        }

        /// <summary>
        /// Updates the privacy settings of the current user.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/Account/update_privacy 
        /// Extra permission required to call this API.
        /// Returns error "40070:Error: not enough permission to call the api, pls contact the app developer." if permission insufficient.
        /// </remarks>
        /// <param name="commentPrivacy">
        /// The comment privacy, indicating who can comment the statuses of mine. 
        /// 0: All(default), 1: People I am following 
        /// </param>
        /// <param name="messagePrivacy">
        /// The message privacy, indicating who can send private message to me.
        /// 0: All 1:People I am following(default)
        /// </param>
        /// <param name="realNamePrivacy">
        /// The realname privacy, indicating whether others can search me by real name.
        /// 0: Allow 1: Not Allow(deault)
        /// </param>
        /// <param name="geoPrivacy">
        ///  The geo privacy, indicating whether show geo info in my statuses if there is any.
        ///  0: Allow(default) 1: Not Allow
        /// </param>
        /// <param name="badgePrivacy">
        /// The badge privacy, indicating whether display my badge.
        /// 0: Display(default) -1: Hide
        /// </param>
        /// <returns>A boolean value indicating whether the update succeed.</returns>
        public void UpdatePrivacy(int? commentPrivacy = null, int? messagePrivacy = null, int? realNamePrivacy = null, int? geoPrivacy = null, int? badgePrivacy = null)
        {
            var requester = new OAuthHttpPost(SinaAPIUri.UpdatePrivacy,token);
            if (commentPrivacy.HasValue)
                requester.Params.Add(new ParamPair("comment", commentPrivacy.Value.ToString(InvariantCulture)));
            if (messagePrivacy.HasValue)
                requester.Params.Add(new ParamPair("message", messagePrivacy.Value.ToString(InvariantCulture)));
            if (realNamePrivacy.HasValue)
                requester.Params.Add(new ParamPair("realname", realNamePrivacy.Value.ToString(InvariantCulture)));
            if (geoPrivacy.HasValue)
                requester.Params.Add(new ParamPair("geo", geoPrivacy.Value.ToString(InvariantCulture)));
            if (badgePrivacy.HasValue)
                requester.Params.Add(new ParamPair("badge", badgePrivacy.Value.ToString(InvariantCulture)));

            var response = requester.Request();
        }

        #endregion

        #region Status

        /// <summary>
        /// Retrieves the info of a microblog status by its id.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/show </remarks>
        /// <param name="statusID">The id to identify the status.</param>
        /// <returns>The status info.</returns>
        public StatusInfo GetStatus(long statusID)
        {
            ValidateArgument(statusID, "statusID");

            var requester = new OAuthHttpGet(SinaAPIUri.ShowStatus,token);
            requester.Params.Add(new ParamPair("id", statusID.ToString(InvariantCulture)));
            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<StatusInfo>(response);

            return result;
        }

        /// <summary>
        /// Posts a status to weibo.com.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/update </remarks>
        /// <param name="updateStatusInfo">The status info object.</param>
        /// <returns>The posted status.</returns>
        public StatusInfo PostStatus(UpdateStatusInfo updateStatusInfo)
        {
            // Validates arguments
            ValidateArgument(updateStatusInfo, "updateStatusInfo");
            ValidateArgument(updateStatusInfo.Status, "updateStatusInfo.Status");

            Collection<ParamPair> customParams = new Collection<ParamPair>();
            //修复原本发微博乱码问题 新浪1，2代api有差别
             customParams.Add(new ParamPair("status", RFC3986Encoder.SinaUrlEncode(updateStatusInfo.Status)));

            if (updateStatusInfo.ReplyTo.HasValue)
            {
                customParams.Add(new ParamPair("in_reply_to_status_id", updateStatusInfo.ReplyTo.Value.ToString()));
            }

            if (updateStatusInfo.Latitude.HasValue && updateStatusInfo.Longitude.HasValue)
            {
                customParams.Add(new ParamPair("lat", updateStatusInfo.Latitude.Value.ToString(InvariantCulture)));
                customParams.Add(new ParamPair("long", updateStatusInfo.Longitude.Value.ToString(InvariantCulture)));
            }

            // The status text must be url-encoded.
            var requester = new OAuthHttpPost(SinaAPIUri.UpdateStatus, token,customParams);

            var response = requester.Request();

            var statusInfo = JsonSerializationHelper.JsonToObject<StatusInfo>(response);

            return statusInfo;
        }

        /// <summary>
        /// Posts a with-pic status to weibo.com.
        /// See <seealso cref="PostStatus"/>
        /// </summary>
        /// <remarks>
        /// 'status' should not be empty, otherwise bad reuqest error 400 returned.
        /// posting duplicate status also causes return 400 error.
        /// See http://open.weibo.com/wiki/2/Statuses/upload
        ///</remarks>
        /// <param name="updateStatusWithPicInfo">The status info object.</param>
        /// <returns>The posted status.</returns>
        public StatusInfo PostStatusWithPic(UpdateStatusWithPicInfo updateStatusWithPicInfo)
        {
            // Validates arguments
            ValidateArgument(updateStatusWithPicInfo, "updateStatusWithPicInfo");
            ValidateArgument(updateStatusWithPicInfo.Status, "updateStatusWithPicInfo.Status");
            ValidateArgument(updateStatusWithPicInfo.Pic, "updateStatusWithPicInfo.Pic");

            FileInfo picInfo = new FileInfo(updateStatusWithPicInfo.Pic);
            if (picInfo.Length > 5 * 1021 * 1024) // 5MB limit
            {
                throw new SinaWeiboException(LocalErrorCode.ArgumentInvalid, "Pic file too large to post.");
            }

            var uri = SinaAPIUri.UpdateStatusWithPic;
            var requester = new OAuthMultiPartHttpPost(uri,token);

            requester.PartFields.Add(new MultiPartField() { Name = "status", Value = RFC3986Encoder.SinaUrlEncode(updateStatusWithPicInfo.Status) });
            requester.PartFields.Add(new MultiPartField() { Name = "pic", FilePath = updateStatusWithPicInfo.Pic });
            if (updateStatusWithPicInfo.Latitude.HasValue && updateStatusWithPicInfo.Longitude.HasValue)
            {
                requester.PartFields.Add(new MultiPartField() { Name = "lat", FilePath = updateStatusWithPicInfo.Latitude.Value.ToString(InvariantCulture) });
                requester.PartFields.Add(new MultiPartField() { Name = "long", FilePath = updateStatusWithPicInfo.Longitude.Value.ToString(InvariantCulture) });
            }

            var response = requester.Request();

            var statusInfo = JsonSerializationHelper.JsonToObject<StatusInfo>(response);

            return statusInfo;
        }

        /// <summary>
        /// Deletes a microblog status by its id.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/destroy </remarks>
        /// <param name="statusID">The id to identify the status.</param>
        public StatusInfo DeleteStatus(long statusID)
        {
            ValidateArgument(statusID, "statusID");

            var requester = new OAuthHttpPost(SinaAPIUri.DeleteStatus,token);
            requester.Params.Add("id", statusID.ToString(InvariantCulture));
            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<StatusInfo>(response);

            return result;
        }

        /// <summary>
        /// Re-posts a microblog status.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/repost </remarks>
        /// <param name="statusID">The id to identify the status to be reposted.</param>
        /// <param name="repostStatusText">The status text to add in addition to the reposted status.</param>
        /// <param name="commentToAuthor">Indicates whether this status is as a comment to the author of the reposted status.</param>
        /// <param name="commentToOriginalAuthor">Indicates whether this status is as a comment to the author of the original status if there is any.</param>
        /// <returns>The status info.</returns>
        public StatusInfo Forward(long statusID, string repostStatusText, bool commentToAuthor = false, bool commentToOriginalAuthor = false)
        {
            ValidateArgument(statusID, "statusID");

            var requester = new OAuthHttpPost(SinaAPIUri.RepostStatus,token);
            requester.Params.Add(new ParamPair("id", statusID.ToString(InvariantCulture)));
            requester.Params.Add(new ParamPair("status", RFC3986Encoder.UrlEncode(repostStatusText)));
            var isComment = 0;
            if (commentToAuthor)
                isComment++;
            if (commentToOriginalAuthor)
                isComment++;
            requester.Params.Add(new ParamPair("is_comment", isComment.ToString()));
            var response = requester.Request();

            var statusInfo = JsonSerializationHelper.JsonToObject<StatusInfo>(response);

            return statusInfo;
        }

        /// <summary>
        /// Comments a status.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/comment </remarks>
        /// <param name="statusID">The status id to identify the status.</param>
        /// <param name="comment">The comment text.</param>
        /// <param name="commentToOriginalAuthor">Indicate whether to comment to the original status if the status is a reposted one.</param>
        /// <returns>The comment info.</returns>
        public CommentInfo Comment(long statusID, string comment, bool commentToOriginalAuthor = false)
        {
            ValidateArgument(statusID, "statusID");
            ValidateArgument(comment, "comment");            

            var requester = new OAuthHttpPost(SinaAPIUri.Comment,token);
            requester.Params.Add(new ParamPair("id", statusID.ToString(InvariantCulture)));
            requester.Params.Add(new ParamPair("comment", RFC3986Encoder.UrlEncode(comment)));            
            if (commentToOriginalAuthor)
                requester.Params.Add(new ParamPair("comment_ori", "1"));

            var response = requester.Request();

            var commentInfo = JsonSerializationHelper.JsonToObject<CommentInfo>(response);

            return commentInfo;
        }

        /// <summary>
        /// Retrieves the comments of a specified status.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/comments </remarks>
        /// <param name="statusID">The ID of the target status</param>
        /// <param name="page">The page index. Default to 1.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 20. Maxmum as 200.</param>
        /// <returns>The comments.</returns>
        public Comments GetComments(long statusID,  int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetComments,token);

            requester.Params.Add("id", statusID.ToString(InvariantCulture));

            if (count.HasValue)
                requester.Params.Add("count", count.Value.ToString(InvariantCulture));
            if (page.HasValue)
                requester.Params.Add("page", page.Value.ToString(InvariantCulture));

            // TODO: more params.

            var response = requester.Request();

            var statuses = JsonSerializationHelper.JsonToObject<Comments>(response);

            return statuses;
        }

        /// <summary>
        /// Replies a comment of a status.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/reply </remarks>
        /// <param name="commentID">The comment id to comment to.</param>
        /// <param name="comment">The comment text.</param>
        /// <param name="statusID">The status which owns the comment.</param>
        /// <param name="withoutMention">Indicates whether to include the '@' ref.</param>
        /// <returns>The comment info.</returns>
        public CommentInfo ReplyComment(long commentID, string comment, long statusID, bool withoutMention = false)
        {
            ValidateArgument(commentID, "commentID");
            ValidateArgument(comment, "comment");

            var requester = new OAuthHttpPost(SinaAPIUri.ReplyComment,token);
            requester.Params.Add(new ParamPair("cid", commentID.ToString(InvariantCulture)));
            requester.Params.Add(new ParamPair("comment", RFC3986Encoder.UrlEncode(comment)));
            requester.Params.Add(new ParamPair("id", statusID.ToString(InvariantCulture)));           
            if (withoutMention)
                requester.Params.Add(new ParamPair("without_mention", "1"));           

            var response = requester.Request();

            var commentInfo = JsonSerializationHelper.JsonToObject<CommentInfo>(response);

            return commentInfo;
        }

        /// <summary>
        /// Deletes a comment identified by the specified <paramref name="commentID"/>.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/comment_destroy </remarks>
        /// <param name="commentID">The comment id.</param>
        /// <returns></returns>
        public CommentInfo DeleteComment(long commentID)
        {
            ValidateArgument(commentID, "commentID");

            var requester = new OAuthHttpPost(SinaAPIUri.DeleteComment,token);
            requester.Params.Add("cid", commentID.ToString(InvariantCulture));

            var response = requester.Request();

            var commentInfo = JsonSerializationHelper.JsonToObject<CommentInfo>(response);

            return commentInfo;
        }

        /// <summary>
        /// Deletes multiple comments.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/comment/destroy_batch </remarks>
        /// <param name="commentIDs">The comments ids.</param>
        /// <returns></returns>
        public Comments DeleteComments(long[] commentIDs)
        {
            ValidateArgument(commentIDs, "commentIDs");

            StringBuilder cidBuilder = new StringBuilder();
            foreach (var item in commentIDs)
            {
                cidBuilder.Append(item);
                cidBuilder.Append(",");
            }

            var requester = new OAuthHttpPost(SinaAPIUri.DeleteComments,token);
            requester.Params.Add(new ParamPair("ids", cidBuilder.ToString()));

            var response = requester.Request();

            var comments = JsonSerializationHelper.JsonToObject<Comments>(response);

            return comments;
        }

        /// <summary>
        /// Searches microblog statuses by the specified conditions.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/search 
        /// Extra permission required to call this API.
        /// </remarks>
        /// <param name="keyword">The keyword to search by.</param>
        /// <param name="statusType">The type of statuses to search. 4: forwarded, 5: original, 0: all, default to 0.</param>
        /// <param name="includePic">Indicates whether searching in statues with pic or not with pic. If not specified, search in both.</param>
        /// <param name="authorID">The author ID. If specified, perform the search in the statuses of this author only.</param>
        /// <param name="provice">The provice id.</param>
        /// <param name="city">The city id.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="page">The page index.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 10.</param>
        /// <param name="returnCounter">Indicates whether to return the counter info or not.</param>
        /// <param name="currentAppOnly">Indicates whether retrieve the statuses the posted from the current app only.</param>
        /// <returns>The statuses found.</returns>
        public Statuses SearchStatuses(string keyword, int? statusType = null, bool? includePic = null, long? authorID = null,
            int? provice = null, int? city = null, long? startTime = null, long? endTime = null,
            int? page = null, int? count = null, bool returnCounter = false, bool currentAppOnly = false)
        {
            ValidateArgument(keyword, "keyword");

            var requester = new SinaHttpGet(SinaAPIUri.SearchStatuses);
            requester.Params.Add(SinaConstants.Source, appKey);
            requester.Params.Add("q", keyword);

            if (statusType.HasValue)
                requester.Params.Add("filter_ori", statusType.Value.ToString(InvariantCulture));
            if (includePic.HasValue)
                requester.Params.Add("filter_pic", includePic.Value ? "1" : "2");

            if (provice.HasValue)
                requester.Params.Add("provice", provice.Value.ToString(InvariantCulture));
            if (city.HasValue)
                requester.Params.Add("city", city.Value.ToString(InvariantCulture));

            if (startTime.HasValue)
                requester.Params.Add("starttime", startTime.Value.ToString(InvariantCulture));
            if (endTime.HasValue)
                requester.Params.Add("endtime", endTime.Value.ToString(InvariantCulture));

            if (page.HasValue)
                requester.Params.Add("page", page.Value.ToString(InvariantCulture));
            if (count.HasValue)
                requester.Params.Add("count", count.Value.ToString(InvariantCulture));
            if (returnCounter)
                requester.Params.Add("needcount", "true");
            if (currentAppOnly)
                requester.Params.Add("base_app", "1");

            var response = requester.Request();

            response = response.Replace("<searchResult>", string.Empty);
            response = response.Replace("</searchResult>", string.Empty);

            var result = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return result;
        }

        #endregion

        #region Timeline

        /// <summary>
        /// Retrieves a certain mumber of latest statuses of the specified user or current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/user_timeline </remarks>
        /// <param name="userID">The user of whoes statues to retrieve, if not specified, default to current user.</param>
        /// <param name="sinceID">Filter condition, only retrives the statuses after <paramref name="sinceID"/>.</param>
        /// <param name="maxID">Filter condition, only retrives the statuses before <paramref name="maxID"/>.</param> 
        /// <param name="page">The page index.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 20.</param>
        /// <param name="feature">The type of statuses to retrieve.</param>
        /// <param name="currentAppOnly">Indicates whether retrieve the statuses the posted from the current app only.</param>
        /// <param name="trimUser">Indicates whether the user field of the status only return userID.</param>
        /// <returns>The statuses.</returns>
        public Statuses GetUserStatuses(long? userID = null, long? sinceID = null, long? maxID = null, int? page = null, int? count = null,
            StatusType feature = StatusType.All, bool currentAppOnly = false, bool trimUser = false)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.UserTimeline,token);
            if(userID.HasValue)
                requester.Params.Add(new ParamPair("uid", userID.Value.ToString(InvariantCulture)));
            
            ConstructPagedRecordsParams(requester.Params, sinceID, maxID, page, count);

            if (feature != StatusType.All)
                requester.Params.Add(new ParamPair("feature", ((int)feature).ToString(InvariantCulture)));
            if(currentAppOnly)
                requester.Params.Add(new ParamPair("base_app", "1"));

            if(trimUser)
                requester.Params.Add(new ParamPair("trim_user", "1"));

            var response = requester.Request();

            var statuses = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return statuses;
        }

        /// <summary>
        /// Retrieves a certain mumber of latest statuses of the public users.
        /// </summary>
        /// <remarks>
        /// Not login required.
        /// See http://open.weibo.com/wiki/2/Statuses/public_timeline 
        /// </remarks>
        /// <param name="count">The number of statuses to return, if not specified, default to 20.</param>
        /// <param name="currentAppOnly">Indicates whether retrieve the statuses the posted from the current app only.</param>
        /// <returns>The statuses retrieved.</returns>
        public Statuses GetPublicStatuses(int count = 20, bool currentAppOnly = false)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.PublicTimeline,token);

            if (count != 20)
                requester.Params.Add(new ParamPair("count", count.ToString(InvariantCulture)));            
            if (currentAppOnly)
                requester.Params.Add(new ParamPair("base_app", "1"));

            var response = requester.Request();

            var statuses = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return statuses;
        }

        /// <summary>
        /// Retrieves a certain mumber of latest statuses of the current user and the friends he is following.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/friend_timeline </remarks>
        /// <param name="sinceID">Filter condition, only retrives the statuses after <paramref name="sinceID"/>.</param>
        /// <param name="maxID">Filter condition, only retrives the statuses before <paramref name="maxID"/>.</param>
        /// <param name="page">The page index.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 20.</param>
        /// <param name="feature">The feature to retrieve.</param>
        /// <param name="currentAppOnly">Indicates whether retrieve the statuses the posted from the current app only.</param>
        /// <returns>The statuses.</returns>
        public Statuses GetFriendsStatuses(long? sinceID = null, long? maxID = null, int? page = null, int? count = null,
            StatusType feature = StatusType.All, bool currentAppOnly = false)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.FriendsTimeline,token); // The same as HomeTimeline
            
            ConstructPagedRecordsParams(requester.Params, sinceID, maxID, page, count);
            
            if (feature != StatusType.All)
                requester.Params.Add(new ParamPair("feature", ((int)feature).ToString(InvariantCulture)));
            if (currentAppOnly)
                requester.Params.Add(new ParamPair("base_app", "1"));

            var response = requester.Request();

            var statuses = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return statuses;
        }

        /// <summary>
        /// Retrieves the statuses which mentions the current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/mentions </remarks>
        /// <param name="sinceID">Filter condition, only retrives the statuses after <paramref name="sinceID"/>.</param>
        /// <param name="maxID">Filter condition, only retrives the statuses before <paramref name="maxID"/>.</param>
        /// <param name="page">The page index.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 20.</param>
        /// <returns>The statuses.</returns>
        public Statuses GetMentions(long? sinceID = null, long? maxID = null, int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetMentions,token);

            ConstructPagedRecordsParams(requester.Params, sinceID, maxID, page, count);

            var response = requester.Request();

            var statuses = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return statuses;
        }

        /// <summary>
        /// Retrieves the comments sent/received by the current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/comments_timeline </remarks>
        /// <param name="sinceID">Filter condition, only retrives the statuses after <paramref name="sinceID"/>.</param>
        /// <param name="maxID">Filter condition, only retrives the statuses before <paramref name="maxID"/>.</param>
        /// <param name="page">The page index.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 20.</param>
        /// <returns>The statuses.</returns>
        public Comments GetCommentsTimeline(long? sinceID = null, long? maxID = null, int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.CommentsTimeline,token);

            ConstructPagedRecordsParams(requester.Params, sinceID, maxID, page, count);

            var response = requester.Request();

            var statuses = JsonSerializationHelper.JsonToObject<Comments>(response);

            return statuses;
        }

        /// <summary>
        /// Retrieves the comments sent by the current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/comments_by_me </remarks>
        /// <param name="sinceID">Filter condition, only retrives the statuses after <paramref name="sinceID"/>.</param>
        /// <param name="maxID">Filter condition, only retrives the statuses before <paramref name="maxID"/>.</param>
        /// <param name="page">The page index.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 20.</param>
        /// <returns>The statuses.</returns>
        public Comments GetCommentsByMe(long? sinceID = null, long? maxID = null, int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.CommentsByMe,token);

            ConstructPagedRecordsParams(requester.Params, sinceID, maxID, page, count);

            var response = requester.Request();

            var comments = JsonSerializationHelper.JsonToObject<Comments>(response);

            return comments;
        }

        /// <summary>
        /// Retrieves the comments sent to the current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/comments_to_me </remarks>
        /// <param name="sinceID">Filter condition, only retrives the statuses after <paramref name="sinceID"/>.</param>
        /// <param name="maxID">Filter condition, only retrives the statuses before <paramref name="maxID"/>.</param>
        /// <param name="page">The page index.</param>
        /// <param name="count">The number of statuses to return, if not specified, default to 20.</param>
        /// <returns>The statuses.</returns>
        public Comments GetCommentsToMe(long? sinceID = null, long? maxID = null, int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.CommentsToMe,token);

            ConstructPagedRecordsParams(requester.Params, sinceID, maxID, page, count);

            var response = requester.Request();

            var statuses = JsonSerializationHelper.JsonToObject<Comments>(response);

            return statuses;
        }

        /// <summary>
        /// Retrieves the comment counter and repost counter of the specified statuses.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/counts </remarks>
        /// <param name="statusIDs">The ids of the statuses to query. At most 20.</param>
        /// <returns>The counters.</returns>
        public Collection<CounterInfo> GetCountersOfCommentNForward(long[] statusIDs)
        {
            ValidateArgument(statusIDs, "statusIDs");

            var requester = new OAuthHttpGet(SinaAPIUri.GetCountersOfCommentNForward,token);

            var statusIDsBuilder = new StringBuilder();
            for (int i = 0; i < statusIDs.Length; i++)
            {
                statusIDsBuilder.Append(statusIDs[i]);
                if (i < statusIDs.Length - 1)
                {
                    statusIDsBuilder.Append(",");
                }
            }
            requester.Params.Add(new ParamPair("ids", statusIDsBuilder.ToString()));

            var response = requester.Request();

            var counters = JsonSerializationHelper.JsonToObject<Collection<CounterInfo>>(response);

            return counters;
        }

        /// <summary>
        /// Retrieves the unread counters of the current user.
        /// </summary>
        /// <remarks>
        /// See http://rm.api.weibo.com/2/remind/unread_count.json
        /// </remarks>
        /// <param name="userID">The user id.</param>
        /// <returns></returns>
        public UnreadInfo GetUnreadInfo(long userID)
        {
            // TODO: Not working
            var requester = new SinaHttpGet(SinaAPIUri.GetUnreadInfo);

            requester.Params.Add("source", appSerect);
            requester.Params.Add("uid", userID.ToString(InvariantCulture));

            var response = requester.Request();

            var unreadInfo = JsonSerializationHelper.JsonToObject<UnreadInfo>(response);

            return unreadInfo;
        }

        /// <summary>
        /// Resets the unread counters of the specified type to zero.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/reset_count </remarks>
        /// <param name="counterType">The type of counter to reset.</param>
        /// <returns>A boolean value indicating whether the counter resets to zero.</returns>
        public void ResetCounter(CounterType counterType)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.ResetCounter,token);
            requester.Params.Add(new ParamPair("type", ((int)counterType).ToString(InvariantCulture)));

            var response = requester.Request();
        }

        /// <summary>
        /// Retrieves the system defined emotions.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Emotions </remarks>
        /// <param name="emotionType">The type of emotion to retrive.</param>
        /// <param name="language">The language. i.e: cnname, twname</param>
        /// <returns>The emotions retrieved.</returns>
        public Collection<EmotionInfo> GetEmotions(EmotionType emotionType , string language )
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetEmotions,token);
            if (emotionType != EmotionType.Image)
            {
                var eType = string.Empty;
                switch (emotionType)
                {
                    case EmotionType.Image:
                        eType = "face";
                        break;
                    case EmotionType.Magic:
                        eType = "ani";
                        break;
                    case EmotionType.Cartoon:
                        eType = "cartoon";
                        break;
                    default:
                        eType = "face";
                        break;
                }
                requester.Params.Add(new ParamPair("type", eType));
            }

            if(!string.IsNullOrEmpty(language))
                requester.Params.Add(new ParamPair("language", language));

            var response = requester.Request();

            var emotions = JsonSerializationHelper.JsonToObject<Collection<EmotionInfo>>(response);

            return emotions;
        }

        #endregion

        #region User

        /// <summary>
        /// Retrives the info of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Users/show </remarks>
        /// <param name="userID">The user id.</param>
        /// <returns>The user info.</returns>
        public UserInfo GetUserInfo(long userID)
        {
            var requester = new OAuthHttpGet(string.Format("{0}.json",SinaAPIUri.GetUserInfo, userID),token);

            requester.Params.Add("uid", userID.ToString());

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Retrives the info of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Users/show </remarks>
        /// <param name="screenName">The screen name of the user to retrieve.</param>
        /// <returns>The user info retrieved.</returns>
        public UserInfo GetUserInfo(string screenName)
        {
            ValidateArgument(screenName, "screenName");

            var requester = new OAuthHttpGet(string.Format("{0}.json", SinaAPIUri.GetUserInfo),token);

            requester.Params.Add(new ParamPair("screen_name", RFC3986Encoder.UrlEncode(screenName)));

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Retrieves the friends of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/friends </remarks>
        /// <param name="userID">The user id. If not provided, defaults to current user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The friends retrieved.</returns>
        public Users GetFriends(long userID, int? cursor , int? count )
        {
            return GetFriends(userID, null, cursor, count);
        }

        /// <summary>
        /// Retrieves the friends of the specified user.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/Statuses/friends 
        /// A friend is a microblog user you are following.
        /// </remarks>
        /// <param name="screenName">The screen name of the user. If not provided, defaults to current user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The friends retrieved.</returns>
        public Users GetFriends(string screenName, int? cursor , int? count )
        {
            ValidateArgument(screenName, "screenName");

            return GetFriends(null, screenName, cursor, count);
        }

        /// <summary>
        /// Retrieves the friends of the specified user.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/Statuses/friends 
        /// One of the <paramref name="userID"/> and <paramref name="screenName"/> should be provided. If not provided, defaults to current user.
        /// </remarks>
        /// <param name="userID">The user id.</param>
        /// <param name="screenName">The screen name of the user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The friends retrieved.</returns>
        public Users GetFriends(long? userID , string screenName , int? cursor, int? count )
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetFriends,token);
            if (userID.HasValue)
                requester.Params.Add(new ParamPair("uid", userID.Value.ToString(InvariantCulture)));

            if (!string.IsNullOrEmpty(screenName))
                requester.Params.Add(new ParamPair("screen_Name", RFC3986Encoder.UrlEncode(screenName)));

            if (cursor.HasValue)
                requester.Params.Add(new ParamPair("cursor", cursor.Value.ToString(InvariantCulture)));

            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            var users = JsonSerializationHelper.JsonToObject<Users>(response);

            return users;
        }

        /// <summary>
        /// Retrieves the followers of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/followers </remarks>
        /// <param name="userID">The user id.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The followers retrieved.</returns>
        public Users GetFollowers(long userID, int? cursor , int? count )
        {
            return GetFollowers(userID, null, cursor, count);
        }

        /// <summary>
        /// Retrieves the followers of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Statuses/followers </remarks>
        /// <param name="screenName">The screen name of the user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The followers retrieved.</returns>
        public Users GetFollowers(string screenName, int? cursor = null, int? count = null)
        {
            ValidateArgument(screenName, "screenName");

            return GetFollowers(null, screenName, cursor, count);
        }

        /// <summary>
        /// Retrieves the followers of the specified user.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/Statuses/followers 
        /// One of the <paramref name="userID"/> and <paramref name="screenName"/> should be provided. If not provided, defaults to current user.
        /// </remarks>
        /// <param name="userID">The user id.</param>
        /// <param name="screenName">The screen name of the user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The followers retrieved.</returns>
        public Users GetFollowers(long? userID = null, string screenName = null, int? cursor = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetFollowers,token);
            if (userID.HasValue)
                requester.Params.Add(new ParamPair("uid", userID.Value.ToString(InvariantCulture)));

            if (!string.IsNullOrEmpty(screenName))
                requester.Params.Add(new ParamPair("screen_Name", RFC3986Encoder.UrlEncode(screenName)));

            if (cursor.HasValue)
                requester.Params.Add(new ParamPair("cursor", cursor.Value.ToString(InvariantCulture)));

            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            var users = JsonSerializationHelper.JsonToObject<Users>(response);

            return users;
        }

        /// <summary>
        /// Retrieves the hot users of the specified <paramref name="category"/>.
        /// </summary>
        /// <remarks>
        /// default 人气关注
        /// ent 影视名星
        /// hk_famous 港台名人
        /// model 模特
        /// cooking 美食&amp;健康
        /// sport 体育名人
        /// finance 商界名人
        /// tech IT互联网
        /// singer 歌手
        /// writer 作家
        /// moderator 主持人
        /// medium 媒体总编
        /// stockplayer 炒股高手
        /// See http://open.weibo.com/wiki/2/Users/hot
        /// </remarks>
        /// <param name="category">The user category, if not specified, default to 'default'.</param>
        /// <returns>The users returned.</returns>
        public Users GetHotUsers(string category = "")
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetHotUsers,token);

            if (!string.IsNullOrEmpty(category))
                requester.Params.Add(new ParamPair("category", category));

            var response = requester.Request();

            var users = JsonSerializationHelper.JsonToObject<Users>(response);

            return users;
        }

        /// <summary>
        /// Updates the remark of a friend of the current user.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/User/friends/update_remark
        /// </remarks>
        /// <param name="friendUserID">The friend's user id.</param>
        /// <param name="remark">The remark.</param>
        /// <returns>The friend's info.</returns>
        public Users UpdateFriendRemark(long friendUserID, string remark)
        {
            var requester = new OAuthHttpPost(SinaAPIUri.UpdateRemark,token);

            requester.Params.Add(new ParamPair("uid", friendUserID.ToString(InvariantCulture)));
            requester.Params.Add(new ParamPair("remark", RFC3986Encoder.UrlEncode(remark)));

            var response = requester.Request();

            var users = JsonSerializationHelper.JsonToObject<Users>(response);

            return users;
        }

        /// <summary>
        /// Returns a bunch of users the current user may be interested in.
        /// </summary>
        /// <remarks>
        /// See http://open.weibo.com/wiki/2/Users/suggestions
        /// </remarks>
        /// <returns>The users returned.</returns>
        public Users GetSuggestedUsers()
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetSuggestedUsers,token);

            var response = requester.Request();

            var users = JsonSerializationHelper.JsonToObject<Users>(response);

            return users;
        }

        /// <summary>
        /// Retrieves the users by the specified keyword.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Users/search 
        /// Extra permission required to call this API.
        /// </remarks>
        /// <param name="keyword">The keyword to search by.</param>
        /// <param name="page">The page index.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 10.</param>
        /// <param name="currentAppOnly">Indicates whether retrieve the statuses the posted from the current app only.</param>
        /// <returns>The users found.</returns>
        public Users SearchUsers(string keyword, int? page, int? count , bool currentAppOnly )
        {
            ValidateArgument("keyword", keyword);

            var requester = new SinaHttpGet(SinaAPIUri.SearchUsers);
            requester.Params.Add(SinaConstants.Source, appKey);

            requester.Params.Add("q", RFC3986Encoder.UrlEncode(keyword));
            if (count.HasValue)
                requester.Params.Add("count", count.Value.ToString(InvariantCulture));
            if (page.HasValue)
                requester.Params.Add("page", page.Value.ToString(InvariantCulture));
            if (currentAppOnly)
                requester.Params.Add("base_app ", "1");

            var response = requester.Request();

            response = response.Replace("<searchResult>", string.Empty);
            response = response.Replace("</searchResult>", string.Empty);

            var result = JsonSerializationHelper.JsonToObject<Users>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the user suggestions by the specified keyword.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Search/suggestions/at_users </remarks>
        /// <param name="keyword">The keyword to search by.</param>
        /// <param name="type">The type of uses to search. 0: friends, 1: followers</param>
        /// <param name="count">The count of a page. If not provided, defaults to 10.</param>
        /// <param name="range">The range to search in. 0: search in name, 1: search in remark, 2: search both. Default to 2.</param>
        /// <returns>The user suggestions found.</returns>
        public Collection<UserSuggestionInfo> GetUserSearchSuggestions(string keyword, int type , int? count , int? range)
        {
            ValidateArgument("keyword", keyword);

            var requester = new OAuthHttpGet(SinaAPIUri.GetUserSearchSuggestions,token);

            requester.Params.Add("q", RFC3986Encoder.UrlEncode(keyword));
            if (count.HasValue)
                requester.Params.Add("count", count.Value.ToString(InvariantCulture));
            requester.Params.Add("type", type.ToString(InvariantCulture));
            if (range.HasValue)
                requester.Params.Add("range", range.Value.ToString(InvariantCulture));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Collection<UserSuggestionInfo>>(response);

            return result;
        }

        #endregion

        #region Friendship

        /// <summary>
        /// Follows a specified user to become a fan of him/her.
        /// </summary>
        /// <remarks>
        /// One of the <paramref name="targetUserID"/> and <paramref name="targetUserScreenName"/> must be provided.
        /// See http://open.weibo.com/wiki/2/Friendships/create
        /// </remarks>
        /// <param name="targetUserID">The id of the user to follow.</param>
        /// <param name="targetUserScreenName">The screen name of the user to follow.</param>
        /// <returns>The info of the user followed.</returns>
        public UserInfo Follow(long? targetUserID, string targetUserScreenName = null)
        {
            var requester = new OAuthHttpPost(SinaAPIUri.CreateFriendship,token);

            if(targetUserID.HasValue)
                requester.Params.Add(new ParamPair("uid", targetUserID.Value.ToString(InvariantCulture)));
            if (!string.IsNullOrEmpty(targetUserScreenName))
                requester.Params.Add(new ParamPair("screen_name", targetUserScreenName));

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Follows a specified user to become a fan of him/her.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friendships/create </remarks>
        /// <param name="targetUserID">The id of the user to follow.</param>
        /// <returns>The info of the user followed.</returns>
        public UserInfo Follow(long targetUserID)
        {
            return Follow(targetUserID, null);
        }

        /// <summary>
        /// Follows a specified user to become a fan of him/her.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friendships/create </remarks>
        /// <param name="targetUserScreenName">The screen name of the user to follow.</param>
        /// <returns>The info of the user followed.</returns>
        public UserInfo Follow(string targetUserScreenName)
        {
            ValidateArgument(targetUserScreenName, "targetUserScreenName");

            return Follow(null, targetUserScreenName);
        }

        /// <summary>
        /// Unfollows a specified user.
        /// </summary>
        /// <remarks>One of the <paramref name="targetUserID"/> and <paramref name="targetUserScreenName"/> must be provided.
        /// See http://open.weibo.com/wiki/2/Friendships/destroy
        /// </remarks>
        /// <param name="targetUserID">The id of the user to follow.</param>
        /// <param name="targetUserScreenName">The screen name of the user to follow.</param>
        /// <returns>The info of the user.</returns>
        public UserInfo Unfollow(long? targetUserID = null, string targetUserScreenName = null)
        {
            var requester = new OAuthHttpPost(SinaAPIUri.DeleteFriendship,token);

            if (targetUserID.HasValue)
                requester.Params.Add(new ParamPair("uid", targetUserID.Value.ToString(InvariantCulture)));

            if (!string.IsNullOrEmpty(targetUserScreenName))
                requester.Params.Add(new ParamPair("screen_name", targetUserScreenName));

            var response = requester.Request();

            var userInfo = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return userInfo;
        }

        /// <summary>
        /// Unfollows a specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friendships/destroy </remarks>
        /// <param name="targetUserID">The id of the user to follow.</param>
        /// <returns>The info of the user.</returns>
        public UserInfo Unfollow(long targetUserID)
        { 
            return Unfollow(targetUserID, null);
        }

        /// <summary>
        /// Unfollows a specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friendships/destroy </remarks>
        /// <param name="targetUserScreenName">The screen name of the user to follow.</param>
        /// <returns>The info of the user.</returns>
        public UserInfo Unfollow(string targetUserScreenName)
        {
            ValidateArgument(targetUserScreenName, "targetUserScreenName");

            return Unfollow(null, targetUserScreenName);
        }

        /// <summary>
        /// Checks whether <paramref name="userAID"/> followed <paramref name="userBID"/>.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friendships/exists </remarks>
        /// <param name="userAID">The id of the user A.</param>
        /// <param name="userBID">The id of the user B.</param>
        /// <returns>The info of the user.</returns>
        public bool ExistsFriendship(long userAID, long userBID)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.ExistsFriendship,token);

            requester.Params.Add(new ParamPair("user_a", userAID.ToString(InvariantCulture)));
            requester.Params.Add(new ParamPair("user_b", userBID.ToString(InvariantCulture)));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<ExistsFriendshipResultInfo>(response);

            return result.Value;
        }

        /// <summary>
        /// Retrieves the friendship info of the users identified by <paramref name="targetUserID"/> and <paramref name="sourceUserID"/>.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friendships/show </remarks>
        /// <param name="targetUserID">The id of the target user.</param>
        /// <param name="sourceUserID">The id of the source user. If not provided, default to current user.</param>
        /// <returns>The info of the friendship.</returns>
        public RelationshipInfo GetFriendshipInfo(long sourceUserID, long targetUserID)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetFriendshipInfo,token);

            requester.Params.Add(new ParamPair("source_id", sourceUserID.ToString(InvariantCulture)));
            requester.Params.Add(new ParamPair("target_id", targetUserID.ToString(InvariantCulture)));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<RelationshipInfo>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the friendship info of the users identified by <paramref name="sourceUserScreenName"/> and <paramref name="targetUserScreenName"/>.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friendships/show </remarks>
        /// <param name="sourceUserScreenName">The screen name of the target user.</param>
        /// <param name="targetUserScreenName">The screen name of the source user. If not provided, default to current user.</param>
        /// <returns>The info of the friendship.</returns>
        public RelationshipInfo GetFriendshipInfo(string sourceUserScreenName, string targetUserScreenName)
        {
            ValidateArgument(sourceUserScreenName, "sourceUserScreenName");
            ValidateArgument(targetUserScreenName, "targetUserScreenName");

            var requester = new OAuthHttpGet(SinaAPIUri.GetFriendshipInfo,token);

            requester.Params.Add(new ParamPair("source_screen_name", sourceUserScreenName));
            requester.Params.Add(new ParamPair("target_screen_name", targetUserScreenName));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<RelationshipInfo>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the id of the users that the specified user is following.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friends/ids </remarks>
        /// <param name="userID">The id of the user. If not provided, defaults to current user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The user ids.</returns>
        public UserIDs GetFollowingUserIDs(long userID, int? cursor = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetFollowingUserIDs,token);

            requester.Params.Add(new ParamPair("uid", userID.ToString(InvariantCulture)));

            if (cursor.HasValue)
                requester.Params.Add(new ParamPair("cursor", cursor.Value.ToString(InvariantCulture)));
            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<UserIDs>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the id of the users that the specified user is following.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Friends/ids </remarks>
        /// <param name="screenName">The id of the user. If not provided, defaults to current user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The user ids.</returns>
        public UserIDs GetFollowingUserIDs(string screenName, int? cursor = null, int? count = null)
        {
            ValidateArgument(screenName, "screenName");

            var requester = new OAuthHttpGet(SinaAPIUri.GetFollowingUserIDs,token);

            requester.Params.Add(new ParamPair("screen_name", screenName));
            if (cursor.HasValue)
                requester.Params.Add(new ParamPair("cursor", cursor.Value.ToString(InvariantCulture)));
            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<UserIDs>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the id of the users who follows the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Followers/ids </remarks>
        /// <param name="userID">The id of the user. If not provided, defaults to current user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The user ids.</returns>
        public UserIDs GetFollowerUserIDs(long userID, int? cursor = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetFollowerUserIDs,token);

                requester.Params.Add(new ParamPair("uid", userID.ToString(InvariantCulture)));
            
            if (cursor.HasValue)
                requester.Params.Add(new ParamPair("cursor", cursor.Value.ToString(InvariantCulture)));
            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<UserIDs>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the id of the users who follows the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Followers/ids </remarks>
        /// <param name="screenName">The screen name of the user. If not provided, defaults to current user.</param>
        /// <param name="cursor">The cursor. -1 means the first page. If provides, next cursor will be returned in the response.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <returns>The user ids.</returns>
        public UserIDs GetFollowerUserIDs(string screenName, int? cursor = null, int? count = null)
        {
            ValidateArgument(screenName, "screenName");
            var requester = new OAuthHttpGet(SinaAPIUri.GetFollowerUserIDs,token);

            requester.Params.Add(new ParamPair("screen_name", RFC3986Encoder.UrlEncode(screenName)));
            if (cursor.HasValue)
                requester.Params.Add(new ParamPair("cursor", cursor.Value.ToString(InvariantCulture)));
            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<UserIDs>(response);

            return result;
        }

        #endregion

        #region Tag

        /// <summary>
        /// Retrieves the tags of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Tags </remarks>
        /// <param name="userID">The id of the user.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <param name="page">The page no.</param>
        /// <returns>The tags returned.</returns>
        public Collection<TagInfo> GetTags(long userID, int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetTags,token);

            requester.Params.Add("uid", userID.ToString(InvariantCulture));
            if (page.HasValue)
                requester.Params.Add(new ParamPair("page", page.Value.ToString(InvariantCulture)));
            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            response = Regex.Replace(response, @"""(\d+)"":", @"""id"":$1,""value"":");

            var result = JsonSerializationHelper.JsonToObject<Collection<TagInfo>>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the tags of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Tags/suggestions </remarks>
        /// <param name="count">The count of a page. If not provided, defaults to 10.</param>
        /// <param name="page">The page no.</param>
        /// <returns>The tags returned.</returns>
        public Collection<TagInfo> GetSuggestedTags(int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetSuggestedTags,token);

            if (count.HasValue)
                requester.Params.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Collection<TagInfo>>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the tags of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Tags/create </remarks>
        /// <param name="tags">The tags. 10 at most for each user.</param>
        /// <returns>The id of the tags returned.</returns>
        public Collection<TagID> CreateTags(params string[] tags)
        {
            ValidateArgument(tags, "tags");

            var count = tags.Length;
            if (count == 0)
                throw new SinaWeiboException(LocalErrorCode.ArgumentNotProvided, "Tags not provided.");

            var tagBuilder = new StringBuilder();
            for (int i = 0; i < tags.Length; i++)
            {
                tagBuilder.Append(tags[i]);
                if( i < count -1)
                    tagBuilder.Append(",");
            }

            var requester = new OAuthHttpPost(SinaAPIUri.CreateTags,token);

            requester.Params.Add("tags", tagBuilder.ToString());

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Collection<TagID>>(response);

            return result;
        }

        /// <summary>
        /// Deletes a user tag by the <paramref name="tagID"/>.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Tags/destroy </remarks>
        /// <param name="tagID">The id of the tag.</param>
        public void DeleteTag(long tagID)
        {
            var requester = new OAuthHttpDelete(SinaAPIUri.DeleteTag,token);

            requester.Params.Add("tag_id", tagID.ToString(InvariantCulture));

            requester.Request();
        }

        /// <summary>
        /// Deletes a bunch of user tags by the <paramref name="tagIDs"/>.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Tags/destroy_batch </remarks>
        /// <param name="tagIDs">The ids of the tags to delete. At most 20 each time.</param>
        public Collection<TagID> DeleteTags(params long[] tagIDs)
        {
            ValidateArgument(tagIDs, "tagIDs");

            var count = tagIDs.Length;
            if (count == 0)
                throw new SinaWeiboException(LocalErrorCode.ArgumentNotProvided, "Tag IDs not provided.");

            var tagBuilder = new StringBuilder();
            for (int i = 0; i < tagIDs.Length; i++)
            {
                tagBuilder.Append(tagIDs[i]);
                if (i < count - 1)
                    tagBuilder.Append(",");
            }

            var requester = new OAuthHttpDelete(SinaAPIUri.DeleteTags,token);

            requester.Params.Add("ids", tagBuilder.ToString());

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Collection<TagID>>(response);

            return result;
        }

        #endregion

        #region Favorite

        /// <summary>
        /// Retrieves the favorite of the current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Favorites </remarks>
        /// <param name="page">The page no.</param>
        /// <returns>The statuses in favorite.</returns>
        public Statuses GetFavorites(int? page = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetFavorites,token);

            if (page.HasValue)
                requester.Params.Add(new ParamPair("page", page.Value.ToString(InvariantCulture)));

            var response = requester.Request();

            // TODO: new data contract
            response = response.Replace(@"""favorites"":",@"""statuses"":");

            var result = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return result;
        }

        /// <summary>
        /// Adds the specified status into the favorite of current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Favorites/create </remarks>
        /// <param name="statusID">The id of the status to add to.</param>
        /// <returns>The status info.</returns>
        public StatusInfo AddToFavorite(long statusID)
        {
            var requester = new OAuthHttpPost(SinaAPIUri.AddToFavorite,token);

            requester.Params.Add("id", statusID.ToString(InvariantCulture));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<StatusInfo>(response);

            return result;
        }

        /// <summary>
        /// Deletes the specified status from the favorite of current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Favorites/destroy </remarks>
        /// <param name="statusID">The id of the status to delete from.</param>
        /// <returns>The status info.</returns>
        public StatusInfo DeleteFromFavorite(long statusID)
        {
            var requester = new OAuthHttpDelete(string.Format("{0}/{1}.json", SinaAPIUri.DeleteFromFavorite, statusID),token);

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<StatusInfo>(response);

            return result;
        }

        /// <summary>
        /// Deletes the specified statuses from the favorite of current user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Favorites/destroy_batch </remarks>
        /// <param name="statusIDs">The ids of the status to delete from.</param>
        /// <returns>The status info.</returns>
        public Statuses DeleteMultipleFromFavorite(params long[] statusIDs)
        {
            ValidateArgument(statusIDs, "statusIDs");

            var count = statusIDs.Length;
            if (count == 0)
                throw new SinaWeiboException(LocalErrorCode.ArgumentNotProvided, "Status IDs not provided.");

            var tagBuilder = new StringBuilder();
            for (int i = 0; i < statusIDs.Length; i++)
            {
                tagBuilder.Append(statusIDs[i]);
                if (i < count - 1)
                    tagBuilder.Append(",");
            }

            var requester = new OAuthHttpDelete(SinaAPIUri.DeleteMultipleFromFavorite,token);

            requester.Params.Add("ids", tagBuilder.ToString());

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return result;
        }

        #endregion

        #region Block

        /// <summary>
        /// Retrieves the block list of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Blocks/blocking </remarks>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <param name="page">The page no.</param>
        /// <returns>The blocking users returned.</returns>
        public Users GetBlockingList(int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetBlockingList,token);

            if (page.HasValue)
                requester.Params.Add("page", page.Value.ToString(InvariantCulture));
            if (count.HasValue)
                requester.Params.Add("count", count.Value.ToString(InvariantCulture));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Users>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the user ids of the block list of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Blocks/blocking/ids </remarks>
        /// <param name="count">The count of a page. If not provided, defaults to 20.</param>
        /// <param name="page">The page no.</param>
        /// <returns>The blocking users returned.</returns>
        public UserIDs GetBlockingListIDs(int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetBlockingListIDs,token);

            if (page.HasValue)
                requester.Params.Add("page", page.Value.ToString(InvariantCulture));
            if (count.HasValue)
                requester.Params.Add("count", count.Value.ToString(InvariantCulture));

            var response = requester.Request();

            response = response.Replace("<ids>", "<id_list><ids>");
            response = response.Replace("</ids>", "</ids></id_list>");

            var result = JsonSerializationHelper.JsonToObject<UserIDs>(response);

            return result;
        }

        /// <summary>
        /// Blocks the specified user for current user.
        /// </summary>
        /// <remarks>
        /// One of the <paramref name="userID"/> and <paramref name="screenName"/> must be specified.
        /// See http://open.weibo.com/wiki/2/Blocks/create 
        /// </remarks>
        /// <param name="userID">The id of the user to block.</param>
        /// <param name="screenName">The screen name of the user to block.</param>
        /// <returns>The blocking user info.</returns>
        public UserInfo Block(long? userID = null, string screenName = null)
        {
            var requester = new OAuthHttpPost(SinaAPIUri.Block,token);

            if (userID.HasValue)
                requester.Params.Add("uid", userID.Value.ToString(InvariantCulture));
            if (!string.IsNullOrEmpty(screenName))
                requester.Params.Add("screen_name", RFC3986Encoder.UrlEncode(screenName));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return result;
        }

        /// <summary>
        /// Unblocks the specified user from the block list of current user.
        /// </summary>
        /// <remarks>
        /// One of the <paramref name="userID"/> and <paramref name="screenName"/> must be specified.
        /// See http://open.weibo.com/wiki/2/Blocks/destroy 
        /// </remarks>
        /// <param name="userID">The id of the user to unblock.</param>
        /// <param name="screenName">The screen name of the user to unblock.</param>
        /// <returns>The blocking user info.</returns>
        public UserInfo Unblock(long? userID = null, string screenName = null)
        {
            var requester = new OAuthHttpDelete(SinaAPIUri.Unblock,token);

            if (userID.HasValue)
                requester.Params.Add("uid", userID.Value.ToString(InvariantCulture));
            if (!string.IsNullOrEmpty(screenName))
                requester.Params.Add("screen_name", RFC3986Encoder.UrlEncode(screenName));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<UserInfo>(response);

            return result;
        }

        /// <summary>
        /// Checks whether the specified user is in the block list of current user.
        /// </summary>
        /// <remarks>
        /// One of the <paramref name="userID"/> and <paramref name="screenName"/> must be specified.
        /// See http://open.weibo.com/wiki/2/Blocks/exists 
        /// </remarks>
        /// <param name="userID">The id of the user to check.</param>
        /// <param name="screenName">The screen name of the user to check.</param>
        /// <returns>The blocking user info.</returns>
        public bool IsBlocked(long? userID = null, string screenName = null)
        {
            var requester = new OAuthHttpDelete(SinaAPIUri.IsBlocked,token);

            if (userID.HasValue)
                requester.Params.Add("uid", userID.Value.ToString(InvariantCulture));
            if (!string.IsNullOrEmpty(screenName))
                requester.Params.Add("screen_name", RFC3986Encoder.UrlEncode(screenName));

            var response = requester.Request();

            var result = response.Contains("true");

            return result;
        }

        #endregion

        #region Trend(Topic)

        /// <summary>
        /// Retrieves the trends(topic) of the specified user.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Trends </remarks>
        /// <param name="userID">The id of the user. </param>
        /// <param name="page">The page no.  If not provided, defaults to 1.</param>
        /// <param name="count">The count of a page. If not provided, defaults to 10.</param>
        /// <returns>The trends returned.</returns>
        public Collection<TrendInfo> GetUserTrends(long userID, int? page = null, int? count = null)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetUserTrends,token);

            requester.Params.Add("uid", userID.ToString(InvariantCulture));

            if (page.HasValue)
                requester.Params.Add("page", page.Value.ToString(InvariantCulture));
            if (count.HasValue)
                requester.Params.Add("count", count.Value.ToString(InvariantCulture));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Collection<TrendInfo>>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the trends(topic) of current hour.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Trends/hourly </remarks>
        /// <param name="basedOnCurrentApp">Indicates whether to retrieve based on current app.</param>
        /// <returns>The trends returned.</returns>
        public PeriodTrends GetHourTrends(bool basedOnCurrentApp = false)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetHourTrends,token);

            if (basedOnCurrentApp)
                requester.Params.Add("base_app", "1");
            else
                requester.Params.Add("base_app", "0");
            
            var response = requester.Request();

            response = Regex.Replace(response, @"(\d{4}-\d{1,2}-\d{1,2}\s\d{1,2}:\d{1,2})", @"time"":""$1"",""trend");

            var result = JsonSerializationHelper.JsonToObject<PeriodTrends>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the trends(topic) of today.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Trends/daily </remarks>
        /// <param name="basedOnCurrentApp">Indicates whether to retrieve based on current app.</param>
        /// <returns>The trends returned.</returns>
        public PeriodTrends GetDayTrends(bool basedOnCurrentApp = false)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetDayTrends,token);

            if (basedOnCurrentApp)
                requester.Params.Add("base_app", "1");
            else
                requester.Params.Add("base_app", "0");

            var response = requester.Request();

            response = Regex.Replace(response, @"\d{4}-\d{1,2}-\d{1,2}\s\d{1,2}:\d{1,2}", "trend");

            var result = JsonSerializationHelper.JsonToObject<PeriodTrends>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the trends(topic) of current week.
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Trends/weekly </remarks>
        /// <param name="basedOnCurrentApp">Indicates whether to retrieve based on current app.</param>
        /// <returns>The trends returned.</returns>
        public PeriodTrends GetWeekTrends(bool basedOnCurrentApp = false)
        {
            var requester = new OAuthHttpGet(SinaAPIUri.GetWeekTrends,token);

            if (basedOnCurrentApp)
                requester.Params.Add("base_app", "1");
            else
                requester.Params.Add("base_app", "0");

            var response = requester.Request();

            response = Regex.Replace(response, @"\d{4}-\d{1,2}-\d{1,2}\s\d{1,2}:\d{1,2}", "trend");

            var result = JsonSerializationHelper.JsonToObject<PeriodTrends>(response);

            return result;
        }

        /// <summary>
        /// Retrieves the statuses under the specified trend(topic).
        /// </summary>
        /// <remarks>See http://open.weibo.com/wiki/2/Trends/statuses </remarks>
        /// <param name="trendName">The name(hot word) of the trend.</param>
        /// <returns>The statuses returned.</returns>
        public Statuses GetTrendStatuses(string trendName)
        {
            ValidateArgument(trendName, "trendName");

            var requester = new OAuthHttpGet(SinaAPIUri.GetTrendStatuses,token);

            requester.Params.Add("trend_name", RFC3986Encoder.UrlEncode(trendName));

            var response = requester.Request();

            var result = JsonSerializationHelper.JsonToObject<Statuses>(response);

            return result;
        }

        /// <summary>
        /// Follows the specified trend(topic).
        /// </summary>
        /// <remarks>
        /// If the current user already followed the trend, error 40028 is returned.
        /// See http://open.weibo.com/wiki/2/Trends/follow </remarks>
        /// <param name="trendName">The name(hot word) of the trend.</param>
        /// <returns>The id of the trend followed.</returns>
        public long FollowTrend(string trendName)
        {
            ValidateArgument(trendName, "trendName");

            var requester = new OAuthHttpPost(SinaAPIUri.FollowTrend,token);

            requester.Params.Add("trend_name", RFC3986Encoder.UrlEncode(trendName));

            var response = requester.Request();

            var match = Regex.Match(response, @"{""topicid"":""(\d+?)""}", RegexOptions.IgnoreCase);

            var result = long.Parse(match.Groups[1].Value);

            return result;
        }

        /// <summary>
        /// Unfollows the specified trend(topic).
        /// </summary>
        /// <remarks>
        /// If the current user already followed the trend, error 40028 is returned.
        /// See http://open.weibo.com/wiki/2/Trends/unfollow </remarks>
        /// <param name="trendID">The id of the trend.</param>
        /// <returns>A boolean value indicating whether the operation succeed.</returns>
        public bool UnfollowTrend(long trendID)
        {
            var requester = new OAuthHttpDelete(SinaAPIUri.UnfollowTrend,token);

            requester.Params.Add("trend_id", trendID.ToString(InvariantCulture));

            var response = requester.Request();

            var match = Regex.Match(response, @"""result"":(\w+)", RegexOptions.IgnoreCase);

            var result = bool.Parse(match.Groups[1].Value);

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Processes async callback response for return-void type methods.
        /// </summary>
        /// <param name="result">The async call result.</param>
        /// <param name="callback">The callback.</param>
        private static void ProcessAsyncCallbackVoidResponse(AsyncCallResult<string> result, VoidAsyncCallback callback)
        {
            var callbackResult = new AsyncCallResult();

            callbackResult.Success = result.Success;
            callbackResult.Exception = result.Exception;

            callback(callbackResult);
        }

        // 摘要:
    //     封装一个具有一个参数并返回 TResult 参数指定的类型值的方法。这是.net 3.5新加的，2.0没有。
    //
    // 参数:
    //   arg:
    //     此委托封装的方法的参数。
    //
    // 类型参数:
    //   T:
    //     此委托封装的方法的参数类型。
    //
    //   TResult:
    //     此委托封装的方法的返回值类型。
    //
    // 返回结果:
    //     此委托封装的方法的返回值。
         public delegate TResult Func<in T, out TResult>(T arg);

        /// <summary>
        /// Processes async callback response by XML de-serialization.
        /// </summary>
        /// <typeparam name="T">The strong type represents the response content.</typeparam>
        /// <param name="result">The async call result.</param>
        /// <param name="preprocess">The func to preprocess the response content before XML de-serialization.</param>
        /// <param name="callback">The callback.</param>
        private static void ProcessAsyncCallbackResponse<T>(AsyncCallResult<string> result, AsyncCallback<T> callback, Func<string, string> preprocess) where T : class
        {
            var callbackResult = new AsyncCallResult<T>();
            try
            {
                callbackResult.Success = result.Success;
                callbackResult.Exception = result.Exception;
                if (result.Success)
                {
                    if (null != preprocess)
                        result.Data = preprocess(result.Data);

                    callbackResult.Data = JsonSerializationHelper.JsonToObject<T>(result.Data);
                }
            }
            catch (InvalidOperationException ioex)
            {
                // Contract mismatch.
                throw new SinaWeiboException(LocalErrorCode.ParseResponseFailed, "Failed to parse server response. Server probably has updated the API contract.", ioex);
            }

            callback(callbackResult);
        }

        /// <summary>
        /// Processes async callback response by customized logic.
        /// </summary>
        /// <typeparam name="T">The strong type represents the response content.</typeparam>
        /// <param name="result">The async call result.</param>
        /// <param name="customProcess">The func to preprocess the response content.</param>
        /// <param name="callback">The callback.</param>
        private static void ProcessAsyncCallbackResponseCustom<T>(AsyncCallResult<string> result, AsyncCallback<T> callback, Func<string, T> customProcess)
        {
            var callbackResult = new AsyncCallResult<T>();
            try
            {
                callbackResult.Success = result.Success;
                callbackResult.Exception = result.Exception;
                if (result.Success)
                {
                    callbackResult.Data = customProcess(result.Data);
                }
            }
            catch (InvalidOperationException ioex)
            {
                // Contract mismatch.
                throw new SinaWeiboException(LocalErrorCode.ParseResponseFailed, "Failed to parse server response. Server probably has updated the API contract.", ioex);
            }

            callback(callbackResult);
        }

        /// <remarks/>
        private static void ConstructPagedRecordsParams(ICollection<ParamPair> additionalParams, long? sinceID = null, long? maxID = null, int? page = null, int? count = null)
        {
            if (sinceID.HasValue)
                additionalParams.Add(new ParamPair("since_id", sinceID.Value.ToString(InvariantCulture)));
            if (maxID.HasValue)
                additionalParams.Add(new ParamPair("max_id", maxID.Value.ToString(InvariantCulture)));
            if (count.HasValue)
                additionalParams.Add(new ParamPair("count", count.Value.ToString(InvariantCulture)));
            if (page.HasValue)
                additionalParams.Add(new ParamPair("page", page.Value.ToString(InvariantCulture)));
        }

        /// <summary>
        /// Validates whether the specified augument is null or not.
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="argName">The argument name.</param>
        private static void ValidateArgument(object arg, string argName)
        {
            if (null == arg || string.IsNullOrEmpty(arg.ToString()))
                throw new SinaWeiboException(LocalErrorCode.ArgumentNotProvided, "Argument '{0}' should not be null.", argName);
        }

        #endregion
    }
}
