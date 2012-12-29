/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.Web.Jobs;
using wojilu.Common.Spider.Domain;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class ImportJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ImportJob ) );

        public void Execute() {

            List<SpiderImport> items = SpiderImport.find( "IsDelete=0" ).list();
            DbContext.closeConnectionAll();
            logger.Info( "begin ImportJob=" + items.Count );

            StringBuilder log = new StringBuilder();
            foreach (SpiderImport item in items) {

                ImportState ts = new ImportState();
                ts.TemplateId = item.Id;
                ts.Log = log;

                ImportUtil.BeginImport( ts );

                DbContext.closeConnectionAll();

            }

        }

        public void End() {
            DbContext.closeConnectionAll();
        }

    }

}
