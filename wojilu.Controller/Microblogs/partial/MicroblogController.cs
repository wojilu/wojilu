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

            set("user.DisplayName", user.DisplayName);
            set( "user.Name", user.Name );
            set( "user.Pic", user.PicMedium );
            set( "user.PicSmall", user.PicSmall );

            set( "user.PicBig", user.PicBig );

            if (Component.IsEnableUserSpace()) {
                set( "user.Link", lnkFull( Link.ToMember( user ) ) );
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
                block.Set( "tag.Link", Link.T2( Site.Instance, new Users.MainController().Tag, ut.Tag.Id ) );
                block.Next();
            }
        }

        private string getAdminBlogCmd() {
            if (ctx.viewer.IsLogin == false) return "";
            if (ctx.viewer.Id == ctx.owner.Id) return string.Format( "<a href=\"{0}\" id=\"addBlogLink\">��Ҫ��΢��</a>", to( new Microblogs.My.MicroblogController().Home ) );
            return "";
        }

        private String bindCmd( User user ) {

            set( "followUrl", to( Follow ) );
            set( "cancelUrl", to( CancelFollow ) );

            if (ctx.viewer.IsLogin == false) return "<div id=\"lblFollow\"><span>�ӹ�ע</span></div>";
            if (ctx.viewer.Id == user.Id) return "";
            if (ctx.viewer.IsFriend( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">���Ǻ���</span></div>";

            if (ctx.viewer.IsFollowing( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">�ѹ�ע</span><span id=\"cancelFollow\">ȡ����ע</span></div>";

            return "<div id=\"cmdFollow\"><span>�ӹ�ע</span></div>";
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


        private void bindComments( IBlock cblock, List<MicroblogComment> clist ) {

            foreach (MicroblogComment c in clist) {

                cblock.Set( "user.Face", c.User.PicSmall );
                cblock.Set( "user.Link", Link.ToMember( c.User ) );
                cblock.Set( "user.Name", c.User.Name );
                cblock.Set( "comment.Id", c.Id );
                cblock.Set( "comment.RootId", c.Root.Id );
                cblock.Set( "comment.Content", c.Content );
                cblock.Set( "comment.Created", c.Created );
                cblock.Set( "comment.Indent", 10 );
                cblock.Set( "comment.ReplyUrl", to( new MicroblogCommentsController().Reply, c.Root.Id ) + "?parentId=" + c.Id );

                String deleteCmd = "";
                if (ctx.viewer.Id == ctx.owner.Id || ctx.viewer.IsAdministrator()) {
                    deleteCmd = string.Format( "<a href=\"{0}\" class=\"ajaxDeleteCmd\" removeId=\"commentItem{1}\">ɾ��</a>",
                        to( new My.MicroblogCommentsController().Delete, c.Id ),
                        c.Id );
                }
                cblock.Set( "comment.DeleteCmd", deleteCmd );

                cblock.Next();
            }

            if (clist.Count > 0) {
                ctx.SetItem( "lastComment", clist[clist.Count - 1] );
            }
        }

        //--------------------------------------------------------------------------------------------------

        private void bindOne( IBlock block, Microblog blog, Boolean isFavorite, Boolean showUserFace ) {


            block.Set( "blog.Id", blog.Id );
            block.Set( "blog.Created", blog.Created );

            block.Set( "blog.ShowLink", Link.To( blog.User, Show, blog.Id ) );

            block.Set( "blog.Content", getBlogContent( blog ) );

            bindUserInfo( block, blog, showUserFace ); // �û���Ϣ
            bindRepost( block, blog ); // ת����Ϣ
            bindPicInfo( block, blog ); // ͼƬ��Ϣ
            bindVideoInfo( block, blog ); // ��Ƶ��Ϣ

            // ������
            String replies = blog.Replies > 0 ? "(" + blog.Replies + ")" : "";
            block.Set( "blog.Replies", replies );

            // ת����
            String reposts = blog.Reposts > 0 ? "(" + blog.Reposts + ")" : "";
            block.Set( "blog.Reposts", reposts );

            block.Set( "blog.CommentsLink", to( new MicroblogCommentsController().Show, blog.Id ) );
            block.Set( "blog.ForwardUrl", to( Forward, blog.Id ) );
            block.Set( "blog.FavoriteCmd", getFavoriteCmd( blog, isFavorite ) ); // �ղ�����

            // ɾ������
            String deleteCmd = "";
            if (ctx.viewer.Id == blog.User.Id) {
                deleteCmd = string.Format( "<a href=\"{0}\" class=\"left10 ajaxDeleteCmd\" removeId=\"mblog{1}\">ɾ��</a>", to( new Microblogs.My.MicroblogController().Delete, blog.Id ), blog.Id );
            }
            block.Set( "blog.DeleteCmd", deleteCmd );
        }

        private void bindRepost( IBlock block, Microblog blog ) {
            // ת��������
            if (blog.ParentId > 0) {

                Microblog parent = microblogService.GetById( blog.ParentId );
                if (parent == null) {
                    block.Set( "blog.ForwardContent", "<div class=\"mblogSingle\">��΢���ѱ�ԭ����ɾ��</div>" );
                }
                else {
                    block.Set( "blog.ForwardContent", loadHtml( Single, blog.ParentId ) );
                }
            }
            else {
                block.Set( "blog.ForwardContent", "" );
            }
        }

        private static void bindUserInfo( IBlock block, Microblog blog, Boolean showUserFace ) {
            IBlock ufBlock = block.GetBlock( "userFace" );
            if (showUserFace) {
                ufBlock.Set("blog.DisplayName", blog.User.DisplayName);
                ufBlock.Set( "blog.UserName", blog.User.Name );
                ufBlock.Set( "blog.UserFace", blog.User.PicSmall );
                ufBlock.Set( "blog.UserLink", alink.ToUserMicroblog( blog.User ) );

                ufBlock.Next();

                block.Set( "userNameInfo", string.Format( "<a href=\"{0}\">{1}</a>: ", alink.ToUserMicroblog( blog.User ), blog.User.DisplayName ) );
            }
            else {
                block.Set( "userNameInfo", "" );
            }

        }

        private static void bindPicInfo( IBlock block, Microblog blog ) {
            IBlock picBlock = block.GetBlock( "pic" );
            if (strUtil.HasText( blog.Pic )) {
                picBlock.Set( "blog.PicSmall", blog.PicSmall );
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


        // ת���ĸ���
        [NonVisit]
        public void Single( int id ) {

            Microblog blog = microblogService.GetById( id );

            set( "blog.UserName", blog.User.Name );
            set( "blog.UserLink", Link.ToMember( blog.User ) );
            set( "blog.Content", blog.Content );

            set( "blog.ShowLink", Link.To( blog.User, new MicroblogController().Show, id ) );

            set( "blog.Replies", blog.Replies );
            set( "blog.Reposts", blog.Reposts );

            bindPicInfo( this.utils.getCurrentView(), blog );
            bindVideoInfo( this.utils.getCurrentView(), blog );

        }

        private string getBlogContent( Microblog blog ) {
            if (blog.ParentId <= 0) return blog.Content;

            String content = strUtil.HasText( blog.Content ) ? blog.Content : "ת��΢��";

            return content;
        }

        private string getFavoriteCmd( Microblog blog, Boolean isFavorite ) {

            if (isFavorite) {
                return string.Format( "<span class=\"cancelFavorite link left10 right10\" to=\"{0}\">ȡ���ղ�</span>", to( CancelFavorite, blog.Id ) );
            }

            return string.Format( "<span class=\"addFavorite link left10 right10\" to=\"{0}\">�ղ�</span>", to( SaveFavorite, blog.Id ) );
        }


        private void bindUsers( List<User> users, String blockName ) {

            IBlock block = getBlock( blockName );
            foreach (User user in users) {
                block.Set("user.DisplayName", user.DisplayName);
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", alink.ToUserMicroblog( user ) );
                block.Next();
            }
        }

    }

}
