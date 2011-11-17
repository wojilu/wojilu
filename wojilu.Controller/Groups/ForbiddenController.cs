/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Groups.Domain;

namespace wojilu.Web.Controller.Groups {

    public class ForbiddenController : ControllerBase {

        public void Group() {

            HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );

            Group g = ctx.owner.obj as Group;
            set( "g.Name", g.Name );
            set( "g.Logo", g.LogoSmall );

            if (g.IsCloseJoinCmd == 1) {
                set( "showJoinCmd", "display:none;" );
                set( "joinCmd", "#" );
            }
            else {

                set( "showJoinCmd", "" );
                set( "joinCmd", to( new Groups.JoinController().Index ) );

            }




        }


    }

}
