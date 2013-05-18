using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.AppInstall;
using wojilu.Data;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Admin {

    public class AppThemeController : ControllerBase {

        public void Index( int appInstallerId ) {

            AppInstaller installer = cdb.findById<AppInstaller>( appInstallerId );
            List<CacheObject> themeList = getThemeList( installer );
            themeList.ForEach( x => x.data.delete = to( Delete, appInstallerId ) + "?id=" + x.Id );

            bindList( "list", "x", themeList );

        }

        private List<CacheObject> getThemeList( AppInstaller installer ) {
            List<CacheObject> list = new List<CacheObject>();
            if (strUtil.IsNullOrEmpty( installer.ThemeType )) return list;

            Type themeType = ObjectContext.GetType( installer.ThemeType );
            if (themeType == null) return list;

            return cdbx.findAll( themeType );
        }

        [HttpDelete]
        public void Delete( int appInstallerId ) {
            int id = ctx.GetInt( "id" );
            AppInstaller installer = cdb.findById<AppInstaller>( appInstallerId );

            CacheObject objTheme = getThemeById( installer, id );
            if (objTheme == null) {
            }

            objTheme.delete();

            rft.CallMethod( objTheme, "DeleteTheme" );                

            echoRedirect( "删除成功", to( Index, appInstallerId ) );

        }

        private CacheObject getThemeById( AppInstaller installer, int id ) {

            if (strUtil.IsNullOrEmpty( installer.ThemeType )) return null;

            Type themeType = ObjectContext.GetType( installer.ThemeType );
            if (themeType == null) return null;

            return cdbx.findById( themeType, id );
        }

    }

}
