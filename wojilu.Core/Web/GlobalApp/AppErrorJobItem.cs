/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using wojilu.Web.Jobs;
using wojilu.Net;
using wojilu.Web.Utils;

namespace wojilu.Web.GlobalApp {

    public class AppErrorJobItem : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AppErrorJobItem ) );
        private static Random rd = new Random();

        public void Execute() {

            List<AppErrorItem> jobs = cdb.findAll<AppErrorItem>();
            foreach (AppErrorItem job in jobs) {
                sendError( job );
                int sleepSecond = rd.Next( 10, 60 );
                Thread.Sleep( sleepSecond * 1000 );
            }

        }

        private void sendError( AppErrorItem job ) {

            logger.Info( "begin send email" );

            String email = config.Instance.Site.Email;
            MailClient mail = MailClient.Init();
            mail.Send( email, job.Title, job.ErrorHtml );

            cdb.delete( job );
        }

        public void End() {
        }

    }
}
