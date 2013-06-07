/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.SOA;

using wojilu.Web.Mvc;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content {

    public partial class ContentController : ControllerBase {


        private void bindRows( ContentApp article, List<ContentSection> sections ) {
            IBlock block = getBlock( "rowList" );
            string[] rowList = article.RowList;
            for (int i = 1; i < (rowList.Length + 1); i++) {
                int columnCount = cvt.ToInt( rowList[i - 1] );
                IBlock columnBlock = block.GetBlock( "columnList" );
                bindColumnList( sections, i, columnCount, columnBlock );
                block.Set( "row.Index", i );
                block.Next();
            }
        }

        private void bindColumnList( List<ContentSection> sectionList, int iRow, int columnCount, IBlock columnBlock ) {
            for (int i = 1; i < (columnCount + 1); i++) {

                columnBlock.Set( "column.Index", i );
                columnBlock.Set( "column.Id", "row" + iRow + "_column" + i );
                columnBlock.Set( "columnId", string.Format( "row{0}_column{1}", iRow, i ) );

                IBlock sectionBlock = columnBlock.GetBlock( "sectionList" );
                List<ContentSection> sections = SectionService.GetByRowColumn( sectionList, iRow, i );
                bindSectionList( sectionBlock, sections );
                columnBlock.Next();
            }
        }

        private void bindSectionList( IBlock sectionBlock, IList sections ) {

            int iSection = 1;
            foreach (ContentSection section in sections) {

                String moreUrl = getMoreUrl( section );
                String moreLink = getMoreLink( moreUrl );
                String title = getTitle( section, moreUrl );

                sectionBlock.Set( "section.Title", title );
                sectionBlock.Set( "section.MoreLink", moreLink );
                sectionBlock.Set( "section.CombineIds", section.CombineIds );
                sectionBlock.Set( "section.StyleClass", section.CssClass );

                String marquee = section.GetMarquee();
                if (strUtil.HasText( marquee )) {
                    String m = string.Format( "<marquee direction=\"{0}\" onMouseOver=\"stop()\" onMouseOut=\"start()\">", marquee );
                    sectionBlock.Set( "section.MarqueeStart", m );
                    sectionBlock.Set( "section.MarqueeEnd", "</marquee>" );
                }
                else {
                    sectionBlock.Set( "section.MarqueeStart", "" );
                    sectionBlock.Set( "section.MarqueeEnd", "" );
                }

                sectionBlock.Set( "section.Id", section.Id );
                sectionBlock.Set( "sectionClassId", iSection );

                String content = getSectionContent( section );
                sectionBlock.Set( "section.Content", content );
                sectionBlock.Next();

                iSection = iSection + 1;

            }
        }

        private String getTitle( ContentSection section, String moreUrl ) {

            if (strUtil.IsNullOrEmpty( moreUrl )) return section.Title;
            if (moreUrl.Equals( "#" )) return section.Title;
            if (isUrl( moreUrl )) return string.Format( "<a href=\"{0}\">{1}</a>", moreUrl, section.Title );

            return section.Title;
        }

        private String getMoreLink( String moreUrl ) {

            if (strUtil.IsNullOrEmpty( moreUrl ) || "#".Equals( moreUrl )) return "";

            if (isUrl( moreUrl )) return string.Format( "<a href=\"{0}\">{1}>></a>", moreUrl, lang( "more" ) );

            return moreUrl;
        }

        private String getMoreUrl( ContentSection section ) {

            // 自定义url优先
            if (strUtil.HasText( section.MoreLink )) {
                return section.MoreLink;
            }

            if (section.ServiceId > 0) return section.MoreLink;

            return clink.toSection( section.Id, ctx );
        }

        private Boolean isUrl( String url ) {
            return url.ToLower().StartsWith( "http://" ) || url.StartsWith( "/" );
        }

        private String getSectionContent( ContentSection section ) {
            String content;
            if (section.ServiceId <= 0) {
                content = getData( section );
            }
            else {
                content = getAutoData( section );
            }
            return content;
        }

        private String getData( ContentSection articleSection ) {
            IPageSection section = BinderUtils.GetPageSection( articleSection, ctx );
            ControllerBase sectionController = section as ControllerBase;
            section.SectionShow( articleSection.Id );
            String actionContent = sectionController.utils.getActionResult();
            return actionContent;
        }

        private String getAutoData( ContentSection section ) {

            Dictionary<string, string> presult = getDefaultValue( section );

            IList data = ServiceContext.GetData( section.ServiceId, section.GetServiceParamValues(), presult );

            if (section.TemplateId <= 0) return getJsonResult( section, data );

            ISectionBinder binder = BinderUtils.GetBinder( section, ctx );
            binder.Bind( section, data );
            ControllerBase sectionController = binder as ControllerBase;
            return sectionController.utils.getActionResult();
        }

        private String getJsonResult( ContentSection section, IList data ) {

            String jsonStr = Json.ToString( data );
            String scriptData = string.Format( "	<script>var sectionData{0} = {1};</script>", section.Id, jsonStr );
            if (section.CustomTemplateId <= 0)
                return scriptData;

            ContentCustomTemplate ct = ctService.GetById( section.CustomTemplateId, ctx.owner.Id );
            if (ct == null) return scriptData;

            return scriptData + ct.Content;
        }

        private Dictionary<string, string> getDefaultValue( ContentSection section ) {
            Service service = ServiceContext.Get( section.ServiceId );

            Dictionary<string, string> pd = service.GetParamDefault();
            Dictionary<string, string> presult = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in pd) {
                if (pair.Key.Equals( "ownerId" )) {
                    presult.Add( pair.Key, ctx.owner.Id.ToString() );
                }
                else if (pair.Key.Equals( "viewerId" )) {
                    presult.Add( pair.Key, ctx.viewer.Id.ToString() );
                }
                else if (pair.Key.Equals( "appId" )) {
                    presult.Add( pair.Key, ctx.app.Id.ToString() );
                }
                else {
                    presult.Add( pair.Key, pair.Key );
                }
            }
            return presult;
        }

        private String adminUrl {
            get { return ctx.app.Url; }
        }

    }
}

