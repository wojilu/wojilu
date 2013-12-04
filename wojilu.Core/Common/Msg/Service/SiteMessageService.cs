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

        public virtual MessageSite GetById(long id) {
            return MessageSite.findById( id );
        }

        public virtual DataPage<MessageSite> GetPage( int pageSize ) {
            return MessageSite.findPage( "", pageSize );
        }

        public virtual Result Insert( MessageSite msg ) {

            return msg.insert();
        }

        public virtual Result Update( MessageSite msg ) {

            return msg.update();
        }

        public virtual void Delete( MessageSite msg ) {
            msg.delete();
        }

    }

}
