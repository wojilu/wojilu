using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Security {

    public class SiteAdminService {


        public static Boolean HasAdminPermission( IMember objUser, String urlPath ) {

            User user = (User)objUser;

            String applicationPath = SystemInfo.ApplicationPath;

            String checkedPath = strUtil.TrimStart( urlPath, applicationPath ).TrimStart( '/' );

            SiteAdminOperation forbiddenAction = OperationDB.GetForbiddenAdminOperations( checkedPath );
            if (forbiddenAction == null) return true;

            List<SiteAdminOperation> userActions = SiteAdminOperationConfig.Instance.GetActionsByUser( user );
            foreach (SiteAdminOperation action in userActions) {
                if (action.Id == forbiddenAction.Id) return true;
            }

            return false;
        }

    }

}
