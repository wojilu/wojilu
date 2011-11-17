/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace wojilu.Common.AppInstall {

    public interface IAppInstallerService {
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
