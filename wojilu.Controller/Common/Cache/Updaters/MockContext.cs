using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Members.Users.Domain;
using wojilu.Web;
using wojilu.Web.Mvc.Routes;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Interface;

namespace wojilu.Apps.Blog.Caching {

    public class MockContext {

        public static MvcContext GetOne( IMember objOwner, int appId ) {


            IWebContext webContext = MockWebContext.New( 1, "http://localhost/", new System.IO.StringWriter() );

            MvcContext ctx = new MvcContext( webContext );

            // route
            Route route = new wojilu.Web.Mvc.Routes.Route();
            route.setAppId( appId ); // 为了让生成的link链接中有appId，必须设置此项
            ctx.utils.setRoute( route );

            // viewer: 某些地方需要判断viewer
            ViewerContext viewer = new ViewerContext();
            viewer.obj = new User();
            ctx.utils.setViewerContext( viewer );

            // owner
            OwnerContext owner = new OwnerContext();
            owner.Id = objOwner.Id;
            owner.obj = objOwner;
            ctx.utils.setOwnerContext( owner );

            // app
            IAppContext app = new AppContext();
            app.Id = appId;
            app.obj = BlogApp.findById( appId );
            app.setAppType( typeof( BlogApp ) ); // 如果要使用alang语言包，必须设置此项
            ctx.utils.setAppContext( app );

            return ctx;
        }


    }

}
