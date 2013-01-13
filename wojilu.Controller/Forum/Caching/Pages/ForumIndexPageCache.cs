using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Common.Caching;

namespace wojilu.Web.Controller.Forum.Caching {

    public class ForumIndexPageCache : CorePageCache {

        public override bool IsCache( MvcContext ctx ) {

            IMember owner = ctx.owner.obj;

            if ((owner is Site)) return true;

            return false;
        }

        public override void ObserveActionCaches() {
            observe( typeof( SiteLayoutCache ) );
            observe( typeof( ForumIndexCache ) );
        }

        public override void UpdateCache( MvcContext ctx ) {

            IMember owner = Site.Instance;

            IApp forum = ctx.app.obj as IApp;

            // 1) ForumIndexCache
            if (forum != null) {

                String forumUrl = alink.ToApp( forum, ctx );
                base.updateAllUrl( forumUrl, ctx );
            }

            // 2) SiteLayoutCache
            else {

                List<ForumApp> siteForums = ForumApp.find( "OwnerId=" + owner.Id + " and OwnerType=:otype" )
                    .set( "otype", owner.GetType().FullName )
                    .list();
                foreach (ForumApp app in siteForums) {

                    String forumUrl = alink.ToApp( app, ctx );
                    base.updateAllUrl( forumUrl, ctx );

                }
            }

        }
    }

}
