/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Members.Groups.Domain {

    public class GroupAccessStatus {

        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public GroupAccessStatus( int id, String name, String desc ) {
            this.Id = id;
            this.Name = name;
            this.Description = desc;
        }

        private static List<GroupAccessStatus> all = getAll();

        private static List<GroupAccessStatus> getAll() {

            List<GroupAccessStatus> all = new List<GroupAccessStatus>();
            all.Add( new GroupAccessStatus( 0, lang.get( "groupOpen" ), lang.get( "groupOpenInfo" ) ) );
            all.Add( new GroupAccessStatus( 1, lang.get( "groupClosed" ), lang.get( "groupClosedInfo" ) ) );
            all.Add( new GroupAccessStatus( 2, lang.get( "groupSecret" ), lang.get( "groupSecretInfo" ) ) );

            return all;
        }

        public static List<GroupAccessStatus> All() {
            return all;
        }

        public static GroupAccessStatus GetById( int id ) {
            if (id == 0 || id==1 || id==2) return all[id];
            return null;
        }

        public static readonly int Open = 0;
        public static readonly int Closed = 1;
        public static readonly int Secret = 2;

        //public static readonly int SystemLocked = 3;
        //public static readonly int SystemHidden = 4;



    }

}
