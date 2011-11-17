using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;

namespace wojilu.Common.Onlines {


    public class OnlineJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( OnlineJob ) );

        public void Execute() {
            deleteTimeoutVisitor();
            updateMaxOnline();
        }

        public void End() {
        }

        private static void deleteTimeoutVisitor() {

            logger.Info( "---------------deleteTimeoutVisitor---------------" );

            List<OnlineUser> allVisitors = cdb.findAll<OnlineUser>();
            for (int i = 0; i < allVisitors.Count; i++) {

                OnlineUser online = allVisitors[i] as OnlineUser;
                if (online == null) continue;
                TimeSpan span = DateTime.Now.Subtract( online.LastActive );
                try {
                    if (span.TotalMinutes > 20) {

                        online.delete();
                    }
                }
                catch (Exception ex) {
                    logger.Error( "DeleteTimeoutVisitor:" + ex );
                }
            }

            OnlineStats.Instance.ReCount();

        }


        private static void updateMaxOnline() {

            logger.Info( "---------------updateMaxOnline---------------" );


            if (OnlineStats.Instance.Count <= config.Instance.Site.MaxOnline) return;

            config.Instance.Site.MaxOnline = OnlineStats.Instance.Count;
            config.Instance.Site.MaxOnlineTime = DateTime.Now;

            config.Instance.Site.Update( "MaxOnline", OnlineStats.Instance.Count );
            config.Instance.Site.Update( "MaxOnlineTime", DateTime.Now );


        }

    }
}
