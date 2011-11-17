/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;


namespace wojilu.Members.Common.Service {

    public class AdminLogService<T> : IAdminLogService<T> where T : ObjectBase<T>, IAdminLog {

        public DataPage<T> GetPage( int pageSize ) {
            return db.findPage<T>( "", pageSize );
        }

        public DataPage<T> GetPage( String condition ) {
            return db.findPage<T>( condition, 20 );
        }

        private void Add( IAdminLog log ) {
            db.insert( log );
        }

        public void Add( User user, String action, String ip ) {
            this.Add( user, action, ip, 0 );
        }

        public void Add( User user, String action, String ip, int categoryId ) {
            this.Add( user, action, "", "", ip, categoryId );
        }

        public void Add( User user, String action, String dataInfo, String dataType, String ip ) {
            this.Add( user, action, dataInfo, dataType, ip, 0 );
        }

        private void Add( User user, String action, String dataInfo, String dataType, String ip, int categoryId ) {
            IAdminLog log = Entity.New( typeof( T ).FullName ) as IAdminLog;
            log.User = user;
            log.Message = action;
            log.Ip = ip;
            log.CategoryId = categoryId;
            log.DataInfo = dataInfo;
            log.DataType = dataType;

            this.Add( log );
        }





    }
}
