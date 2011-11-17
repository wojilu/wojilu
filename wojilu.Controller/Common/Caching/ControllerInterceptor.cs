using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Web.Controller.Common.Admin;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Admin;
using wojilu.Web.Controller.Admin.Sys;
using wojilu.Members.Interface;
using wojilu.Caching;
using wojilu.Web.Mvc.Utils;

namespace wojilu.Web.Controller.Common.Caching {

    public class ControllerInterceptor : IMvcFilter {


        public void Process( MvcEventPublisher publisher ) {

            publisher.End_ProcessAction += new EventHandler<MvcEventArgs>( publisher_End_ProcessAction );

        }

        void publisher_End_ProcessAction( object sender, MvcEventArgs e ) {

            MvcContext ctx = e.ctx;
            if (ctx.HttpMethod.Equals( "GET" )) return;

            if (!(ctx.owner.obj is Site)) return;



            List<ICacheFilter> dbs = CacheFilterDB.Instance.Renews;
            foreach (ICacheFilter cr in dbs) {

                if (actionSame( cr, ctx )) {
                    cr.Update( ctx );
                    break;
                }

            }

        }

        private bool actionSame( ICacheFilter cr, MvcContext ctx ) {

            Dictionary<String, String> actions = cr.Actions;

            foreach (KeyValuePair<String, String> kv in actions) {

                if (ctx.controller.GetType().FullName.Equals( kv.Key ) == false &&
                    ctx.controller.GetType().BaseType.FullName.Equals( kv.Key ) == false
                    ) continue;

                if (actionSame( kv.Value, ctx )) return true;
            }

            return false;
        }

        private bool actionSame( String strActions, MvcContext ctx ) {
            String[] arrActions = strActions.Split( '/' );
            foreach (String action in arrActions) {
                if (action.Equals( ctx.route.action )) return true;
            }
            return false;
        }


    }





}
