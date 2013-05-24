using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.AppInstall;
using wojilu.Data;
using wojilu.Web.Mvc.Attr;
using System.Collections;
using wojilu.Common.Themes;

namespace wojilu.Web.Controller.Admin {

    public class AppThemeController : ControllerBase {

        public IThemeService themeService { get; set; }

        public AppThemeController() {
            themeService = new ThemeService();
        }

        public void Index( int appInstallerId ) {

            AppInstaller installer = cdb.findById<AppInstaller>( appInstallerId );
            List<ITheme> themeList = themeService.GetThemeList( installer );

            bindList( "list", "x", themeList, bindThemes );
        }

        public void bindThemes( IBlock block, String lbl, Object x ) {
            ITheme theme = x as ITheme;
            block.Set( "x.data.delete", to( Delete, ctx.route.id ) + "?id=" + theme.Id );
        }


        [HttpDelete]
        public void Delete( int appInstallerId ) {
            String id = ctx.Get( "id" );
            AppInstaller installer = cdb.findById<AppInstaller>( appInstallerId );

            ITheme objTheme = themeService.GetThemeById( installer, id );
            if (objTheme == null) {
                echoError( "主题不存在" );
                return;
            }

            objTheme.Delete();

            echoRedirect( "删除成功", to( Index, appInstallerId ) );

        }



    }

}
