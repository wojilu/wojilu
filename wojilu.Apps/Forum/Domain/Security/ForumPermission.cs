/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Security;
using wojilu.ORM;

namespace wojilu.Apps.Forum.Domain.Security {

    [Serializable]
    public class ForumPermission {



        public static String GetDefaultPermission() {

            String str = "wojilu.Apps.Forum.Domain.ForumRole:1:1_2_3_4_5_6_7_8_9_10_11_12/wojilu.Members.Sites.Domain.SiteRole:0:1_2_3_4_5_6_7/wojilu.Members.Sites.Domain.SiteRole:1:1_2_3_4_5_6_7_8_9_10_11_12/wojilu.Members.Sites.Domain.SiteRole:2:1_2_3_4_5_6_7_8_9_10_11/wojilu.Members.Sites.Domain.SiteRole:4:1_2_3_4_5_6_7_8_9_10_11/wojilu.Members.Sites.Domain.SiteRole:5:1_2_3_4_5_6_7_8_9_10_11/wojilu.Members.Sites.Domain.SiteRank:1:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:2:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:3:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:4:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:5:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:6:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:7:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:8:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:9:1_2_3_4_5/wojilu.Members.Sites.Domain.SiteRank:10:1_2_3_4_5";

            return str;
        }

        public static void AddOwnerAdminPermission( Object obj ) {

            ISecurity s = obj as ISecurity;

            IEntity eobj = obj as IEntity;

            String ownerType = eobj.get( "OwnerType" ) as string;
            int ownerId = cvt.ToInt( eobj.get( "OwnerId" ) );

            if (ownerType.Equals( typeof( Site ).FullName ) == false) {

                Type t = Entity.GetType( ownerType );
                IMember owner = ndb.findById( t, ownerId ) as IMember;
                if (owner == null) return;

                s.Security = addOwnerAdminPermission( s.Security, ownerType, owner.GetAdminRole() );

                db.update( obj, "Security" );
            }
        }

        private static String addOwnerAdminPermission( String securityString, String ownerType, IRole adminRole ) {
            String s = adminRole.GetType().FullName + ":" + adminRole.Id + ":1_2_3_4_5_6_7_8_9_10_11_12";
            return strUtil.Join( securityString, s, "/" );
        }


    }
}
