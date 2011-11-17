using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Download.Domain {

    public class FileLang : CacheObject, ISort, IComparable {
        
        public int OrderId { get; set; }

        public void updateOrderId() {
            this.update();
        }

        public int CompareTo( object obj ) {
            FileLang t = obj as FileLang;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        //---------------------------------------------------------------


        public static List<FileLang> GetAll() {
            List<FileLang> list = cdb.findAll<FileLang>();
            list.Sort();
            return list;
        }

        public static FileLang GetById( int id ) {
            return cdb.findById<FileLang>( id );
        }

        public static String GetName( int id ) {
            return cdb.findById<FileLang>( id ).Name;
        }

    }

}
