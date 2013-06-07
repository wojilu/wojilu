/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Interface;
using wojilu.Common.MemberApp;
using wojilu.Common.AppInstall;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Common.AppBase {

    public class AppFactory {

        public static IApp Create( int appInfoId, IMember owner, AccessStatus accessStatus ) {

            return CreateApp( new AppInstallerService().GetById( appInfoId ), owner, accessStatus );
        }

        private static IApp CreateApp( AppInstaller appInfo, IMember owner, AccessStatus accessStatus ) {

            IApp app = Entity.New( appInfo.TypeFullName ) as IApp;
            app.OwnerId = owner.Id;
            app.OwnerUrl = owner.Url;
            app.OwnerType = owner.GetType().FullName;
            db.insert( app );

            UpdateAccessStatus( app, accessStatus );

            return app;
        }

        private static void UpdateAccessStatus( IApp app, AccessStatus accessStatus ) {
            IAccessStatus objApp = app as IAccessStatus;
            if (objApp != null) {
                objApp.AccessStatus = (int)accessStatus;
                db.update( objApp, "AccessStatus" );
            }
        }

        public static void UpdateAccessStatus( IMemberApp app, AccessStatus accessStatus ) {
            Type t = Entity.New( app.AppInfo.TypeFullName ).GetType();
            IAccessStatus objApp = ndb.findById( t, app.AppOid ) as IAccessStatus;
            if (objApp != null) {
                objApp.AccessStatus = (int)accessStatus;
                db.update( objApp, "AccessStatus" );
            }
        }



    }
}

