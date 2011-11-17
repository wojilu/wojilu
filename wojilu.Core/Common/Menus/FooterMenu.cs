/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Data;
using wojilu.Common.AppBase;

namespace wojilu.Common.Menus {

    [Serializable]
    public class FooterMenu : CacheObject, ISort, IComparable {

        public String Link { get; set; }

        public int OrderId { get; set; }

        public void updateOrderId() {
            this.update();
        }

        public int CompareTo( object obj ) {
            FooterMenu t = obj as FooterMenu;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        public static List<FooterMenu> GetAll() {
            List<FooterMenu> list = cdb.findAll<FooterMenu>();
            list.Sort();
            return list;
        }

        public static FooterMenu GetById( int id ) {
            return cdb.findById<FooterMenu>( id );
        }

    }

}
