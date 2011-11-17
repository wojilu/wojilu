using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Layouts;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using System.Collections;
using wojilu.Common.Menus;
using wojilu.Common.Microblogs.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Tags;

namespace wojilu.Web.Controller.Microblogs {

    public class MainController : ControllerBase {

        public ISiteSkinService siteSkinService { get; set; }

        public MainController() {

            siteSkinService = new SiteSkinService();
            HideLayout( typeof( wojilu.Web.Controller.Microblogs.LayoutController ) );
        }

        public override void Layout() {

            set( "siteName", config.Instance.Site.SiteName );

            set( "microblogHomeLink", getFullUrl( alink.ToMicroblog() ) );
            set( "regLink", to( new RegisterController().Register ) );

            bindLoginForm();
            bindLoginUser();

            List<Tag> tags = TagService.GetHotTags( 25 );
            IBlock block = getBlock( "tags" );
            foreach (Tag tag in tags) {
                block.Set( "tag.Name", tag.Name );
                block.Set( "tag.Link", alink.ToTag( tag.Name ) );
                block.Next();
            }

        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private void bindLoginForm() {


            set( "ActionLink", to( new wojilu.Web.Controller.MainController().CheckLogin ) );
            set( "resetPwdLink", to( new Common.ResetPwdController().StepOne ) );
            //String returnUrl = strUtil.HasText( ctx.web.PathReferrer ) ? ctx.web.PathReferrer : to( Index );
            set( "returnUrl", to( Index ) );

        }

        private void bindLoginUser() {


            set( "mvcUrlExt", MvcConfig.Instance.UrlExt );


        }

        public void Index() {

            load( "topNav", new wojilu.Web.Controller.Layouts.TopNavController().Index );

            List<Microblog> list = Microblog.find( "order by Id desc" ).list( 20 );
            ctx.SetItem( "_microblogList", list );
            load( "recentList", List );

            List<User> follows = User.find( "order by FollowersCount desc, Id desc" ).list( 9 );
            bindFollowers( follows, "followers" );

            List<User> recents = User.find( "Pic<>'' order by Id desc" ).list( 9 );
            bindFollowers( recents, "recent" );

        }

        [NonVisit]
        public void List() {

            List<Microblog> list = ctx.GetItem( "_microblogList" ) as List<Microblog>;

            IBlock block = getBlock( "list" );
            foreach (Microblog m in list) {

                block.Set( "m.UserName", m.User.Name );
                block.Set( "m.UserFace", m.User.PicSmall );
                block.Set( "m.UserLink", alink.ToUserMicroblog( m.User ) );
                block.Set( "m.Content", m.Content );
                block.Set( "m.Created", m.Created );

                IBlock picBlock = block.GetBlock( "pic" );
                if (strUtil.HasText( m.Pic )) {
                    picBlock.Set( "blog.PicSmall", m.PicSmall );
                    picBlock.Set( "blog.PicMedium", m.PicMedium );
                    picBlock.Set( "blog.PicOriginal", m.PicOriginal );
                    picBlock.Next();
                }


                block.Next();
            }


        }

        private void bindFollowers( List<User> follows, String blockName ) {
            IBlock block = getBlock( blockName );
            foreach (User u in follows) {

                block.Set( "user.Name", u.Name );
                block.Set( "user.Face", u.PicSmall );
                block.Set( "user.Link", alink.ToUserMicroblog( u ) );
                block.Next();
            }

        }


    }

}
