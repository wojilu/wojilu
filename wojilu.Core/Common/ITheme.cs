using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppInstall;

namespace wojilu.Common {

    public interface ITheme {
        String Id { get; set; }
        String Name { get; set; }
        String Description { get; set; }
        String Pic { get; set; }

        List<ITheme> GetAll();
        ITheme GetById( String id );
        void Delete();
    }

    public class ThemeHelper {


        public static List<ITheme> GetThemeList( AppInstaller installer ) {
            List<ITheme> list = new List<ITheme>();
            if (strUtil.IsNullOrEmpty( installer.ThemeType )) return list;

            Type themeType = ObjectContext.GetType( installer.ThemeType );
            if (themeType == null) return list;

            ITheme obj = ObjectContext.Create<ITheme>( themeType );
            return obj.GetAll();
        }

        public static ITheme GetThemeById( AppInstaller installer, String id ) {

            if (strUtil.IsNullOrEmpty( installer.ThemeType )) return null;

            Type themeType = ObjectContext.GetType( installer.ThemeType );
            if (themeType == null) return null;

            ITheme obj = ObjectContext.Create<ITheme>( themeType );
            return obj.GetById( id );
        }

    }

}
