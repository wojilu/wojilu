/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;


namespace wojilu.Common.Comments {

    public interface ICommentService<T> where T : ObjectBase<T>, IComment {

        T GetById( int commentId, int appId );
        //List<T> GetNew( int ownerId, int appId, int count );

        DataPage<T> GetPage( int appId );
        DataPage<T> GetPageAll( String condition );
        DataPage<T> GetPageByTarget( int blogId, int pageSize );
        Result Insert( IComment c, String lnkTarget );

        void Delete( IComment c );
        void DeleteBatch( String ids );


        void Reply( IComment parent, IComment comment, String lnkTarget );
    }

}
