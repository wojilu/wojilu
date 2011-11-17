using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Controller.Common {

    public interface ICacheHelper {

        String ReadCache( String key );
        void AddCache( String key, String val );
        void DeleteCache( String key );

        void SetTimestamp( String key, DateTime t );
        DateTime GetTimestamp( String key );

    }

    public enum CacheType {
        Disk,
        InMemory,
        Memechaed,
        None
    }

    public class NonCache : ICacheHelper {


        public string ReadCache( string key ) {
            return null;
        }

        public void AddCache( string key, string val ) {
        }

        public void DeleteCache( string key ) {
        }

        public void SetTimestamp( string key, DateTime t ) {
        }

        public DateTime GetTimestamp( string key ) {
            return DateTime.MinValue;
        }

    }

}
