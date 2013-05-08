using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller {

    public class ForumInstaller : BaseInstaller {

        public void Init( MvcContext ctx ) {

            ForumApp forum = createFirstForum( ctx );

            base.AddMenuToHome( ctx, alink.ToApp( forum ), "首页" );


        }

        private ForumApp createFirstForum( MvcContext ctx ) {
            throw new NotImplementedException();
        }

    }

}
