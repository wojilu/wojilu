using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Serialization;

namespace wojilu.cms.Domain {

    public class Category : ObjectBase<Category> {

        [Column( Length = 10 ), NotNull( "请输入名称" )]
        public string Name { get; set; }

        public string ToJson() {
            return Json.Serialize( this );
        }

        public static Category FromJson( string jsonString ) {
            return Json.DeserializeObject<Category>( jsonString );
        }

    }
}
