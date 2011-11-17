/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Feeds.Interface;

namespace wojilu.Common.Feeds.Domain {

    [Serializable]
    public class Share : ObjectBase<Share>, IFeed {

        public User Creator { get; set; }
        public String DataType { get; set; }

        public int Replies { get; set; }

        public String TitleTemplate { get; set; }
        [LongText]
        public String TitleData { get; set; }

        public String BodyTemplate { get; set; }
        [LongText]
        public String BodyData { get; set; }

        [LongText]
        public String BodyGeneral { get; set; }

        private String _hashdata;
        public String HashData {
            get {  
                if( strUtil.IsNullOrEmpty( _hashdata ) )
                    _hashdata = new HashTool().Get( this.BodyTemplate + this.BodyData, HashType.MD5 );
                return _hashdata;
            }
            set {
                _hashdata = value;
            }
        }


        public DateTime Created { get; set; }



        private List<ShareComment> _comments = new List<ShareComment>();

        public List<ShareComment> GetComments() {
            return _comments;
        }

        public void setComments( List<ShareComment> comments ) {
            _comments = comments;
        }


    }

}
