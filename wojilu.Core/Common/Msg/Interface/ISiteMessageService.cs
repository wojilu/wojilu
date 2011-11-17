/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {

    public interface ISiteMessageService {

        MessageSite GetById( int id );
        DataPage<MessageSite> GetPage( int pageSize );

        Result Insert( MessageSite msg );
        Result Update( MessageSite msg );

        void Delete( MessageSite msg );
    }

}
