/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using wojilu.Members.Interface;

namespace wojilu.Web.Context {

    public class OwnerContext : IOwnerContext {

        private int _Id;
        private IMember _obj;

        public int Id {
            get { return _Id; }
            set { _Id = value; }
        }

        public IMember obj {
            get { return _obj; }
            set { _obj = value; }
        }

    }
}

