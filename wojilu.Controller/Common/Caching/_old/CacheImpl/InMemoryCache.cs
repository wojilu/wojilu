using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;

namespace wojilu.Web.Controller.Common {

    public class InMemoryCache : ICacheHelper {

        private ApplicationCache mcache = new ApplicationCache();

        public string ReadCache( string key ) {
            Object obj = mcache.Get( key );
            if (obj == null) return null;
            return obj.ToString();
        }

        public void AddCache( string key, string val ) {
            mcache.Put( key, val );
        }

        public void DeleteCache( string key ) {
            mcache.Remove( key );
        }


        public void SetTimestamp( String key, DateTime t ) {


            mcache.Put( "__ts_" + key, t );

        }

        public DateTime GetTimestamp( String key ) {

            Object obj = mcache.Get( "__ts_" + key );
            if (obj == null) return DateTime.MinValue;
            return (DateTime)obj;


        }


    }

}
