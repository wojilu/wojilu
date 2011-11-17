using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Web;
using wojilu.Web.Mvc.Routes;
using wojilu.Members.Interface;
using System.Web;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Common.Caching {

    public class MockContext {

        public static MvcContext GetOne( IMember objOwner, Type appType, int appId ) {

            MvcContext ctx = getContextInit();

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
            app.obj = ndb.findById( appType, appId );
            app.setAppType( appType ); // 如果要使用alang语言包，必须设置此项
            ctx.utils.setAppContext( app );

            return ctx;
        }

        public static MvcContext GetOne( IMember objOwner ) {


            MvcContext ctx = getContextInit();

            // route
            Route route = new wojilu.Web.Mvc.Routes.Route();
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
            app.obj = null;
            ctx.utils.setAppContext( app );

            return ctx;
        }

        private static MvcContext getContextInit() {

            String urlRoot = "http://" + SystemInfo.Authority;


            IWebContext webContext = MockWebContext.New( 1, urlRoot, new System.IO.StringWriter() );

            MvcContext ctx = new MvcContext( webContext );
            return ctx;
        }



    }

}
