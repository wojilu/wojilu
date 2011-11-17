using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Utils;


namespace wojilu.Web.Controller.Content.Binder {


    public class FocusBinderController : ControllerBase, ISectionBinder {

        public void Bind( ContentSection section, IList serviceData ) {

            IBlock fblock = getBlock( "focus" );
            IBlock block = getBlock( "list" );
            if (serviceData.Count == 0) return;

            bindFocus( fblock, (ContentPost)serviceData[0] ); // 第一篇是焦点要闻
            bindPickedList( serviceData, block );
        }

        private void bindFocus( IBlock fblock, ContentPost article ) {
            fblock.Set( "article.Title", strUtil.SubString( article.Title, 19 ) );
            fblock.Set( "article.SummaryInfo", strUtil.CutString( article.Summary, 100 ) );
            fblock.Set( "article.Url", alink.ToAppData( article ) );
            fblock.Next();
        }

        private void bindPickedList( IList serviceData, IBlock block ) {
            for (int i = 1; i < serviceData.Count; i++) {
                ContentPost a = serviceData[i] as ContentPost;

                String typeIcon = BinderUtils.getTypeIcon( a );
                String attIcon = a.Attachments > 0 ? BinderUtils.iconAttachment : "";

                block.Set( "post.Title", a.Title );
                block.Set( "post.Url", alink.ToAppData( a ) );
                block.Set( "post.DataIcon", typeIcon );
                block.Set( "post.AttachmentIcon", attIcon );
                block.Set( "post.Created", a.Created.ToShortDateString() );
                block.Next();
            }
        }


    }

}
