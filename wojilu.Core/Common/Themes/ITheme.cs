using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppInstall;

namespace wojilu.Common.Themes {

    public interface ITheme {
        String Id { get; set; }
        String Name { get; set; }
        String Description { get; set; }
        String Pic { get; set; }

        List<ITheme> GetAll();
        ITheme GetById( String id );
        void Delete();
    }

}
