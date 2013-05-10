/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using System.Reflection;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Security {

    public class SecurityUtils {


        public static String getPath( MethodInfo method, String rootNamespace ) {
            String nsString;

            nsString = strUtil.TrimStart( method.ReflectedType.FullName, rootNamespace );
            nsString = strUtil.TrimEnd( nsString, "Controller" );
            nsString = strUtil.TrimStart( nsString, "." ).Replace( ".", "/" );

            String path = strUtil.Join( nsString, method.Name );
            return path;
        }

        public static String getPath( aAction action, String rootNamespace ) {
            String nsString;

            MethodInfo method = action.Method;

            nsString = strUtil.TrimStart( action.Target.GetType().FullName, rootNamespace );
            nsString = strUtil.TrimEnd( nsString, "Controller" );
            nsString = strUtil.TrimStart( nsString, "." ).Replace( ".", "/" );

            String path = strUtil.Join( nsString, method.Name );

            if (MvcConfig.Instance.IsUrlToLower) {
                path = path.ToLower();
            }

            return path;
        }

    }
}
