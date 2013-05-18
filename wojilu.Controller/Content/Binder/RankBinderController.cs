using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Common;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Binder {

    public class RankBinderController : ControllerBase, ISectionBinder {

        public void Bind( ContentSection section, IList serviceData ) {

            IBlock block = base.getBlock( "list" );

            int i = 1;
            foreach (IBinderValue item in serviceData) {

                BinderUtils.bindMashupData( block, item, i, ctx );
                block.Next();
                i++;
            }
        }


    }
}
