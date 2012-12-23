using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class PostUpdateObserver: ActionObserver {



        private static readonly ILog logger = LogManager.GetLogger( typeof( PostUpdateObserver ) );

        public override void ObserveActions() {

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();

            observe( post.Restore );
            observe( post.Update );
            observe( post.UpdateTitleStyle );

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Update );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Update );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Update );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.CreateListInfo );
            observe( img.CreateImgList );
            observe( img.SetLogo );
            observe( img.UpdateListInfo );
            observe( img.DeleteImg );


        }

    }

}
