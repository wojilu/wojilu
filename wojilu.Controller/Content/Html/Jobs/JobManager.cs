/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class JobManager {

        public static void PostAdd( ContentPost post ) {

            HtmlJobItem item = new HtmlJobItem();
            item.Name = typeof( JobProcessor ).FullName;
            item.Method = getMethodName( new JobProcessor().AfterPostAdd );
            item.PostId = post.Id;

            cdb.insert( item );
        }

        public static void PostDelete( ContentPost post ) {

            HtmlJobItem item = new HtmlJobItem();
            item.Name = typeof( JobProcessor ).FullName;
            item.Method = getMethodName( new JobProcessor().AfterPostDelete );
            item.PostId = post.Id;

            cdb.insert( item );
        }

        public static void PostUpdate( ContentPost post ) {

            HtmlJobItem item = new HtmlJobItem();
            item.Name = typeof( JobProcessor ).FullName;
            item.Method = getMethodName( new JobProcessor().AfterPostUpdate );
            item.PostId = post.Id;

            cdb.insert( item );
        }

        public static void ImportPost( List<int> ids ) {

            if (ids == null || ids.Count == 0) return;

            HtmlJobItem item = new HtmlJobItem();
            item.Name = typeof( JobProcessor ).FullName;
            item.Method = "AfterImport";
            item.Ids = strUtil.GetIds( ids );

            cdb.insert( item );

        }

        private static String getMethodName( aActionWithId action ) {
            return action.Method.Name;
        }

    }

}
