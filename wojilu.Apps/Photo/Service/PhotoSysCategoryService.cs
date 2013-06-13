/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;

namespace wojilu.Apps.Photo.Service {

    public class PhotoSysCategoryService : IPhotoSysCategoryService {

        public virtual List<PhotoSysCategory> GetAll() {
            return db.find<PhotoSysCategory>( "order by OrderId desc, Id asc" ).list();
        }

        public virtual List<PhotoSysCategory> GetForDroplist() {
            List<PhotoSysCategory> list = this.GetAll();
            PhotoSysCategory c = new PhotoSysCategory();
            c.Name = lang.get( "plsSelect" ) + "...";
            list.Insert( 0, c );
            return list;
        }


        public virtual PhotoSysCategory GetById( int id ) {
            return db.findById<PhotoSysCategory>( id );
        }



    }

}
