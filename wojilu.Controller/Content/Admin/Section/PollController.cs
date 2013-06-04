/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Admin.Common;
using wojilu.Web.Controller.Content.Utils;


namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public partial class PollController : ControllerBase, IPageAdminSection {

        public virtual IContentPostService postService { get; set; }
        public virtual ContentPollService pollService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }

        public PollController() {
            postService = new ContentPostService();
            pollService = new ContentPollService();
            sectionService = new ContentSectionService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {

            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().EditCount, sectionId );
            links.Add( lnk );

            return links;
        }

        public String GetEditLink( int postId ) {
            return to( new Common.PollController().Edit, postId );
        }

        public String GetSectionIcon( int sectionId ) {
            return BinderUtils.iconPoll;
        }

        public void AdminSectionShow( int sectionId ) {

            set( "section.Id", sectionId );
            set( "addLink", to( new Admin.Common.PollController().Add, sectionId ) );
            set( "listLink", to( new Admin.Common.PollController().AdminList, sectionId ) );

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            List<ContentPost> posts = postService.GetBySection( sectionId, section.ListCount );
            List<ContentPoll> polls = pollService.GetByTopicList( posts );

            IBlock block = getBlock( "list" );

            foreach (ContentPost x in posts) {

                ContentPoll p = pollService.GetByTopicId( polls, x.Id );

                String editLink = string.Format( "<a href=\"{0}\" class=\"frmBox\"><img src=\"{1}edit.gif\" />{2}</a>",
                    to( new Admin.Common.PollController().Edit, x.Id ),
                    sys.Path.Img,
                    lang( "edit" ) );
                block.Set( "editLink", editLink );
                ctx.SetItem( "poll", p );
                block.Set( "pollHtml", loadHtml( new wojilu.Web.Controller.Content.Common.PollController().Detail ) );
                block.Next();
            }

        }

        public List<ContentPost> GetSectionPosts( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            return postService.GetBySection( sectionId, section.ListCount );
        }

    }
}
