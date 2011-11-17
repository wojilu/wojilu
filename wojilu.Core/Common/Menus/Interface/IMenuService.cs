/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Common.Menus.Interface {


    public interface IMenuService {

        Type GetMemberType();
        IMenu New();

        IMenu FindById( int ownerId, int menuId );
        List<IMenu> GetList( IMember owner );
        List<IMenu> GetRootList( IMember owner );
        List<IMenu> GetByParent( IMenu m );

        Result Insert( IMenu menu, User creator, IMember owner );
        Result Update( IMenu menu );
        Result Delete( IMenu menu );

        IMenu FindByApp( String rawAppUrl );
        IMenu AddMenuByApp( IMemberApp app, String name, String friendUrl, String rawAppUrl );
        void RemoveMenuByApp( IMemberApp app, String rawAppUrl );
        void UpdateMenuByApp( IMemberApp app, String rawAppUrl );

    }

}

