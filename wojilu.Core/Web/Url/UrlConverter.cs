/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using System;
using wojilu.Web.Mvc;
using wojilu.Web.Context;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Interface;
using wojilu.Common;

namespace wojilu.Web.Url {

    public class UrlConverter {

        public static String toMenu( IMenu menu, MvcContext ctx ) {

            if ("default".Equals( menu.Url )) {
                return Link.ToMember( menu.OwnerType, menu.OwnerUrl );
            }

            return UrlConverter.getMenuUrl( menu, ctx );
        }

        /// <summary>
        /// member的首页按member的方式生成；有别名的按别名生成简短网址；其余生成原始网址
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static String getMenuUrl( IMenu menu, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( menu.Url )) {
                return getMenuFullUrl( menu, ctx );
            }

            if ("default".Equals( menu.Url )) return Link.ToMember( menu.OwnerType, menu.OwnerUrl );


            String ownerPathAndUrl = getMemberPathUrlByMenu( menu );
            ownerPathAndUrl = strUtil.Join( ctx.url.AppPath, ownerPathAndUrl );

            String result = strUtil.Join( ownerPathAndUrl, menu.Url );
            return result + MvcConfig.Instance.UrlExt;
        }

        public static String getMemberPathUrlByMenu( IMenu menu ) {
            if (menu.OwnerType.Equals( ConstString.SiteTypeFullName )) return "";
            return strUtil.Join( MemberPath.GetPath( strUtil.GetTypeName( menu.OwnerType ) ), menu.OwnerUrl );
        }

        //------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// 完整路径，包括http、域名、后缀名（即得到别名url的原始网址）
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static String getMenuFullUrl( IMenu menu, MvcContext ctx ) {

            String ownerPath = LinkHelper.GetMemberPathPrefix( menu.OwnerType, menu.OwnerUrl );

            return getFullUrl( menu.RawUrl, ownerPath, ctx );
        }


        private static String getFullUrl( String url, String ownerPathAndUrl, MvcContext ctx ) {



            // 包括http是完整的url
            Boolean isFullUrl = url.ToLower().StartsWith( "http:" ) || url.ToLower().StartsWith( "https:" );
            if (isFullUrl) return url;

            // 包括完整的ownerPath
            if (url.StartsWith( "/" ) || url.StartsWith( "t/" )) {
                return strUtil.Join( ctx.url.SiteAndAppPath, url ) + MvcConfig.Instance.UrlExt;
            }

            String result = url;

            //-------------------------- trimStart ----------------------------------------------------------

            if (url.StartsWith( ctx.url.AppPath )) {
                result = strUtil.TrimStart( url, ctx.url.AppPath );
            }

            if (result.StartsWith( ownerPathAndUrl )) {
                result = strUtil.TrimStart( result, ownerPathAndUrl );
            }

            if (result.StartsWith( "/" + ownerPathAndUrl ) && strUtil.HasText( ownerPathAndUrl )) {
                result = strUtil.TrimStart( result, "/" + ownerPathAndUrl );
            }

            //------------------------------------------------------------------------------------
            if (ownerPathAndUrl.StartsWith( "http" ) == false) {
                String location = strUtil.Join( ctx.url.SiteAndAppPath, ownerPathAndUrl );
                result = strUtil.Join( location, result );
            }
            else {
                result = strUtil.Join( ownerPathAndUrl, result );
            }

            if (PathHelper.UrlHasExt( result )) {
                return result;
            }
            else if (result.EndsWith( "/" )) {
                return result;
            }
            else {
                return result + MvcConfig.Instance.UrlExt;
            }
        }


        /// <summary>
        /// 不包括http、域名、application path和网址后缀(仅仅是path)
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static String getMenuFullPath( MvcContext ctx, IMenu menu ) {

            String url = menu.RawUrl;

            // 增加微博的特殊处理
            if (url != null && url.StartsWith( "t" + MvcConfig.Instance.UrlSeparator )) {
                url = strUtil.Join( "", url, MvcConfig.Instance.UrlSeparator );
            }

            String ownerPathAndUrl = getMemberPathUrlByMenu( menu );

            // 带http的完整网址
            Boolean isFullUrl = PathHelper.IsFullUrl( url );
            if (isFullUrl) return url;

            // 包括完整的ownerPath
            if (url.StartsWith( "/" )) {
                return url + MvcConfig.Instance.UrlExt;
            }

            String result = url;
            if (url.StartsWith( ctx.url.AppPath )) {
                result = strUtil.TrimStart( url, ctx.url.AppPath );
            }

            if (result.StartsWith( ownerPathAndUrl )) {
                result = strUtil.TrimStart( result, ownerPathAndUrl );
            }
            if (result.StartsWith( "/" + ownerPathAndUrl ) && strUtil.HasText( ownerPathAndUrl )) {
                result = strUtil.TrimStart( result, "/" + ownerPathAndUrl );
            }

            String location = strUtil.Join( ctx.url.AppPath, ownerPathAndUrl );
            result = strUtil.Join( location, result );
            return result;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------


        public static String clearUrl( IMemberApp app, MvcContext ctx, IMember owner ) {

            String appUrl = alink.ToUserAppFull( app );

            String ownerPath = LinkHelper.GetMemberPathPrefix( owner.GetType().FullName, owner.Url );
            ownerPath = ownerPath.TrimStart( '/' );


            return clearUrl( appUrl, ownerPath, ctx );
        }

        public static String clearUrl( IMemberApp app, MvcContext ctx ) {

            String appUrl = alink.ToUserAppFull( app );

            IMember owner = ctx.owner.obj;

            String ownerPath = LinkHelper.GetMemberPathPrefix( owner.GetType().FullName, owner.Url );
            ownerPath = ownerPath.TrimStart( '/' );

            return clearUrl( appUrl, ownerPath, ctx );
        }

        public static String clearUrl( IAppData data, MvcContext ctx ) {

            String ownerPath = LinkHelper.GetMemberPathPrefix( data.OwnerType, data.OwnerUrl );
            ownerPath = ownerPath.TrimStart( '/' );

            return clearUrl( alink.ToAppData( data ), ownerPath, ctx );
        }

        public static String clearUrl( String rawUrl, MvcContext ctx, String memberTypeFullName, String memberUrl ) {

            String ownerPath = LinkHelper.GetMemberPathPrefix( memberTypeFullName, memberUrl );
            ownerPath = ownerPath.TrimStart( '/' );

            return clearUrl( rawUrl, ownerPath, ctx );
        }


        private static String clearUrl( String rawInputUrl, String ownerPathAndUrl, MvcContext ctx ) {

            String result;
            if (rawInputUrl.StartsWith( ctx.url.AppPath )) {
                result = strUtil.TrimStart( rawInputUrl, ctx.url.AppPath );
            }
            else {
                result = strUtil.TrimStart( rawInputUrl, ctx.url.SiteAndAppPath );
            }

            result = result.TrimStart( '/' );

            if (result.StartsWith( ownerPathAndUrl )) {
                result = strUtil.TrimStart( result, ownerPathAndUrl ); // 相对于当前owner
                result = strUtil.TrimEnd( result, MvcConfig.Instance.UrlExt );

                return strUtil.TrimStart( result, "/" );
            }
            else {
                result = strUtil.Join( "/", result ); //相对于网站根目录
                result = strUtil.TrimEnd( result, MvcConfig.Instance.UrlExt );
                return result;
            }

        }

    }

}
