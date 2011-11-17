/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.ORM;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Photo.Domain {

    [Serializable]
    public class PhotoSysCategory : ObjectBase<PhotoSysCategory>, ISort {

        public PhotoSysCategory() {
        }

        public PhotoSysCategory( String name ) {
            this.Name = name;
        }

        public String Name { get; set; }
        public int OrderId { get; set; }


        public void updateOrderId() {
            base.update( "OrderId" );
        }

    }

}
