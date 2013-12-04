/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Domain;
using wojilu.Web.Mvc.Attr;
using System.Text;
using wojilu.Members.Sites.Domain;
using wojilu.Common;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Microblogs {

    public partial class MicroblogController : ControllerBase {

        private String lnkFull( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private void bindUserInfo( User user ) {

            set( "siteName", config.Instance.Site.SiteName );

            set( "microblogHomeLink", lnkFull( alink.ToMicroblog() ) );
            set( "favoriteLink", to( new My.MicroblogFavoriteController().List ) );

            set( "homeLink", to( new My.MicroblogController().Home ) );
            set( "atmeLink", to( new My.MicroblogController().Atme ) );

            set( "myCommentLink", to( new My.MicroblogCommentsController().My ) );

            set( "user.Name", user.Name );
            set( "user.Pic", user.PicSX );
            set( "user.PicSmall", user.PicSmall );

            set( "user.PicBig", user.PicM );

            if (Component.IsEnableUserSpace()) {
                set( "user.Link", lnkFull( toUser( user ) ) );
                set( "userLinkStyle", "" );
            }
            else {
                set( "user.Link", "" );
                set( "userLinkStyle", "display:none" );
            }

            set( "user.MLink", lnkFull( alink.ToUserMicroblog( user ) ) );
            set( "user.Signature", user.Signature );
            set( "user.Description", user.Profile.Description );

            String adminBlogCmd = getAdminBlogCmd();
            set( "adminBlogCmd", adminBlogCmd );

            int microblogCount = microblogService.CountByUser( user.Id );
            set( "user.MicroblogCount", microblogCount );

            bindStats( user );
        }

        private void bindUserTags() {
            IBlock block = getBlock( "tags" );
            List<UserTagShip> list = userTagService.GetPage( ctx.owner.Id );
            foreach (UserTagShip ut in list) {
                block.Set( "tag.Name", ut.Tag.Name );
                block.Set( "tag.Link", Link.To( Site.Instance, new Users.MainController().Tag, ut.Tag.Id ) );
                block.Next();
            }
        }

        private string getAdminBlogCmd() {
            if (ctx.viewer.IsLogin == false) return "";
            if (ctx.viewer.Id == ctx.owner.Id) return string.Format( "<a href=\"{0}\" id=\"addBlogLink\">我要发微博</a>", to( new Microblogs.My.MicroblogController().Home ) );
            return "";
        }

        private String bindCmd( User user ) {

            set( "followUrl", to( Follow ) );
            set( "cancelUrl", to( CancelFollow ) );

            if (ctx.viewer.IsLogin == false) return "<div id=\"lblFollow\"><span>加关注</span></div>";
            if (ctx.viewer.Id == user.Id) return "";
            if (ctx.viewer.IsFriend( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">已是好友</span></div>";

            if (ctx.viewer.IsFollowing( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">已关注</span><span id=\"cancelFollow\">取消关注</span></div>";

            return "<div id=\"cmdFollow\"><span>加关注</span></div>";
        }

        private void bindStats( User user ) {
            set( "user.FollowingCount", (user.FriendCount + user.FollowingCount) );
            set( "user.FollowersCount", (user.FollowersCount + user.FriendCount) );

            set( "user.FollowingLink", t2( new Microblogs.FriendController().FollowingList ) );
            set( "user.FollowerLink", t2( new Microblogs.FriendController().FollowerList ) );
        }


        private void loadCommonView( Microblog blog ) {

            List<Microblog> list = new List<Microblog>();
            list.Add( blog );
            List<MicroblogVo> volist = mfService.CheckFavorite( list, ctx.viewer.Id );

            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", false );
            load( "blogList", bindBlogs );
        }

        //--------------------------------------------------------------------------------------------------

        private void bindOne( IBlock block, Microblog blog, Boolean isFavorite, Boolean showUserFace ) {


            block.Set( "blog.Id", blog.Id );
            block.Set( "blog.Created", blog.Created );

            if (ctx.GetItemString( "_showType" ) == "microblog") {
                block.Set( "blog.ShowLink", MbLink.ToShowMicroblog( blog.User, blog.Id ) );
            }
            else {
                block.Set( "blog.ShowLink", MbLink.ToShowFeed( blog.User, blog.Id ) );
            }

            block.Set( "blog.Content", getBlogContent( blog ) );

            bindUserInfo( block, ctx, blog, showUserFace ); // 用户信息
            bindRepost( block, blog ); // 转发信息
            bindPicInfo( block, blog ); // 图片信息
            bindVideoInfo( block, blog ); // 视频信息

            // 评论数
            block.Set( "blog.StrReplies", blog.Replies == 0 ? "" : string.Format( "<span class=\"feed-replies\">(<span class=\"feed-replies-num\" id=\"renum{1}\">{0}</span>)</span>", blog.Replies, blog.Id ) );

            block.Set( "blog.StrLikes", blog.Likes == 0 ? "" : string.Format( "<span class=\"feed-likes\">(<span class=\"feed-likes-num\">{0}</span>)</span>", blog.Likes ) );
            block.Set( "blog.SaveLikeLink", to( SaveLike, blog.Id ) );


            // 转发数
            String reposts = blog.Reposts > 0 ? "(" + blog.Reposts + ")" : "";
            block.Set( "blog.Reposts", reposts );

            block.Set( "blog.CommentsLink", getCommentUrl( blog ) );
            block.Set( "blog.ForwardUrl", to( Forward, blog.Id ) );
            block.Set( "blog.FavoriteCmd", getFavoriteCmd( blog, isFavorite ) ); // 收藏命令

            // 删除命令
            String deleteCmd = getDeleteCmd( ctx, blog );
            block.Set( "blog.DeleteCmd", deleteCmd );
        }

        private static String getDeleteCmd( MvcContext ctx, Microblog blog ) {
            String deleteCmd = "";
            if (ctx.viewer.Id == blog.User.Id || ctx.viewer.IsAdministrator()) {
                deleteCmd = string.Format( "<a href=\"{0}\" class=\"left10 ajaxDeleteCmd\" removeId=\"mblog{1}\">x</a>", Link.To( new Microblogs.MicroblogSaveController().Delete, blog.Id ), blog.Id );
            }
            return deleteCmd;
        }

        private string getCommentUrl( Microblog x ) {

            String dataType = strUtil.IsNullOrEmpty( x.DataType ) ? typeof( Microblog ).FullName : x.DataType;
            long dataId = getDataId( x );

            return t2( new wojilu.Web.Controller.Open.CommentController().List )
                + "?dataType=" + dataType
                + "&ownerId=" + x.User.Id
                + "&dataId=" + dataId
                + "&dataTitle=(微博" + x.Created.ToShortDateString() + ")" + strUtil.ParseHtml( x.Content, 25 )
                + "&url=" + MbLink.ToShow( x.User, x.Id )
                + "&dataUserId=" + x.Creator.Id
                + "&feedId=" + x.Id;
        }

        private long getDataId( Microblog x ) {

            if (x.DataType != null && x.DataType.Equals( typeof( Microblog ).FullName )) {
                return x.Id;
            }

            if (x.DataId <= 0) return x.Id;

            return x.DataId;
        }

        private void bindRepost( IBlock block, Microblog blog ) {
            // 转发的内容
            if (blog.ParentId > 0) {

                Microblog parent = microblogService.GetById( blog.ParentId );
                if (parent == null) {
                    block.Set( "blog.ForwardContent", "<div class=\"mblogSingle\">此微博已被原作者删除</div>" );
                }
                else {
                    block.Set( "blog.ForwardContent", loadHtml( Single, blog.ParentId ) );
                }
            }
            else {
                block.Set( "blog.ForwardContent", "" );
            }
        }

        private static void bindUserInfo( IBlock block, MvcContext ctx, Microblog blog, Boolean showUserFace ) {
            IBlock ufBlock = block.GetBlock( "userFace" );
            if (showUserFace) {

                ufBlock.Set( "blog.UserName", blog.User.Name );
                ufBlock.Set( "blog.UserFace", blog.User.PicSmall );

                if (ctx.GetItemString( "_showType" ) == "microblog") {
                    ufBlock.Set( "blog.UserLink", alink.ToUserMicroblog( blog.User ) );
                    ufBlock.Set( "userNameInfo", string.Format( "<a href=\"{0}\">{1}</a>", alink.ToUserMicroblog( blog.User ), blog.User.Name ) );
                }
                else {
                    ufBlock.Set( "blog.UserLink", Link.ToMember( blog.User ) );
                    ufBlock.Set( "userNameInfo", string.Format( "<a href=\"{0}\">{1}</a>", Link.ToMember( blog.User ), blog.User.Name ) );
                }

                String deleteCmd = getDeleteCmd( ctx, blog );
                ufBlock.Set( "blog.DeleteCmd", deleteCmd );

                ufBlock.Next();

                if (ctx.GetItemString( "_showType" ) == "microblog") {
                    block.Set( "userNameInfo", string.Format( "<a href=\"{0}\">{1}</a>", alink.ToUserMicroblog( blog.User ), blog.User.Name ) );
                }
                else {
                    block.Set( "userNameInfo", string.Format( "<a href=\"{0}\">{1}</a>", Link.ToMember( blog.User ), blog.User.Name ) );
                }


            }
            else {
                block.Set( "userNameInfo", "" );
            }

        }

        private static void bindPicInfo( IBlock block, Microblog blog ) {
            IBlock picBlock = block.GetBlock( "pic" );
            if (strUtil.HasText( blog.Pic )) {
                picBlock.Set( "blog.PicSmall", blog.PicSx );
                picBlock.Set( "blog.PicMedium", blog.PicMedium );
                picBlock.Set( "blog.PicOriginal", blog.PicOriginal );
                picBlock.Next();
            }

        }

        private static void bindVideoInfo( IBlock block, Microblog blog ) {
            IBlock vBlock = block.GetBlock( "video" );
            if (strUtil.HasText( blog.FlashUrl )) {

                String vpic = strUtil.HasText( blog.PicUrl ) ? blog.PicUrl : strUtil.Join( sys.Path.Img, "/big/novideopic.png" );

                vBlock.Set( "blog.FlashPic", vpic );
                vBlock.Set( "blog.Flash", wojilu.Web.Utils.WebHelper.GetFlash( blog.FlashUrl, 450, 340 ) );
                vBlock.Set( "blog.FlashPageUrl", blog.PageUrl );
                vBlock.Next();
            }

        }


        //--------------------------------------------------------------------------------------------------


        // 转发的附件
        [NonVisit]
        public virtual void Single( long id ) {

            Microblog blog = microblogService.GetById( id );

            set( "blog.UserName", blog.User.Name );
            set( "blog.UserLink", toUser( blog.User ) );
            set( "blog.Content", blog.Content );

            if (ctx.GetItemString( "_showType" ) == "microblog") {
                set( "blog.ShowLink", MbLink.ToShowMicroblog( blog.User, blog.Id ) );
            }
            else {
                set( "blog.ShowLink", MbLink.ToShowFeed( blog.User, blog.Id ) );
            }

            set( "blog.Replies", blog.Replies );
            set( "blog.Reposts", blog.Reposts );

            bindPicInfo( this.utils.getCurrentView(), blog );
            bindVideoInfo( this.utils.getCurrentView(), blog );

        }

        private string getBlogContent( Microblog blog ) {
            if (blog.ParentId <= 0) return blog.Content;

            String content = strUtil.HasText( blog.Content ) ? blog.Content : "转发微博";

            return content;
        }

        private string getFavoriteCmd( Microblog blog, Boolean isFavorite ) {

            if (isFavorite) {
                return string.Format( "<span class=\"cancelFavorite link left10 right10\" to=\"{0}\">取消收藏</span>", to( CancelFavorite, blog.Id ) );
            }

            return string.Format( "<span class=\"addFavorite link left10 right10\" to=\"{0}\">收藏</span>", to( SaveFavorite, blog.Id ) );
        }


        private void bindUsers( List<User> users, String blockName ) {

            IBlock block = getBlock( blockName );
            foreach (User user in users) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", alink.ToUserMicroblog( user ) );
                block.Next();
            }
        }

    }

}
