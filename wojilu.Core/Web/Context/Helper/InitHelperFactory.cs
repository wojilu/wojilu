/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.DI;
using wojilu.Members.Interface;

namespace wojilu.Web.Context {



    public class InitHelperFactory {

        private static Dictionary<String, IInitHelper> _InitList = getInitList();

        private static Dictionary<String, IInitHelper> getInitList() {

            Dictionary<String, IInitHelper> map = new Dictionary<String, IInitHelper>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (rft.IsInterface( kv.Value, typeof( IInitHelper ) ) == false) continue;

                IInitHelper obj = ObjectContext.CreateObject( kv.Value ) as IInitHelper;

                map.Add( obj.GetMemberType().Name.ToLower(), obj );

            }

            return map;

        }

        public static IInitHelper GetHelper( MvcContext ctx ) {

            IInitHelper obj = null;
            _InitList.TryGetValue( ctx.route.ownerType, out obj );

            if (obj == null) throw ctx.ex( HttpStatus.NotFound_404, "not support owner" );

            return obj;
        }

        public static IInitHelper GetHelper( Type ownerType, MvcContext ctx ) {

            IInitHelper obj = null;
            _InitList.TryGetValue( ownerType.Name.ToLower(), out obj );

            if (obj == null) throw ctx.ex( HttpStatus.NotFound_404, "not support owner" );

            return obj;
        }

    }






}
