/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.Common.Categories {


    [Serializable]
    public abstract class CategoryBase : ObjectBase<CategoryBase>, INode, ISort {

        protected CategoryBase() { }

        public int OwnerId { get; set; }

        [Column( Length = 150 )]
        public String OwnerUrl { get; set; }

        public int AppId { get; set; }
        public int OrderId { get; set; }
        public int ParentId { get; set; }

        [Column( Length = 50 )]
        [NotNull( Lang = "exName" )]
        public String Name { get; set; }

        [LongText]
        public String Description { get; set; }

        [Column( Length = 150 )]
        public String Logo { get; set; }

        public int DataCount { get; set; }
        public DateTime Created { get; set; }

        public List<CategoryBase> GetByAppId( int appId ) {
            return db.find<CategoryBase>( "AppId=" + appId + " order by Id" ).list();
        }


        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

    }
}

