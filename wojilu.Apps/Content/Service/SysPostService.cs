/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase;
using wojilu.Apps.Content.Interface;

namespace wojilu.Apps.Content.Service {

    public class SysPostService : ISysPostService {

        public virtual DataPage<ContentPost> GetPage() {
            DataPage<ContentPost> list = ContentPost.findPage( "SaveStatus<>" + SaveStatus.SysDelete, 20 );
            return list;
        }


        public virtual void Delete( String ids ) {
            db.updateBatch<ContentPost>( "set SaveStatus=" + SaveStatus.SysDelete + "", "Id in (" + ids + ")" );
        }


        public virtual int GetDeleteCount() {
            return db.count<ContentPost>( "SaveStatus=" + SaveStatus.SysDelete );
        }


        public virtual DataPage<ContentPost> GetPageTrash() {
            DataPage<ContentPost> list = ContentPost.findPage( "SaveStatus=" + SaveStatus.SysDelete, 20 );
            return list;
        }

        public virtual ContentPost GetById_ForAdmin(long id) {
            return ContentPost.findById( id );
        }

        public virtual void UnDelete( ContentPost post ) {
            post.SaveStatus = SaveStatus.Normal;
            post.update();
        }

        public virtual void DeleteTrue( String ids ) {
            db.deleteBatch<ContentPost>( "Id in (" + ids + ")" );
        }

    }
}
