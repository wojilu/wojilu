/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {


    public partial class VideoShowController : ControllerBase, IPageAdminSection {


        private void bindSectionShow( int sectionId, ContentPost video ) {

            int vWidth = 320;
            int vHeight = 240;

            set( "addUrl", to( new VideoController().Add, sectionId ) + "?width=" + vWidth + "&height=" + vHeight );
            set( "listUrl", to( new VideoController().AdminList, sectionId ) );

            if (video == null) {
                set( "post.Content", "" );
                set( "post.Title", "" );
            }
            else {
                String content = WebHelper.GetFlash( video.SourceLink, video.Width, video.Height );
                set( "post.Content", content );

                if (strUtil.HasText( video.TitleHome ))
                    set( "post.Title", video.TitleHome );
                else
                    set( "post.Title", video.Title );
            }

            IBlock editlinkBlock = getBlock( "editlink" );
            if (video != null) {
                editlinkBlock.Set( "post.EditUrl", to( new VideoController().Edit, video.Id ) );
                editlinkBlock.Next();
            }
        }

    }
}

