/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Common.AppInstall {

    public interface IAppInstallerService {

        /// <summary>
        /// 安装某个app
        /// </summary>
        /// <param name="appType">必须事先IApp的类型，比如ForumApp</param>
        /// <param name="owner"></param>
        /// <param name="creator"></param>
        /// <param name="appName">app名称</param>
        /// <returns></returns>
        IMemberApp Install( Type appType, IMember owner, User creator, String appName );

        List<AppInstaller> GetAll();
        List<AppInstaller> GetByCategory( int categoryId );
        List<AppInstaller> GetUserDataAdmin();

        AppInstaller GetById( int id );
        AppInstaller GetApprovedById( int id, Type ownerType );

        List<AppInstaller> GetByOwnerType( Type ownerType );
        AppInstaller GetByType( Type appType );
        AppInstaller GetByTypeFullName( String typeFullName );
        AppInstaller GetByTypeName( String typeName );

        void UpdateStatus( AppInstaller installer, string val );
    }
}
