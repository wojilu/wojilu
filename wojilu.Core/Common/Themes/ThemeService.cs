using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppInstall;

namespace wojilu.Common.Themes {

    public class ThemeService : IThemeService {


        public List<ITheme> GetThemeList( AppInstaller installer ) {
            List<ITheme> list = new List<ITheme>();
            if (strUtil.IsNullOrEmpty( installer.ThemeType )) return list;

            Type themeType = ObjectContext.GetType( installer.ThemeType );
            if (themeType == null) return list;

            ITheme obj = ObjectContext.Create<ITheme>( themeType );
            return obj.GetAll();
        }

        public ITheme GetThemeById( AppInstaller installer, String id ) {

            if (strUtil.IsNullOrEmpty( installer.ThemeType )) return null;

            Type themeType = ObjectContext.GetType( installer.ThemeType );
            if (themeType == null) return null;

            ITheme obj = ObjectContext.Create<ITheme>( themeType );
            return obj.GetById( id );
        }
    }
}
