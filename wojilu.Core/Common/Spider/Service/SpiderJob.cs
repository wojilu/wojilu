using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;
using wojilu.Common.Spider.Domain;
using wojilu.Data;
using System.Threading;
using wojilu.DI;
using wojilu.Common.Spider.Interface;

namespace wojilu.Common.Spider.Service {

    public class SpiderJob : IWebJobItem {

        private static Random rd = new Random();

        private static readonly ILog logger = LogManager.GetLogger( typeof( SpiderJob ) );

        public ISpiderTool defaultSpider { get; set; }

        public SpiderJob() {
            defaultSpider = new SpiderTool();
        }

        public void Execute() {

            List<SpiderTemplate> list = SpiderTemplate.find( "IsDelete=0" ).list();
            DbContext.closeConnectionAll();

            logger.Info( "begin SpiderJob=" + list.Count );

            StringBuilder log = new StringBuilder();
            foreach (SpiderTemplate s in list) {

                ISpiderTool spider = getSpider( s );

                spider.DownloadPage( s, log, new int[] { SpiderConfig.SuspendFrom, SpiderConfig.SuspendTo } ); // 2~6秒暂停
                DbContext.closeConnectionAll();

                int sleepms = rd.Next( SpiderConfig.SuspendFrom, SpiderConfig.SuspendTo );
                Thread.Sleep( sleepms );
            }

            String[] arrLog = log.ToString().Split( '\n' );
            StringBuilder errorLog = new StringBuilder();
            foreach (String item in arrLog) {
                if (item.Trim().StartsWith( "error=" )) errorLog.AppendLine( item.Trim() );
            }

            SpiderLog sg = new SpiderLog();
            sg.Msg = errorLog.ToString();
            sg.insert();
            DbContext.closeConnectionAll();

        }

        private ISpiderTool getSpider( SpiderTemplate s ) {

            if (strUtil.IsNullOrEmpty( s.SpiderType )) return defaultSpider;

            ISpiderTool spider = ObjectContext.GetByType( s.SpiderType ) as ISpiderTool;
            if (spider == null) return defaultSpider;

            return spider;
        }

        public void End() {
            DbContext.closeConnectionAll();
        }

    }

}
