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

        public IContentCustomTemplateService ctService { get; set; }

        public RankBinderController() {
            ctService = new ContentCustomTemplateService();
        }

        public void Bind( ContentSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = base.getBlock( "list" );

            int i = 1;
            foreach (IBinderValue item in serviceData) {

                BinderUtils.bindMashupData( block, item, i );
                block.Next();
                i++;
            }
        }


    }
}
