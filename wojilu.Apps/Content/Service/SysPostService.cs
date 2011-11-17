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

        public DataPage<ContentPost> GetPage() {
            DataPage<ContentPost> list = ContentPost.findPage( "SaveStatus<>" + SaveStatus.SysDelete, 20 );
            return list;
        }


        public void Delete( String ids ) {
            db.updateBatch<ContentPost>( "set SaveStatus=" + SaveStatus.SysDelete + "", "Id in (" + ids + ")" );
        }


        public int GetDeleteCount() {
            return db.count<ContentPost>( "SaveStatus=" + SaveStatus.SysDelete );
        }


        public DataPage<ContentPost> GetPageTrash() {
            DataPage<ContentPost> list = ContentPost.findPage( "SaveStatus=" + SaveStatus.SysDelete, 20 );
            return list;
        }

        public ContentPost GetById_ForAdmin( int id ) {
            return ContentPost.findById( id );
        }

        public void UnDelete( ContentPost post ) {
            post.SaveStatus = SaveStatus.Normal;
            post.update();
        }

        public void DeleteTrue( String ids ) {
            db.deleteBatch<ContentPost>( "Id in (" + ids + ")" );
        }

    }
}
