/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class LogCacher {

        private static Hashtable spiderLogs = Hashtable.Synchronized( new Hashtable() );
        private static Hashtable importLogs = Hashtable.Synchronized( new Hashtable() );

        public static StringBuilder GetNewSpiderLog( String key ) {

            StringBuilder sb = new StringBuilder();
            spiderLogs[key] = sb;
            return sb;
        }

        public static StringBuilder GetSpiderLog( String key ) {

            if (spiderLogs.Contains( key )) return spiderLogs[key] as StringBuilder;
            return new StringBuilder();
        }

        public static StringBuilder GetNewImportLog( String key ) {

            StringBuilder sb = new StringBuilder();
            importLogs[key] = sb;
            return sb;
        }

        public static StringBuilder GetImportLog( String key ) {

            if (importLogs.Contains( key )) return importLogs[key] as StringBuilder;
            return new StringBuilder();
        }

    }

}
