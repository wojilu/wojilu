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

    /// <summary>
    /// 使用说明
    /// HitsJob.Add( objBlogPost );
    /// </summary>
    public class HitsJob  {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HitsJob ) );

        public static void Add( IHits objTarget ) {

            HitsItem cachedItem = findFromCache( objTarget );

            if (cachedItem != null) {
                cachedItem.Target.Hits = cachedItem.Target.Hits + 1;
                cachedItem.IsUpdated = false;
                cachedItem.Updated = DateTime.Now;
                logger.Debug( "updateHits=>" + cachedItem.Name );
            }
            else {
                cachedItem = new HitsItem();
                cachedItem.Name = getName( objTarget );
                cachedItem.Target = objTarget;
                cachedItem.Target.Hits = cachedItem.Target.Hits + 1;
                cachedItem.Updated = DateTime.Now;
                cachedItem.insertByIndex( "Name", cachedItem.Name );

                logger.Debug( "addHits=>" + cachedItem.Name );

            }
        }

        private static HitsItem findFromCache( IHits objTarget ) {
            List<HitsItem> results = cdb.findByName<HitsItem>( getName( objTarget ) );
            if (results.Count == 0) return null;
            return results[0];
        }

        private static String getName( IHits objTarget ) {
            return objTarget.GetType().FullName + objTarget.Id;
        }



    }
}
