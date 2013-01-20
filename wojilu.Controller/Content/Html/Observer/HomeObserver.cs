/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class HomeObserver : ActionObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HomeObserver ) );

        public override void ObserveActions() {

            Admin.ContentController ac = new wojilu.Web.Controller.Content.Admin.ContentController();
            observe( ac.SaveLayout );
            observe( ac.SaveResize );
            observe( ac.SaveStyle );

            Admin.ContentSectionController cs = new wojilu.Web.Controller.Content.Admin.ContentSectionController();
            observe( cs.Create );
            observe( cs.CreateAuto );
            observe( cs.CreateFeed );
            observe( cs.SaveRowUI );
            observe( cs.SaveUI );
            observe( cs.SaveSectionUI );
            observe( cs.Delete );
            observe( cs.SaveSectionTitleUI );
            observe( cs.SaveSectionContentUI );
            observe( cs.SaveCombine );
            observe( cs.RemoveSection );
            observe( cs.SaveEffect );

            Admin.SectionSettingController ss = new wojilu.Web.Controller.Content.Admin.SectionSettingController();
            observe( ss.Update );
            observe( ss.SaveCount );
            observe( ss.UpdateBinder );

            Admin.SettingController sc = new wojilu.Web.Controller.Content.Admin.SettingController();
            observe( sc.Save );

            Admin.SkinController sk = new wojilu.Web.Controller.Content.Admin.SkinController();
            observe( sk.Apply );

            Admin.TemplateController tpl = new wojilu.Web.Controller.Content.Admin.TemplateController();
            observe( tpl.UpdateTemplate );

            Admin.TemplateCustomController tpc = new wojilu.Web.Controller.Content.Admin.TemplateCustomController();
            observe( tpc.Save );
            observe( tpc.Reset );

            Admin.RowController row = new wojilu.Web.Controller.Content.Admin.RowController();
            observe( row.AddRow );
            observe( row.Move );
            observe( row.DeleteRow );

        }

        public override void AfterAction( MvcContext ctx ) {

            if (!HtmlHelper.CanHtml( ctx )) return;

            HtmlMaker.GetHome().Process( ctx.app.Id );
            logger.Info( "HomeObserver make app home" );
        }




    }

}
