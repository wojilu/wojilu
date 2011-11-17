/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Serialization;
using wojilu.Members.Users.Domain;
using wojilu.Common.Feeds.Service;

namespace wojilu.Common.Feeds.Domain {

    [Serializable]
    public class FeedInfo {

        private Dictionary<string, object> dic = new Dictionary<string, object>();
        private int templateId;
        private String commentContet;
        private String typeFullName;
        private User creator;

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

        public void Publish() {
            new FeedService().publishUserAction( creator, typeFullName, templateId, JSON.DicToString( dic ), commentContet );
        }
    }

}
