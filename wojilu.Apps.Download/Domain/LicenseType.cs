using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Download.Domain {

    public class LicenseType : CacheObject, ISort, IComparable {
        
        public int OrderId { get; set; }

        public void updateOrderId() {
            this.update();
        }

        public int CompareTo( object obj ) {
            LicenseType t = obj as LicenseType;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        //---------------------------------------------------------------


        public static List<LicenseType> GetAll() {
            List<LicenseType> list = cdb.findAll<LicenseType>();
            list.Sort();
            return list;
        }

        public static LicenseType GetById( int id ) {
            return cdb.findById<LicenseType>( id );
        }

        public static String GetName( int id ) {
            return cdb.findById<LicenseType>( id ).Name;
        }

    }


}
