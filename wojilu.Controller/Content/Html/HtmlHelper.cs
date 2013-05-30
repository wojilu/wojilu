/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class HtmlHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlHelper ) );

        public static void SetPostToContext( MvcContext ctx, ContentPost post ) {
            ctx.SetItem( "_currentContentPost", post );
        }

        public static ContentPost GetPostFromContext( MvcContext ctx ) {
            return ctx.GetItem( "_currentContentPost" ) as ContentPost;
        }

        public static void SetPostListToContext( MvcContext ctx, List<ContentPost> posts ) {
            ctx.SetItem( "_currentContentPostList", posts );
        }

        public static List<ContentPost> GetPostListFromContext( MvcContext ctx ) {
            return ctx.GetItem( "_currentContentPostList" ) as List<ContentPost>;
        }

        public static Boolean IsMakeHtml( MvcContext ctx ) {
            if (ctx.GetItem( "_makeHtml" ) == null) return false;
            Boolean isMakeHtml = (Boolean)ctx.GetItem( "_makeHtml" );
            return isMakeHtml;
        }

        public static Boolean CanHtml( MvcContext ctx ) {

            if (ctx.owner == null || ctx.owner.obj == null) return false;

            // 只限网站使用
            if (ctx.owner.obj.GetType() != typeof( Site )) return false;

            // 是否需要自动生成
            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting setting = app.GetSettingsObj();
            return setting.IsAutoHtml == 1;
        }

        public static bool IsHtmlDirError( String htmlDir, Result errors ) {

            if (strUtil.HasText( htmlDir )) {


                if (htmlDir.Length > 50) {
                    errors.Add( "目录名称不能超过50个字符" );
                    return true;
                }

                if (isReservedKeyContains( htmlDir )) {
                    errors.Add( "目录名称是保留词，请换一个" );
                    return true;
                }

                if (isHtmlDirUsed( htmlDir )) {
                    errors.Add( "目录名称已被使用，请换一个" );
                    return true;
                }

            }

            return false;
        }

        private static bool isHtmlDirUsed( string dirName ) {

            List<ContentApp> appList = ContentApp.find( "OwnerType=:otype" )
                .set( "otype", typeof( Site ).FullName )
                .list();

            foreach (ContentApp app in appList) {

                if (dirName.Equals( app.GetSettingsObj().StaticPath )) return true;
            }

            return false;
        }

        private static bool isReservedKeyContains( string dirName ) {

            if (dirName == null) return false;

            String[] arrKeys = new String[] { "framework", "bin", "html", "static" };

            return new List<String>( arrKeys ).Contains( dirName.ToLower() );
        }


    }

}
