using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Mvc.Attr {

    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class CacheActionAttribute : Attribute {

        private Type _actionCacheType;

        public Type ActionCacheType {
            get { return _actionCacheType; }
            set { _actionCacheType = value; }
        }

        public CacheActionAttribute( Type t ) {
            this.ActionCacheType = t;
        }
    }

}
