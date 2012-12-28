/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Htmls {

    public class HtmlJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlJob ) );

        public void Execute() {

            List<HtmlJobItem> jobs = cdb.findAll<HtmlJobItem>();
            foreach (HtmlJobItem x in jobs) {
                runJobSingle( x );
            }
        }

        private void runJobSingle( HtmlJobItem x ) {

            try {

                Object p = ObjectContext.CreateObject( x.Name );
                rft.CallMethod( p, x.Method, new object[] { x.PostId } );

                cdb.delete( x );

            }

            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
            }
        }

        public void End() {
        }
    }
}
