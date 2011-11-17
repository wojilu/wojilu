using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.DI;

namespace wojilu.Web.Controller.Common {

    public class MemcachedCache : ICacheHelper {

        private IApplicationCache c {
            get {
                return ObjectContext.GetByType( "wojilu.Caching.MemcachedCache" ) as IApplicationCache;
            }
        }

        public string ReadCache( string key ) {
            Object obj = c.Get( key );
            if (obj == null) return null;

            return obj.ToString();
        }

        public void AddCache( string key, string val ) {
            c.Put( key, val );
        }

        public void DeleteCache( string key ) {
            c.Remove( key );
        }


        public void SetTimestamp( String key, DateTime t ) {

            c.Put( "__ts_" + key, t );
        }

        public DateTime GetTimestamp( String key ) {

            Object obj = c.Get( "__ts_" + key );
            if (obj == null) return DateTime.MinValue;
            return (DateTime)obj;
        }


    }

}
