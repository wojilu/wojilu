/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Common.MemberApp.Interface {

    public interface IMemberAppService {

        Type GetMemberType();
        IMenuService menuService { get; set; }

        IMemberApp New();

        IMemberApp Add( User creator, IMember owner, String name, int appinfoId, AccessStatus accessStatus );
        IMemberApp Add( User creator, String name, int appinfoId );


        void Delete( IMemberApp app, String rawAppUrl );

        IMemberApp FindById( int userAppId, int userId );
        IList GetAppInfos( int memberId );
        IMemberApp GetByApp( IApp app );
        IMemberApp GetByApp( Type t, int appId );
        IList GetByMember( int memberId );

        void Start( IMemberApp app, String rawAppUrl );
        void Stop( IMemberApp app, String rawAppUrl );
        void Update( IMemberApp app, String newName, String rawAppUrl );
        void UpdateAccessStatus( IMemberApp app, AccessStatus accessStatus );


        Boolean HasInstall( int ownerId, int appInfoId );
    }
}

