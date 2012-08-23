/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Apps.Blog.Service {

    public class BlogSysCategoryService : IBlogSysCategoryService {

        public virtual List<BlogSysCategory> GetAll() {
            return db.find<BlogSysCategory>( "order by OrderId desc, Id asc" ).list();
        }

        public virtual List<BlogSysCategory> GetForDroplist() {
            List<BlogSysCategory> list = this.GetAll();
            BlogSysCategory c = new BlogSysCategory();
            c.Name = lang.get( "plsSelect" ) + "...";
            list.Insert( 0, c );
            return list;
        }


        public virtual BlogSysCategory GetById( int id ) {
            return db.findById<BlogSysCategory>( id );
        }



    }

}
