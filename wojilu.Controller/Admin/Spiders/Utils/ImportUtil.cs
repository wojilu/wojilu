/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Enum;
using wojilu.Data;
using wojilu.Apps.Content.Service;
using wojilu.DI;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Admin.Spiders {


    public class ImportUtil {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ImportUtil ) );

        public static void BeginImport( object param ) {

            ImportState ts = param as ImportState;

            try {
                List<int> ids = beginImportPrivate( ts );
                JobManager.ImportPost( ids );
            }
            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
                ts.Log.AppendLine( ex.Message );
                ts.Log.AppendLine( ex.StackTrace );

                DbContext.closeConnectionAll();

            }
        }

        private static List<int> beginImportPrivate( ImportState ts ) {

            int id = ts.TemplateId;

            SpiderImport item = SpiderImport.findById( id );

            List<SpiderArticle> articles = SpiderArticle
                .find( "SpiderTemplateId in (" + item.DataSourceIds + ") and Id>" + item.LastImportId + " order by Id" )
                .list();

            List<ContentSection> sections = ContentSection.find( "Id in (" + item.SectionIds + ")" ).list();
            if (sections.Count == 0) throw new Exception( "导入的目标section不存在" );

            ContentSection section = null;
            List<int> results = new List<int>();
            for (int i = 0; i < articles.Count; i++) {

                if (articleExist( articles[i] )) {
                    ts.Log.AppendLine( "pass..." + articles[i].Title );
                    continue;
                }

                section = getNextSection( sections, section ); // 均匀分散到各目标section中
                ContentApp app = getApp( section );

                if (item.IsApprove == 1) {
                    importToTemp( articles[i], item, section, app );
                }
                else {
                    int newArticleId = importDirect( articles[i], item, section, app );
                    results.Add( newArticleId );
                }

                ts.Log.AppendLine( "导入：" + articles[i].Title );

            }

            if (articles.Count > 0) {
                item.LastImportId = articles[articles.Count - 1].Id;
                item.update( "LastImportId" );
                ts.Log.AppendLine( "导入完毕(操作结束)" );
            }
            else {
                ts.Log.AppendLine( "没有新条目可导入(操作结束)" );
            }


            return results;
        }

        private static bool articleExist( SpiderArticle spiderArticle ) {
            logger.Info( "articleExist=" + spiderArticle.Url );
            ContentPost post = ContentPost.find( "OutUrl=:url" ).set( "url", spiderArticle.Url ).first();
            if (post != null) return true;
            return ContentTempPost.find( "SourceLink=:url" ).set( "url", spiderArticle.Url ).first() != null;
        }


        private static int importToTemp( SpiderArticle art, SpiderImport item, ContentSection section, ContentApp app ) {

            ContentTempPost post = new ContentTempPost();
            post.Creator = item.Creator;
            post.OwnerId = app.OwnerId;
            post.OwnerType = app.OwnerType;
            post.AppId = app.Id;
            post.SectionId = section.Id;

            post.Title = art.Title;
            post.SourceLink = art.Url;
            post.Content = art.Body;

            post.insert();

            return post.Id;
        }

        private static int importDirect( SpiderArticle art, SpiderImport item, ContentSection section, ContentApp app ) {

            ContentPostService postService = ObjectContext.Create<ContentPostService>();

            ContentPost post = new ContentPost();
            post.Title = art.Title;
            post.Content = art.Body;

            if (art.IsPic == 1) {
                post.CategoryId = PostCategory.Img;
                post.ImgLink = art.PicUrl;
            }

            post.SourceLink = art.Url;

            post.Creator = item.Creator;
            post.CreatorUrl = item.Creator.Url;
            post.PageSection = section;
            post.OwnerId = app.OwnerId;
            post.OwnerType = app.OwnerType;
            post.OwnerUrl = app.OwnerUrl;
            post.AppId = app.Id;

            postService.Insert( post, "" );

            return post.Id;
        }

        private static ContentSection getNextSection( List<ContentSection> sections, ContentSection section ) {

            if (section == null) return sections[0];
            for (int i = 0; i < sections.Count; i++) {

                if (sections[i].Id == section.Id) {
                    if (i == sections.Count - 1)
                        return sections[0];
                    else
                        return sections[i + 1];
                }

            }
            return null;
        }

        private static ContentApp getApp( ContentSection section ) {
            return ContentApp.findById( section.AppId );
        }
    }

}
