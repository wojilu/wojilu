/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Feeds.Service;

using wojilu.Members.Users.Domain;

namespace wojilu.Common.Feeds.Domain {

    [Serializable]
    public class FeedInfo {

        private Dictionary<string, object> dic = new Dictionary<string, object>();
        private int templateId;
        private String commentContet;
        private String typeFullName;
        private User creator;

        private String ip;

        public void AddData( String key, String value ) {
            dic.Add( key, value );
        }

        public void SetTemplateId( int id ) {
            templateId = id;
        }

        public void SetComment( String content ) {
            commentContet = content;
        }

        public void SetTypeFullName( String name ) {
            typeFullName = name;
        }

        public void SetCreator( User user ) {
            creator = user;
        }

        public void SetIp( String ip ) {
            this.ip = ip;
        }

        public void Publish() {
            new FeedService().publishUserAction( creator, typeFullName, templateId, Json.ToString( dic ), commentContet, ip );
        }
    }

}
