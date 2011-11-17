/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Common.Security;
using wojilu.Common.Security.Utils;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Security {

    public class SiteAdminOperationConfig : CacheObject, ISecurity {

        public String Security { get; set; }

        //-------------------------------------------

        public static SiteAdminOperationConfig Instance {
            get {
                IList list = new SiteAdminOperationConfig().findAll();
                if (list.Count == 0) {
                    SiteAdminOperationConfig config = new SiteAdminOperationConfig();
                    config.insert();
                    return config;
                }
                return (list[0] as SiteAdminOperationConfig);
            }
        }

        private SecurityTool getSecurityTool() {
            // TODO 注入 apps 动态权限
            SecurityTool tool = new SecurityTool( this, new SiteAdminOperation(), new SiteRoleService().GetAdminRoles() );
            return tool;
        }

        public List<SiteAdminOperation> GetActionsByUser( User user ) {
            IList actionsByRole = getSecurityTool().GetActionsByRole( user.Role );
            IList actionsByRank = getSecurityTool().GetActionsByRole( user.Rank );

            List<SiteAdminOperation> results = new List<SiteAdminOperation>();
            foreach (SiteAdminOperation action in actionsByRole) {
                if (!results.Contains( action )) results.Add( action );
            }
            foreach (SiteAdminOperation action in actionsByRank) {
                if (!results.Contains( action )) results.Add( action );
            }
            return results;
        }

    }

}
