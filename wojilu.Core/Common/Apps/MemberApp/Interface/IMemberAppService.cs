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

        IMemberApp Add(User creator, IMember owner, string name, long appinfoId, AccessStatus accessStatus);
        IMemberApp Add(User creator, string name, long appinfoId);


        void Delete( IMemberApp app, String rawAppUrl );

        IMemberApp FindById(long userAppId, long userId);
        IList GetAppInfos(long memberId);
        IMemberApp GetByApp( IApp app );
        IMemberApp GetByApp(Type t, long appId);
        IList GetByMember(long memberId);

        void Start( IMemberApp app, String rawAppUrl );
        void Stop( IMemberApp app, String rawAppUrl );
        void Update( IMemberApp app, String newName, String rawAppUrl );
        void UpdateAccessStatus( IMemberApp app, AccessStatus accessStatus );


        bool HasInstall(long ownerId, long appInfoId);
    }
}

