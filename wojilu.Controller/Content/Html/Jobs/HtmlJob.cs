/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;
using wojilu.DI;
using wojilu.Data;

namespace wojilu.Web.Controller.Content.Htmls {

    /// <summary>
    /// 自动生成html静态页面
    /// </summary>
    public class HtmlJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlJob ) );

        public void Execute() {

            
            List<HtmlJobItem> jobs = cdb.findAll<HtmlJobItem>();
            logger.Info( "begin HtmlJob=" + jobs.Count );
            
            foreach (HtmlJobItem x in jobs) {
                runJobSingle( x );
            }
        }

        private void runJobSingle( HtmlJobItem x ) {

            try {

                Object p = ObjectContext.CreateObject( x.Name );

                if (strUtil.HasText( x.Ids )) {
                    rft.CallMethod( p, x.Method, new object[] { x.Ids } );
                    cdb.delete( x );
                }
                else if (x.PostId > 0) {
                    rft.CallMethod( p, x.Method, new object[] { x.PostId } );
                    cdb.delete( x );
                }
                else {
                    logger.Info( "param is invalid. type=" + x.Name + ", method=" + x.Method );
                }


            }

            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
            }
        }

        public void End() {
            DbContext.closeConnectionAll();
        }
    }
}
