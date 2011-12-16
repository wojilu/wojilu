using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Controller.Shop.Utils;


namespace wojilu.Web.Controller.Shop.Binder {


    public class FocusBinderController : ControllerBase, ISectionBinder {

        public void Bind( ShopSection section, IList serviceData ) {

            IBlock fblock = getBlock( "focus" );
            IBlock block = getBlock( "list" );
            if (serviceData.Count == 0) return;

            bindFocus(fblock, (ContentPost)serviceData[0]); // 第一篇是焦点要闻
            bindPickedList( serviceData, block );
        }

        private void bindFocus(IBlock fblock, ContentPost article)
        {
            fblock.Set("binder.Title", strUtil.SubString(article.Title, 19));
            fblock.Set("binder.SummaryInfo", strUtil.CutString(article.Summary, 100));
            fblock.Set("binder.Url", alink.ToAppData(article));
            fblock.Next();
        }

        private void bindPickedList( IList serviceData, IBlock block ) {
            for (int i = 1; i < serviceData.Count; i++) {
                ContentPost a = serviceData[i] as ContentPost;

                String typeIcon = Content.Utils.BinderUtils.getTypeIcon( a );
                String attIcon = a.Attachments > 0 ? Content.Utils.BinderUtils.iconAttachment : "";

                block.Set("binder.Title", a.Title);
                block.Set("binder.Url", alink.ToAppData(a));
                block.Set("binder.DataIcon", typeIcon);
                block.Set("binder.AttachmentIcon", attIcon);
                block.Set("binder.Created", a.Created.ToShortDateString());
                block.Next();
            }
        }


    }

}
