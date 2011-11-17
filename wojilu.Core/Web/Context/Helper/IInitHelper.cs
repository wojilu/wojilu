using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Interface;

namespace wojilu.Web.Context {

    public interface IInitHelper {

        Type GetMemberType();
        IMember getOwnerByUrl( MvcContext ctx );
        Boolean IsAppRunning( MvcContext ctx );
        List<IMenu> GetMenus( IMember owner );
    }

}
