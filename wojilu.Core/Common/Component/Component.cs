using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;

namespace wojilu.Common {

    public class Component : CacheObject {

        public String TypeFullName { get; set; }

        public int Status { get; set; }


        [NotSave]
        public String StatusName {
            get { return ComponentStatus.GetStatusName( this.Status ); }
        }

        public static Boolean IsClose( Type t ) {

            List<Component> list = cdb.findAll<Component>();
            foreach (Component c in list) {

                if (c.TypeFullName.Equals( t.FullName )) {
                    return c.Status == ComponentStatus.Close.Id;
                }

            }

            return false;
        }


        public static Boolean IsEnableUserSpace() {
            return IsClose( typeof( User ) ) == false;
        }

        public static Boolean IsEnableGroup() {
            return IsClose( typeof( Group ) ) == false;
        }


    }



}
