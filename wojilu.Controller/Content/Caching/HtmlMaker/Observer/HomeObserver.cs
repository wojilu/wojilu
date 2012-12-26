/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class HomeObserver : ActionObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HomeObserver ) );

        public override void ObserveActions() {

            Dictionary<Type, String> dic = new ContentIndexCache().GetRelatedActions();
            foreach (KeyValuePair<Type, String> kv in dic) {
                this.observe( kv.Key, kv.Value );
            }

        }

        public override void AfterAction( Context.MvcContext ctx ) {
            new HomeMaker( ctx ).Process( ctx.app.Id );
            logger.Info( "HomeObserver make app home" );
        }


    }

}
