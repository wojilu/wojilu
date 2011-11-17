using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Microblogs.My {

    public class MicroblogFavoriteController : ControllerBase {

        public MicroblogFavoriteService favoriteService { get; set; }
        public MicroblogService blogService { get; set; }
        public MicroblogCommentService commentService { get; set; }

        public MicroblogFavoriteController() {
            favoriteService = new MicroblogFavoriteService();
            blogService = new MicroblogService();
            commentService = new MicroblogCommentService();
            LayoutControllerType = typeof( MicroblogController );
        }

        public void List() {

            load( "publisher", new MicroblogController().Publisher );

            DataPage<Microblog> list = favoriteService.GetBlogPage( ctx.owner.Id, 25 );


            List<MicroblogVo> volist = favoriteService.CheckFavorite( list.Results, ctx.viewer.Id );
            ctx.SetItem( "_microblogVoList", volist );
            ctx.SetItem( "_showUserFace", true );
            load( "flist", new wojilu.Web.Controller.Microblogs.MicroblogController().bindBlogs );

            set( "page", list.PageBar );

        }


    }

}
