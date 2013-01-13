/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;
using wojilu.Web.Context;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.AppBase {




    public class ExtObject {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ExtObject ) );

        // 参数 ctx 提供当前登录用户信息和link
        public static String GetExtView( int id, String typeFullName, String extTypeFullName, MvcContext ctx ) {
            IExtViewService viewService = getExtDataService( extTypeFullName );
            if (viewService == null) throw new NotImplementedException( "not implemented view service:" + getServiceTypeName( extTypeFullName ) );
            logger.Info( "view service name: " + viewService.GetType().FullName );

            return viewService.GetViewById( id, typeFullName, ctx );
        }

        private static IExtViewService getExtDataService( String typeFullName ) {
            String serviceType = getServiceTypeName( typeFullName );
            IExtViewService extData = ObjectContext.GetByType( serviceType ) as IExtViewService;

            return extData;
        }

        private static String getServiceTypeName( String typeFullName ) {
            // 思路：
            // 1、根据 typeFullName 从 ObjectContext 容器中，获取对应的 service
            // 2、这个 service 必须实现了 IExtDataService 接口：GetById( int id, int appId )

            String typeName = strUtil.GetTypeName( typeFullName );
            String ns = strUtil.TrimEnd( typeFullName, "." + typeName );

            // wojilu.Apps.Content.Domain => wojilu.Apps.Content.Views
            String serviceNs = strUtil.TrimEnd( ns, "Domain" ) + "Views";

            // wojilu.Apps.Content.Views.PollExtView
            String serviceType = serviceNs + "." + typeName + "ExtViewService";
            return serviceType;
        }

    }

}
