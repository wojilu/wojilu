using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Section;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.SOA;
using wojilu.DI;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class TemplateController : ControllerBase {

        public IShopSectionTypeService sectionTypeService { get; set; }
        public IShopSectionService sectionService { get; set; }
        public IShopSectionTemplateService templateService { get; set; }

        public TemplateController() {
            sectionTypeService = new ShopSectionTypeService();
            sectionService = new ShopSectionService();
            templateService = new ShopSectionTemplateService();
        }

        public static String getSectionTypeName() {
            return typeof( ListController ).FullName;
        }

        public void Select( int id ) {

            ShopSection section = sectionService.GetById( id, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "customLink", to( new TemplateCustomController().EditBinder, id ) );

            Service service = ServiceContext.Get( section.ServiceId );
            ShopSectionTemplate tpl = templateService.GetById( section.TemplateId );

            String templateType = TemplateUtil.getServiceTemplates( service.Note, tpl, ctx, templateService );
            set( "templateType", templateType );

            target( UpdateTemplate, id );
        }

        [HttpPost, DbTransaction]
        public void UpdateTemplate( int id ) {

            ShopSection section = sectionService.GetById( id, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            int tplId = section.TemplateId;

            int templateId = ctx.PostInt( "templateId" );

            if (templateId != tplId) {
                section.TemplateId = templateId;
                section.CustomTemplateId = 0; // 切换模板之后，不再使用自定义模板
                sectionService.Update( section );

            }
            echoToParentPart(lang("opok"));

        }

        public void List() {

            DataPage<ShopSectionType> list = sectionTypeService.GetPage();
            String thumbPath = BinderUtils.GetSectionTemplateThumbPath();
            String sectionTypeName = getSectionTypeName();

            IBlock block = getBlock( "list" );
            foreach (ShopSectionType c in list.Results) {

                String thumb = strUtil.Join( thumbPath, c.TypeFullName ) + ".png";

                block.Set( "c.Name", c.Name );
                block.Set( "c.Thumb", thumb );
                block.Set( "c.TypeFullName", c.TypeFullName );

                String strChecked = "";
                if (sectionTypeName.Equals( c.TypeFullName )) strChecked = " checked ";
                block.Set( "c.Checked", strChecked );

                block.Next();
            }

            set( "page", list.PageBar );
        }


    }

}
