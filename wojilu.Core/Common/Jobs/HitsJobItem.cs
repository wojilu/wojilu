/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Web.Jobs;

namespace wojilu.Common.Jobs {

    public class HitsJobItem : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HitsJobItem ) );

        public void Execute() {

            List<HitsItem> list = cdb.findAll<HitsItem>();

            logger.Debug( "hits job count:" + list.Count );

            foreach (HitsItem obj in list) {

                if (obj.IsUpdated) continue;

                IEntity hits = obj.Target as IEntity;
                if (hits == null) continue;
                db.update( hits, "Hits" );
                logger.Debug( "savedb hits=>" + hits.GetType().FullName + hits.Id );

                // 5分钟前的项目清除掉
                if (DateTime.Now.Subtract( obj.Updated ).Minutes > 5) {
                    obj.delete();
                }
                else {
                    obj.IsUpdated = true;
                    obj.Updated = DateTime.Now;
                }

            }
        }

        public void End() {
            DbContext.closeConnectionAll();
        }

    }
}
