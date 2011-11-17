/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Interface {


    public interface IAdminLogService<T> where T : IAdminLog {

        DataPage<T> GetPage( int pageSize );
        DataPage<T> GetPage( String condition );

        void Add( User user, String action, String ip );
        void Add( User user, String action, String dataInfo, String dataType, String ip );
        void Add( User user, String action, String ip, int categoryId );


    }

}
