using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Members.Interface;

namespace wojilu.ORM.Utils {

    internal class ValueSetter {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ValueSetter ) );

        /// <summary>
        /// 1.5新增，针对已删除用户应用 null object 模式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ep"></param>
        /// <param name="propertyValue"></param>
        public static void setEntityByCheckNull( IEntity obj, EntityPropertyInfo ep, Object propertyValue, int realUserId ) {


            if (propertyValue == null && rft.IsInterface( ep.Type, typeof( IUser ) )) {

                IEntity user = getNullUser( realUserId );
                obj.set( ep.Name, user );
            }
            else {
                obj.set( ep.Name, propertyValue );
            }

        }

        private static IEntity getNullUser( int realUserId ) {

            IUserFactory userFactory = CurrentRequest.getItem( "_user_factory" ) as IUserFactory;
            if (userFactory == null) return null;
            IEntity user = userFactory.NullUser() as IEntity;
            if (user != null) {
                user.set( "RealId", realUserId );
            }
            return user;

        }

    }

}
