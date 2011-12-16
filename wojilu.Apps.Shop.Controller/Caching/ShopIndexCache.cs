using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Interface;
using wojilu.Caching;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Shop.Caching {

    public class ContentIndexPageCache : PageCache {

        public override List<Type> GetRelatedActions() {
            List<Type> list = new List<Type>();
            list.Add( typeof( ShopIndexCache ) );
            list.Add( typeof( SiteLayoutCache ) );
            return list;
        }

        public override void UpdateCache( MvcContext ctx ) {

            IApp app = ctx.app.obj as IApp;

            if (app != null) {
                base.updateAllUrl( alink.ToApp( app, ctx ), ctx );
            }
            else {

                List<ShopApp> apps = ShopApp.find( "OwnerId=" + Site.Instance.Id + " and OwnerType=:otype" )
                    .set( "otype", typeof( Site ).FullName )
                    .list();

                foreach (ShopApp a in apps) {
                    base.updateAllUrl( alink.ToApp( a, ctx ), ctx );
                }

            }


        }


    }

    public class ShopIndexCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Shop.ShopController ).FullName + "_" + appId;
        }

        public override Dictionary<Type, String> GetRelatedActions() {

            Dictionary<Type, String> dic = new Dictionary<Type, String>();


            dic.Add( typeof( Admin.ShopController ), "AddRow/DeleteRow/SaveLayout/SaveResize/SaveStyle" );
            dic.Add( typeof( Admin.ShopSectionController ), "Create/CreateAuto/CreateFeed/Delete/SaveRowUI/SaveUI/SaveSectionUI/SaveSectionTitleUI/SaveSectionShopUI/SaveCombine/RemoveSection/SaveEffect" );

            dic.Add( typeof( Admin.SectionSettingController ), "Update/SaveCount/UpdateBinder" );
            dic.Add( typeof( Admin.SettingController ), "Save" );
            dic.Add( typeof( Admin.SkinController ), "Apply" );

            dic.Add( typeof( Admin.TemplateController ), "UpdateTemplate" );
            dic.Add( typeof( Admin.TemplateCustomController ), "Save/Reset" );

            Dictionary<Type, String> postDic = getPostAdminActions();
            foreach (KeyValuePair<Type, String> kv in postDic) dic.Add( kv.Key, kv.Value );

            return dic;
        }

        internal static Dictionary<Type, String> getPostAdminActions() {

            Dictionary<Type, String> dic = new Dictionary<Type, String>();
            dic.Add( typeof( Admin.ItemController ), "Delete/DeleteTrue/SaveAdmin/Restore" );
            dic.Add(typeof(Admin.Section.ListController), "Create/Update/Delete");
            //dic.Add(typeof(Admin.Section.TalkController), "Create/Update/Delete");
            //dic.Add( typeof( Admin.Section.TextController ), "Create/Update/Delete" );
            //dic.Add( typeof( Admin.Section.VideoController ), "Create/Update/Delete" );
            //dic.Add( typeof( Admin.Section.VideoShowController ), "Create/Update/Delete" );
            //dic.Add( typeof( Admin.Section.ImgController ), "CreateListInfo/CreateImgList/SetLogo/UpdateListInfo/Delete/DeleteImg" );
            //dic.Add( typeof( Admin.Section.PollController ), "Create/Delete" );

            return dic;
        }



        public override void UpdateCache( MvcContext ctx ) {

            IMember owner = ctx.owner.obj;
            int appId = ctx.app.Id;

            String key = GetCacheKey( ctx, null );

            String content = getIndexCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );
        }

        private static String getIndexCache( int appId, IMember owner ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( ShopApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Shop.ShopController().Index );

            return content;
        }

    }



}
