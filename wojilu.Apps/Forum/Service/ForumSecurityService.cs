/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Context;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Common.Security;
using wojilu.Common.Security.Utils;

namespace wojilu.Apps.Forum.Service {

    public class ForumSecurityService {


        public static SecurityTool GetSecurityTool( ISecurity f, MvcContext ctx ) {

            IList forumRoles = ForumRole.GetAll();

            IList ownerRoles;
            if (ctx.owner.obj.GetType() != typeof( Site ))
                ownerRoles = ctx.owner.obj.GetRoles();
            else
                ownerRoles = new ArrayList();

            IList siteRoles = new SiteRoleService().GetRoleAndRank();

            IList allRoles = new RoleMerger()
                .Add( forumRoles )
                .Add( ownerRoles )
                .Add( siteRoles )
                .GetResults();

            SecurityTool tool = new SecurityTool( f, new ForumAction(), allRoles );
            return tool;
        }



    }

}
