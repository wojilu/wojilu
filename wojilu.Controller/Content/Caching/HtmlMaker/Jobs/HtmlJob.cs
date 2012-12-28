using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Caching {

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

                Type t = ObjectContext.GetType( x.Name );

                Object p = ObjectContext.CreateObject( t );
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
