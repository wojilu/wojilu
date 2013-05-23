/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Section;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Common.Installers {

    public class NewsInstaller : CmsInstallerBase, IAppInstaller {

        private static readonly ILog logger = LogManager.GetLogger( typeof( NewsInstaller ) );

        private XApp xapp;

        protected override void initTheme() {

            if (ContentTheme.IdHasError( base.themeId )) {
                logger.Error( "theme id error, id=" + base.themeId );
                return;
            }

            this.xapp = ContentTheme.GetByThemeId( base.themeId );

            if (this.xapp == null) throw new Exception( "没有有效的主题数据, themeId=" + base.themeId );
        }

        protected override void createLayout() {

            app.Style = xapp.Style;
            app.Layout = xapp.Layout;
            app.Style = xapp.Style;
            app.SkinStyle = xapp.SkinStyle;

            this.app.update();
        }

        protected override IMemberApp createPortalApp() {

            // 1、创建app
            IMemberApp ret = base.createApp();

            // 2、创建section
            foreach (XSection section in xapp.SectionList) {

                ContentSection s;

                // 聚合区块
                if (section.ServiceId > 0) {

                    s = createSectionMashup( section.Name, section.ServiceId, section.TemplateId, section.LayoutStr );
                    s.CssClass = section.CssClass;
                    s.ListCount = section.ListCount;
                    s.ServiceParams = section.ServiceParams;
                    s.update();

                    // 添加内容区块
                } else {

                    s = createSection( section.Name, section.TypeFullName, section.LayoutStr );
                    s.CssClass = section.CssClass;
                    s.ListCount = section.ListCount;
                    s.update();

                    for (int i = section.PostList.Count - 1; i >= 0; i--) {

                        XPost post = section.PostList[i];

                        ContentPost newPost = createPost( s, post.Title, post.TitleHome, post.Content, post.Pic, post.SourceLink, post.CategoryId, post.Width, post.Height );

                        if (strUtil.HasText( post.Summary )) {
                            newPost.Summary = post.Summary;
                        }

                        if (post.PickStatus > 0) {
                            newPost.PickStatus = post.PickStatus;
                        }

                        if (strUtil.HasText( post.Summary ) || post.PickStatus > 0) {
                            newPost.update();
                        }
                    }

                }

                // 自定义模板
                if (strUtil.HasText( section.TemplateCustom )) {

                    ContentCustomTemplate cs = new ContentCustomTemplate();
                    cs.Content = section.TemplateCustom;
                    cs.OwnerId = this.app.OwnerId;
                    cs.OwnerType = this.app.OwnerType;
                    cs.OwnerUrl = this.app.OwnerUrl;
                    cs.Name = "" + DateTime.Now.ToShortDateString();
                    cs.Creator = ctx.viewer.obj as User;
                    tplService.Insert( cs );

                    s.CustomTemplateId = cs.Id;
                    s.update( "CustomTemplateId" );
                }

            }

            return ret;
        }




    }

}
