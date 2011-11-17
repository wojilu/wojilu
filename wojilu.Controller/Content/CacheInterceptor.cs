/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.SOA;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content {

    public class CacheInterceptor : IInterceptor{

        private void removeCache( IEntity obj ) {

            String serviceTypeName = getServiceType( obj );

            Service service = getService( serviceTypeName );
            if (service == null) return;

            List<ContentApp> apps = getApps( service );

            foreach (ContentApp app in apps) {
                String pageName = getCachedPageName( app );
                removePageCache( pageName );
            }
        }

        private string getServiceType( IEntity obj ) {

            // wojilu.Apps.Photo.Domain.PhotoPost =>
            // wojilu.Apps.Photo.Service.PhotoPostService

            String typeName = strUtil.GetTypeName( obj.GetType() );
            String ns = strUtil.TrimEnd( obj.GetType().FullName, ".Domain."+ typeName );

            String serviceNs = ns + ".Service";

            return serviceNs + "." + typeName + "Service";

        }

        private Service getService( string serviceTypeName ) {

            List<Service> serviceList = cdb.findAll<Service>();

            foreach (Service s in serviceList) {
                if (s.Type.Equals( serviceTypeName )) return s;
            }

            return null;
        }

        private List<ContentApp> getApps( Service service ) {

            List<ContentSection> sections = ContentSection.find( "ServiceId=" + service.Id ).list();
            if (sections.Count == 0) return new List<ContentApp>();

            String ids = strUtil.GetIds( sections );
            if (strUtil.IsNullOrEmpty( ids )) return new List<ContentApp>();

            return ContentApp.find( "Id in (" + ids + ")" ).list();
        }

        private string getCachedPageName( ContentApp app ) {

            // 1) appId=396 Created=2010-10-21
            // 2) http://www.xxx.com/Content396/Content/Index.aspx
            // 3) /framework/cache/2010-10-21/Content396_Content_Index.config

            return string.Format( "{0}cache/{1}/Content{2}_Content_Index.config",
                cfgHelper.FrameworkRoot,
                string.Format( "{0}-{1}-{2}", app.Created.Year, app.Created.Month, app.Created.Day ),
                app.Id );
        }

        private void removePageCache( string pageName ) {

            String path = PathHelper.Map( pageName );
            if (file.Exists( path )) file.Delete( path );
        }


        public void AfterInsert( IEntity entity ) {
            removeCache( entity );
        }

        public void AfterUpdate( IEntity entity ) {
            removeCache( entity );
        }

        public void AfterDelete( IEntity entity ) {
            removeCache( entity );
        }

        public void BeforInsert( IEntity entity ) {
        }
        public void BeforUpdate( IEntity entity ) {
        }
        public void BeforDelete( IEntity entity ) {
        }

    }

}
