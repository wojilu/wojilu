/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Service {

    public class SiteMessageService : ISiteMessageService {

        public MessageSite GetById( int id ) {
            return MessageSite.findById( id );
        }

        public DataPage<MessageSite> GetPage( int pageSize ) {
            return MessageSite.findPage( "", pageSize );
        }

        public Result Insert( MessageSite msg ) {

            return msg.insert();
        }

        public Result Update( MessageSite msg ) {

            return msg.update();
        }

        public void Delete( MessageSite msg ) {
            msg.delete();
        }

    }

}
