using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Menus;
using wojilu.Common.Pages.Domain;
using wojilu.Common.Pages.Service;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Skins;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Pages.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Helpers {

    public class SiteInitHelper {

        public SkinService skinService { get; set; }
        public IFeedService feedService { get; set; }
        public IPageService pageService { get; set; }

        public SiteInitHelper() {
            skinService = new SkinService();
            skinService.SetSkin( new SpaceSkin() );

            feedService = new FeedService();
            pageService = new PageService();

        }

        public Boolean HasInit() {
            if (skinService.GetSkinCount() <= 0) return false;
            if (feedService.GetTemplateBundleCount() <= 0) return false;
            return true;
        }

        public Boolean InitSite() {

            Boolean isInit = false;

            backupCacheData();

            if (skinService.GetSkinCount() <= 0) {
                InitSpaceSkin();
                isInit = true;
            }

            if (feedService.GetTemplateBundleCount() <= 0) {
                InitTemplateBundle();
                isInit = true;
            }

            if (pageService.GetPagesCount( Site.Instance ) <= 0) {
                InitSiteFooter();
                isInit = true;
            }

            InitBlogCategory();
            InitPhotoCategory();
            InitGroupCategory();
            InitForumRole();
            InitContentSkin();

            return isInit;
        }

        private void backupCacheData() {
            String srcPath = PathHelper.Map( strUtil.Join( cfgHelper.FrameworkRoot, "/data/" ) );
            String destPath = PathHelper.Map( strUtil.Join( cfgHelper.FrameworkRoot, "/data_backup_" + getTimeString() + "/" ) );
            file.CopyDirectory( srcPath, destPath, true );

            // 删除不必要的数据
            String[] fileList = new String[] {
                typeof(wojilu.Common.Menus.FooterMenu).FullName,
                typeof(wojilu.Members.Sites.Domain.SiteApp).FullName,
                typeof(wojilu.Members.Sites.Domain.SiteMenu).FullName,
                typeof(wojilu.Web.Controller.Security.AppAdminRole).FullName,
                typeof(wojilu.Web.Controller.Security.AppRole).FullName
            };
            foreach (String x in fileList) {
                String filePath = PathHelper.Map( strUtil.Join( cfgHelper.FrameworkRoot, "/data/" + x + ".config" ) );
                file.Delete( filePath );
            }
        }

        private string getTimeString() {
            DateTime x = DateTime.Now;
            return string.Format( "{0}{1}{2}_{3}{4}{5}", x.Year, x.Month, x.Day, x.Hour, x.Minute, x.Second );
        }

        public void InitForumRole() {
            ForumRole role = ForumRole.findById( 1 );
            if (role != null) return;
            role = new ForumRole();
            role.Name = "版主";
            role.insert();
        }

        public void InitContentSkin() {

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

        public void InitGroupCategory() {

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

        public void InitBlogCategory() {

            new BlogSysCategory( "旅行" ).insert();
            new BlogSysCategory( "日记" ).insert();
            new BlogSysCategory( "心情" ).insert();
            new BlogSysCategory( "娱乐" ).insert();

        }

        public void InitPhotoCategory() {

            new PhotoSysCategory( "人物" ).insert();
            new PhotoSysCategory( "风景" ).insert();
            new PhotoSysCategory( "动物" ).insert();
            new PhotoSysCategory( "人文" ).insert();

        }



        public void InitSpaceSkin() {

            new SpaceSkin( "花与墙", "1/skin.css", "1/thumb.jpg" ).insert();
            new SpaceSkin( "书写", "2/skin.css", "2/thumb.jpg" ).insert();
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
            new SpaceSkin( "博客", "31/skin.css", "31/thumb.jpg" ).insert();


            new GroupSkin( "默认模板", "1/skin.css", "1/thumb.jpg" ).insert();
        }

        public void InitTemplateBundle() {

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

        public void InitSiteFooter() {

            List<FooterMenu> list = cdb.findAll<FooterMenu>();
            foreach (FooterMenu x in list) {
                x.delete();
            }

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
            fm.Link = Link.To( Site.Instance, new Common.PageController().Show, page.Id );
            fm.insert();

        }

    }
}
