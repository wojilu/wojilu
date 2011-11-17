using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Mvc.Attr {

    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class CacheActionAttribute : Attribute {

        private Type _cacheInfoType;

        public Type Type {
            get { return _cacheInfoType; }
            set { _cacheInfoType = value; }
        }

        public CacheActionAttribute( Type t ) {
            this.Type = t;
        }
    }

}
