/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;

using wojilu.Common.Skins;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Pages.Service;
using wojilu.Common.Pages.Domain;
using wojilu.Common.Feeds.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Pages.Interface;
using wojilu.Common.Menus;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller {

    public class SiteInitController : ControllerBase {

        public IUserService userService { get; set; }
        public SkinService skinService { get; set; }
        public IFeedService feedService { get; set; }
        public IPageService pageService { get; set; }

        public SiteInitController() {

            skinService = new SkinService();
            skinService.SetSkin( new SpaceSkin() );

            feedService = new FeedService();
            pageService = new PageService();
        }

        public void Index() {

            if (isInit() == false) {
                String view = loadHtml( view1InitData );
                actionContent( view );
            }
            else if (hasMember() == false) {
                String view = loadHtml( view2Register );
                actionContent( view );
            }
            else {
                String view = loadHtml( view3Initok );
                actionContent( view );
            }
        }

        public void SelectDb() {
            set( "link", to( Init ) );
        }

        [NonVisit]
        public void view1InitData() {
            set( "link", to( Init ) );
            set( "selectDbLink", to( SelectDb ) );
        }

        [NonVisit]
        public void view2Register() {
            set( "link", to( new RegisterController().Register ) );
        }

        [NonVisit]
        public void view3Initok() {
        }

        private Boolean hasMember() {
            return User.count() > 0;
        }

        [HttpPut, DbTransaction]
        public void Init() {

            Boolean isInit = false;
            if (skinService.GetSkinCount() <= 0) {
                initSkin();
                isInit = true;
            }

            if (feedService.GetTemplateBundleCount() <= 0) {
                initTemplateBundle();
                isInit = true;
            }

            if (pageService.GetPagesCount( Site.Instance ) <= 0) {
                initSiteFooter();
                isInit = true;
            }

            initPhotoCategory();
            initGroupCategory();
            initForumRole();
            initContentSkin();

            if (isInit) {
                echoRedirect( "初始化成功", Index );
            }
            else
                echoRedirect( "系统已经初始化过", Index );

        }


        private void initForumRole() {
            ForumRole role = ForumRole.findById( 1 );
            if (role != null) return;
            role = new ForumRole();
            role.Name = "版主";
            role.insert();
        }


        private void initContentSkin() {

            ContentSkin s1 = new ContentSkin();
            s1.Name = "默认";
            s1.StylePath = "/apps/content/1/skin.css";
            s1.ThumbUrl = "/apps/content/1/skinThumb.jpg";
            s1.insert();

            ContentSkin s2 = new ContentSkin();
            s2.Name = "蓝色";
            s2.StylePath = "/apps/content/2/skin.css";
            s2.ThumbUrl = "/apps/content/2/skinThumb.jpg";
            s2.insert();

            ContentSkin s3 = new ContentSkin();
            s3.Name = "绿色";
            s3.StylePath = "/apps/content/3/skin.css";
            s3.ThumbUrl = "/apps/content/3/skinThumb.jpg";
            s3.insert();

        }

        private void initGroupCategory() {

            new GroupCategory( "阅读、学习" ).insert();
            new GroupCategory( "娱乐" ).insert();
            new GroupCategory( "生活、情感" ).insert();
            new GroupCategory( "非主流、另类" ).insert();
            new GroupCategory( "计算机、数码、硬件" ).insert();
            new GroupCategory( "互联网" ).insert();
            new GroupCategory( "编程、软件" ).insert();
            new GroupCategory( "自然与科学" ).insert();
            new GroupCategory( "人物" ).insert();
            new GroupCategory( "地区" ).insert();
            new GroupCategory( "行业" ).insert();

            new GroupCategory( "文化与社会" ).insert();
        }

        private void initPhotoCategory() {

            new PhotoSysCategory( "人物" ).insert();
            new PhotoSysCategory( "风景" ).insert();
            new PhotoSysCategory( "动物" ).insert();
            new PhotoSysCategory( "人文" ).insert();

        }

        private Boolean isInit() {
            if (skinService.GetSkinCount() <= 0) return false;
            if (feedService.GetTemplateBundleCount() <= 0) return false;
            return true;
        }

        private void initSkin() {

            new SpaceSkin( "书写", "1/skin.css", "1/thumb.jpg" ).insert();
            new SpaceSkin( "花与墙", "2/skin.css", "2/thumb.jpg" ).insert();
            new SpaceSkin( "山水相映", "3/skin.css", "3/thumb.jpg" ).insert();
            new SpaceSkin( "灯火夜晚", "4/skin.css", "4/thumb.jpg" ).insert();
            new SpaceSkin( "水中贝壳", "5/skin.css", "5/thumb.jpg" ).insert();
            new SpaceSkin( "清新居室", "6/skin.css", "6/thumb.jpg" ).insert();
            new SpaceSkin( "可爱卡通", "7/skin.css", "7/thumb.jpg" ).insert();
            new SpaceSkin( "绚烂", "8/skin.css", "8/thumb.jpg" ).insert();
            new SpaceSkin( "大地", "9/skin.css", "9/thumb.jpg" ).insert();
            new SpaceSkin( "雾中风景", "10/skin.css", "10/thumb.jpg" ).insert();
            new SpaceSkin( "林中路", "11/skin.css", "11/thumb.jpg" ).insert();
            new SpaceSkin( "草地", "12/skin.css", "12/thumb.jpg" ).insert();
            new SpaceSkin( "迷宫", "13/skin.css", "13/thumb.jpg" ).insert();
            new SpaceSkin( "黑云压城", "14/skin.css", "14/thumb.jpg" ).insert();
            new SpaceSkin( "青青生长", "15/skin.css", "15/thumb.jpg" ).insert();
            new SpaceSkin( "童年相伴", "16/skin.css", "16/thumb.jpg" ).insert();
            new SpaceSkin( "无题", "17/skin.css", "17/thumb.jpg" ).insert();
            new SpaceSkin( "座次", "18/skin.css", "18/thumb.jpg" ).insert();
            new SpaceSkin( "阳光", "19/skin.css", "19/thumb.jpg" ).insert();
            new SpaceSkin( "黑板", "20/skin.css", "20/thumb.jpg" ).insert();
            new SpaceSkin( "色彩", "21/skin.css", "21/thumb.jpg" ).insert();
            new SpaceSkin( "大海", "22/skin.css", "22/thumb.jpg" ).insert();
            new SpaceSkin( "迎风起舞", "23/skin.css", "23/thumb.jpg" ).insert();
            new SpaceSkin( "远方", "24/skin.css", "24/thumb.jpg" ).insert();
            new SpaceSkin( "五彩之笔", "25/skin.css", "25/thumb.jpg" ).insert();
            new SpaceSkin( "蓝天", "26/skin.css", "26/thumb.jpg" ).insert();
            new SpaceSkin( "运算", "27/skin.css", "27/thumb.jpg" ).insert();
            new SpaceSkin( "草原与树", "28/skin.css", "28/thumb.jpg" ).insert();
            new SpaceSkin( "脚印", "29/skin.css", "29/thumb.jpg" ).insert();
            new SpaceSkin( "格子", "30/skin.css", "30/thumb.jpg" ).insert();


            new GroupSkin( "默认模板", "1/skin.css", "1/thumb.jpg" ).insert();
        }

        private void initTemplateBundle() {

            // 1
            TemplateBundle tpl1 = new TemplateBundle();
            tpl1.OneLineStoryTemplatesStr = "[{ Title: \"{*actor*} 发表了新日志 {*blog*}\" }]";
            db.insert( tpl1 );

            // 2
            TemplateBundle tt = new TemplateBundle();
            tt.insert();
            tt.delete();

            // 3
            TemplateBundle tpl3 = new TemplateBundle();
            tpl3.ShortStoryTemplatesStr = "[{ Title: \"{*actor*} 上传了{*photoCount*}张 新照片。\", Body: \"{*photos*}\"  }]";
            db.insert( tpl3 );

            // 4
            tt.insert();
            tt.delete();


            // 5
            TemplateBundle tpl5 = new TemplateBundle();
            tpl5.OneLineStoryTemplatesStr = "[{ Title: \"{*actor*} 发表了论坛主题 {*topic*}\" }]";
            db.insert( tpl5 );

            // 6
            TemplateBundle tpl6 = new TemplateBundle();
            tpl6.OneLineStoryTemplatesStr = "[{ Title: \"{*actor*} 发表了论坛帖子 {*post*}\" }]";
            db.insert( tpl6 );

            // 7
            tt.insert();
            tt.delete();

            // 8
            tt.insert();
            tt.delete();


            TemplateBundle tpl9 = new TemplateBundle();
            tpl9.OneLineStoryTemplatesStr = "[{ Title: \"{*actor*} 和 {*friend*} 成为了好友\" }]";
            db.insert( tpl9 );

        }

        private void initSiteFooter() {
            PageCategory category1 = createCategory( "网站信息" );
            PageCategory category2 = createCategory( "帮助中心" );

            createPage( "网站简介", category1, true );
            createPage( "联系方式", category1, true );
            createPage( "隐私保护", category1, true );
            createPage( "如何使用帮助", category2, false );
        }

        private PageCategory createCategory( String name ) {
            PageCategory category = new PageCategory();
            category.Creator = new User( 1 );
            category.OwnerType = typeof( Site ).FullName;
            category.OwnerUrl = "/";
            category.Name = name;
            db.insert( category );
            return category;
        }

        private void createPage( String title, PageCategory category, Boolean isFooter ) {

            Page page = new Page();
            page.Creator = new User( 1 );
            page.OwnerType = typeof( Site ).FullName;
            page.OwnerUrl = "/";
            page.Title = title;
            page.Content = title;
            page.Category = category;
            page.EditReason = "创建页面";

            pageService.Insert( page );

            // FooterMenu
            if (isFooter == false) return;

            FooterMenu fm = new FooterMenu();
            fm.Name = page.Name;
            fm.Link = to( new Common.PageController().Show, page.Id );
            fm.insert();

        }


    }
}

