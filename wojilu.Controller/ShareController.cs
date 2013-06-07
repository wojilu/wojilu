/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common;
using wojilu.Members.Users.Domain;
using wojilu.Drawing;
using wojilu.Common.Microblogs;

namespace wojilu.Web.Controller {

    public class ShareController : ControllerBase {

        [Login]
        public void Add() {

            // 使用owner  避免二级域名跨域
            set( "ActionLink", Link.To( ctx.owner.obj, new Microblogs.MicroblogSaveController().Create ) );

            set( "mbTotalCount", MicroblogAppSetting.Instance.MicroblogContentMax );

            String title = strUtil.CutString( ctx.Get( "title" ), 150 );
            String url = strUtil.CutString( ctx.Get( "url" ), 150 );
            set( "mbContent", title + " " + url + " " );

            // pic
            //---------------------------------------------------
            IBlock picBlock = getBlock( "pic" );
            String pic = strUtil.CutString( ctx.Get( "pic" ), 150 );

            if (strUtil.HasText( pic )) {

                String picThumb = "";
                if (pic.IndexOf( "upload/face" ) > 0) {
                    picThumb = sys.Path.GetAvatarThumb( pic, ThumbnailType.Medium );
                }
                else {
                    picThumb = sys.Path.GetPhotoThumb( pic, ThumbnailType.Small );
                }

                picBlock.Set( "picThumbSrc", picThumb );
                picBlock.Set( "picSrc", pic );

                picBlock.Next();
            }

        }

        [Login, HttpPost, DbTransaction]
        public void Save() {


        }


    }

}
