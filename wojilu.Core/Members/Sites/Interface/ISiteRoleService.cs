/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Members.Sites.Domain;
using wojilu.Common.Security;

namespace wojilu.Members.Sites.Service {

    public interface ISiteRoleService {

        SiteRole GetById( int id );
        void InsertSiteRole( SiteRole role );
        void UpdateSiteRole( SiteRole role );
        void DeleteSiteRole( SiteRole role );


        List<SiteRole> GetAllRoles();
        List<SiteRole> GetRolesWithotGuest();
        List<SiteRole> GetAdminRoles();
        List<SiteRole> GetNormalRoles();


        List<SiteRank> GetRankAll();
        SiteRank GetRankById( int id );
        SiteRank GetRankByCredit( int credit );
        SiteRank GetNextRank( SiteRank rank );
        SiteRank GetPreRank( SiteRank rank );
        void InsertRank( SiteRank rank );
        void UpdateRank( SiteRank rank );
        void DeleteRank( SiteRank rank );


        List<SiteRankOther> GetRankOther();
        SiteRankOther GetRankOtherById( int id );
        void UpdateRankByOther( SiteRankOther otherRank );


        List<IRole> GetRoleAndRank();
        List<IRole> GetRoleAndRank( IList newRoles );

    }

}
