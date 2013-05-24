using System;
using System.Collections.Generic;
using wojilu.Common.AppInstall;

namespace wojilu.Common.Themes {

    public interface IThemeService {

        ITheme GetThemeById( AppInstaller installer, string id );

        List<ITheme> GetThemeList( AppInstaller installer );

    }

}
