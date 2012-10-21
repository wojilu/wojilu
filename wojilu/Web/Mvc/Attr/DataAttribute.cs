/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Context;


namespace wojilu.Web.Mvc.Attr {

    /// <summary>
    /// 检查当前数据是否存在 
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class DataAttribute : System.Attribute, IActionFilter {

        private Type _dataType;

        public DataAttribute( Type dataType ) {
            _dataType = dataType;
        }

        public Type DataType {
            get { return _dataType; }
            set { _dataType = value; }
        }


        public void BeforeAction( ControllerBase controller ) {

            MvcContext ctx = controller.ctx;

            IEntity obj = ndb.findById( _dataType, ctx.route.id );
            if (obj == null) {
                ctx.utils.endMsg( lang.get( "exDataNotFound" ), HttpStatus.NotFound_404 );
                return;
            }

            ctx.SetItem( "__currentRouteIdData", obj );
        }

        public void AfterAction( ControllerBase controller ) {
        }

        public int Order { get; set; }

    }

}
