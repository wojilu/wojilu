using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Web.Controller.Photo;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Domain;
using wojilu.Drawing;
using wojilu.Members.Users.Domain;
using System.IO;

namespace wojilu.Web.Controller {

    public class WaterfallInstaller : BaseInstaller {

        private static readonly ILog logger = LogManager.GetLogger( typeof( WaterfallInstaller ) );

        public IPhotoPostService postService { get; set; }

        public void Init( MvcContext ctx ) {

            base.AddMenuToHome( ctx, PhotoLink.ToHome(), "首页" );

            // 初始化基本效果图片
            addPhotoPosts( ctx );

        }

        private void addPhotoPosts( MvcContext ctx ) {


            User creator = ctx.viewer.obj as User;

            for (int i = 0; i < 20; i++) {

                PhotoPost x = new PhotoPost();
                x.DataUrl = Img.CopyToUploadPath( "/__installer/pic/pic" + i + ".jpg" );
                x.Creator = creator;
                x.CreatorUrl = creator.Url;
                x.OwnerId = creator.Id;
                x.OwnerType = creator.GetType().FullName;
                x.OwnerUrl = creator.Url;

                x.Title = Path.GetFileName( x.DataUrl );

                x.insert();

            }

        }




    }

}
