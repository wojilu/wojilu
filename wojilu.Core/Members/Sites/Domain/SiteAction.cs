/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Data;
using wojilu.Common.Security;

namespace wojilu.Members.Sites.Domain {

    [Serializable]
    public class SiteAction : CacheObject, ISecurityAction {

        private String _tip;
        //private String _actionUrl;

        public String Tip {
            get { return _tip; }
            set { _tip = value; }
        }
        //public String ActionUrl {
        //    get { return _actionUrl; }
        //    set { _actionUrl = value; }
        //}

        //public static IList GetAll() {
        //    return new SiteAction().FindAll();
        //}

        public String Url { get; set; }
        public String Format { get; set; }

        public ISecurityAction GetById( int id ) {
            return this.findById( id ) as ISecurityAction;
        }

    }
}

