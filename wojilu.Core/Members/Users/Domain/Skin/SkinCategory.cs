/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.ORM;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class SkinCategory : ObjectBase<SkinCategory> {

        private int _DataCount;
        private String _Description;
        private String _ImgUrl;
        private int _OrderId;
        private String _Title;
        private String _Url;
        private DateTime _CreateTime;


        public String Name {
            get { return _Title; }
            set { _Title = value; }
        }

        public int OrderId {
            get { return _OrderId; }
            set { _OrderId = value; }
        }

        public int DataCount {
            get { return _DataCount; }
            set { _DataCount = value; }
        }

        public String Description {
            get { return _Description; }
            set { _Description = value; }
        }

        public String ImgUrl {
            get { return _ImgUrl; }
            set { _ImgUrl = value; }
        }

        public String Url {
            get { return _Url; }
            set { _Url = value; }
        }

        public DateTime CreateTime {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

    }
}

