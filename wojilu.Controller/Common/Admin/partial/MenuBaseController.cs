/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Web.Context;
using wojilu.Members.Interface;
using wojilu.Common.Menus.Interface;

namespace wojilu.Web.Controller.Common.Admin {

    public partial class MenuBaseController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MenuBaseController ) );

        private void bindMenus( List<IMenu> menus ) {
            IBlock block = getBlock( "list" );
            foreach (IMenu menu in menus) {

                int depth = getTree().GetDepth( menu.Id );
                int indent = 20 * depth;

                String strIndent = "<span style=\"margin-left:" + indent + "px\"></span>";
                block.Set( "d.Indent", strIndent );

                block.Set( "d.Id", menu.Id );

                block.Set( "d.Name", menu.Name );
                block.Set( "d.Style", menu.Style );

                String externalLink = menu.OpenNewWindow == 1 ? " class=\"externalLink\" " : "";
                block.Set( "d.ExternalLink", externalLink );

                block.Set( "d.Url", strUtil.CutString( menu.Url, 50 ) );
                String showUrl = UrlConverter.getMenuFullUrl( menu, ctx );
                block.Set( "d.RawUrl", strUtil.CutString( showUrl, 80 ) );
                block.Set( "d.DeleteUrl", to( Delete, menu.Id ) );
                block.Set( "d.EditUrl", to( Edit, menu.Id ) );

                String lnkSubMenu = "";
                if (menu.ParentId == 0)
                    lnkSubMenu = string.Format( "<a href=\"{0}\" class=\"frmBox\" >{1}</a>", to( AddSubMenu, menu.Id ), lang( "addSubMenu" ) );

                block.Set( "d.AddSubMenu", lnkSubMenu );

                block.Next();
            }
        }

        protected IMenu validateMenu( IMenu menu ) {

            menu.Name = ctx.Post( "Name" );
            menu.Url = ctx.Post( "Url" );
            menu.Style = getMenuStyle( ctx );
            String rawUrl = ctx.Post( "RawUrl" );

            if (strUtil.IsNullOrEmpty( menu.Name )) errors.Add( lang( "exName" ) );
            if (strUtil.IsNullOrEmpty( rawUrl )) errors.Add( lang( "exUrl" ) );

            Boolean isUrl = RegPattern.IsMatch( rawUrl, RegPattern.Url );
            if (!isUrl) errors.Add( lang( "exUrlFormat" ) );

            if (ctx.HasErrors) return null;


            Boolean isOutUrl = PathHelper.IsOutUrl( rawUrl );

            logger.Info( "isOutUrl=" + isOutUrl );

            if (isOutUrl) {
                menu.RawUrl = rawUrl;
                menu.Url = "";
            }
            else {
                IMember owner = ctx.owner.obj;
                String cleanedUrl = UrlConverter.clearUrl( rawUrl, ctx, owner.GetType().FullName, owner.Url );
                logger.Info( "cleanedUrl=" + cleanedUrl );
                menu.RawUrl = cleanedUrl;
            }

            menu.OpenNewWindow = ctx.PostIsCheck( "chkBlank" );

            if (strUtil.IsNullOrEmpty( menu.Name )) errors.Add( lang( "exName" ) );
            if (strUtil.IsNullOrEmpty( menu.RawUrl )) errors.Add( lang( "exUrl" ) );
            if (strUtil.HasText( menu.Url ) && strUtil.IsUrlItem( menu.Url ) == false) errors.Add( lang( "exFriendUrlFormat" ) );

            return menu;
        }

        private static String getMenuStyle( MvcContext ctx ) {

            String style = "";
            if (ctx.PostIsCheck( "chkBold" ) == 1) style += "font-weight:bold;";
            String menuColor = ctx.Post( "menuColor" );
            if (strUtil.HasText( menuColor )) {
                String mcolor = cvt.ToColorValue( menuColor );
                if (mcolor != null) style += "color:" + mcolor + ";";
            }
            return style;
        }

    }
}

