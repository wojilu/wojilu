/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum.Admin {


    public partial class ForumController : ControllerBase {


        private void bindBoards( List<ForumBoard> orderedList ) {
            IBlock fbBlock = getBlock( "list" );

            foreach (ForumBoard board in orderedList) {

                int depth = getTree().GetDepth( board.Id );
                int indent = 20 * depth;

                String imgTree = strUtil.Join( sys.Path.Img, "tree/expand.gif" );

                String icon = board.IsCategory == 1 ? "<img src=\"" + imgTree + "\" />" : "";
                String name = "<span style=\"margin-left:" + indent + "px\">" + icon + board.Name + "</span>";

                fbBlock.Set( "board.Id", board.Id );
                fbBlock.Set( "board.Title", name );
                fbBlock.Set( "board.Description", strUtil.CutString( board.Description, 50 ) );

                if (board.ParentId == 0)
                    setCategory( fbBlock, board );
                else
                    setBoard( fbBlock, board );

                fbBlock.Next();
            }
        }


        private void setCategory( IBlock fbBlock, ForumBoard fb ) {

            fbBlock.Set( "board.StyleClass", "categoryRow" );
            fbBlock.Set( "board.AddUrl", string.Format( "<a href='{0}' class='frmBox' title='" + alang( "addSubBoard" ) + "'>+" + alang( "addSubBoard" ) + "</a>", to( AddSubBoard, fb.Id ) ) );

            fbBlock.Set( "board.EditUrl", to( EditBoard, fb.Id ) );
            fbBlock.Set( "board.DeleteUrl", to( DeleteCategory, fb.Id ) );
            fbBlock.Set( "board.SetCategory", string.Empty );
            fbBlock.Set( "board.Moderator", string.Empty );
            fbBlock.Set( "board.SetModerator", string.Empty );
            fbBlock.Set( "board.SetSecurity", string.Empty );
            fbBlock.Set( "deleteMsg", alang( "exDeleteSubFirst" ) );

        }

        private void setBoard( IBlock fbBlock, ForumBoard fb ) {

            fbBlock.Set( "board.StyleClass", "" );

            fbBlock.Set( "board.AddUrl", string.Format( "<a href='{0}' class='frmBox' title='" + alang( "addSubBoard" ) + "'>+" + lang( "add" ) + "</a>", to( AddSubBoard, fb.Id ) ) );

            fbBlock.Set( "board.EditUrl", to( EditBoard, fb.Id ) );
            fbBlock.Set( "board.DeleteUrl", to( DeleteBoard, fb.Id ) );
            fbBlock.Set( "lineStyle", "" );

            int categoryCount = categoryService.CountByBoard( fb.Id );
            String categoryCountStr = categoryCount > 0 ? "(" + categoryCount + ")" : "";
            String lnkSetCategory = string.Format( "<a href='{0}' class='frmBox' title='" + alang( "postCategoryAdmin" ) + "'>" + alang( "category" ) + "{1}</a>", to( new CategoryController().Admin, fb.Id ), categoryCountStr );

            String imgUser = strUtil.Join( sys.Path.Img, "users.gif" );
            String imgSecurity = strUtil.Join( sys.Path.Img, "security.gif" );

            String lnkSetModerator = string.Format( "<a href='{0}' class='frmBox' title='" + alang( "setModerator" ) + "'><img src=\"{1}\"/> " + alang( "setModerator" ) + "</a>", to( new ModeratorController().List, fb.Id ), imgUser );
            String lnkSetSecurity = string.Format( "<a href='{0}' class='frmBox' xwidth='600' title='" + alang( "setSecurity" ) + "'><img src=\"{1}\"/> " + alang( "setSecurity" ) + "</a>", to( new SecurityController().BoardSetting, fb.Id ), imgSecurity );

            fbBlock.Set( "board.SetCategory", lnkSetCategory );
            fbBlock.Set( "board.Moderator", moderatorService.GetModeratorText( fb.Moderator ) );
            fbBlock.Set( "board.SetModerator", lnkSetModerator );
            fbBlock.Set( "board.SetSecurity", lnkSetSecurity );
            fbBlock.Set( "deleteMsg", alang( "exDeleteBoardTip" ) );
        }

        //------------------------------------------------------------------------------

        private void bindAddSubBoard( int boardId, ForumBoard board ) {
            set( "lblForumAction", alang( "addSubBoard" ) );

            set( "Name", ctx.Post( "Name" ) );
            set( "Description", ctx.Post( "Description" ) );
            set( "Notice", ctx.Post( "Notice" ) );
            set( "boardLogo", string.Empty );

            set( "CategoryDropDown", Html.InputHidden( "ParentId", boardId.ToString() ) + board.Name );
            set( "ViewId", BoardViewStatus.GetDropList( "ViewId", 0 ) );

            set( "chkIsCategory", Html.CheckBox( "IsCategory", alang( "noPost" ), "1", false ) );
        }


        private void bindBoard( ForumBoard board ) {
            set( "Name", board.Name );
            set( "Description", board.Description );

            String logo;
            if (strUtil.HasText( board.Logo )) {
                logo = sys.Path.GetPhotoOriginal( board.Logo );
                logo = "<span id=\"logoContainer\"><img src=\"" + logo + "\"/> " +
                    "<img src=\"" + sys.Path.Img + "delete.gif\" title=\"" + lang( "delete" ) + "\" id=\"deleteLogo\" class=\"right20\" data-action=\"" + to( DeleteLogo, board.Id ) + "\"/></span>";
            }
            else {
                logo = strUtil.Join( sys.Path.Skin, "apps/forum/normal.gif" );
                logo = "<img src=\"" + logo + "\"/>";
            }
            set( "boardLogo", logo );

            set( "chkIsCategory", Html.CheckBox( "IsCategory", alang( "noPost" ), "1", (board.IsCategory == 1) ) );

            set( "Notice", board.Notice );

            set( "CategoryDropDown", getTree().DropList( "ParentId", board.ParentId, board.Id, lang( "rootNode" ) ) );
            set( "ViewId", BoardViewStatus.GetDropList( "ViewId", board.ViewId ) );
        }

        private void bindTrashTopic( DataPage<ForumTopic> deletedPage ) {
            IBlock block = getBlock( "list" );
            foreach (ForumTopic topic in deletedPage.Results) {
                block.Set( "post.Id", topic.Id );
                block.Set( "post.Title", topic.Title );
                block.Set( "post.MemberName", topic.Creator == null ? "" : topic.Creator.Name );
                block.Set( "post.MemberUrl", topic.Creator == null ? "" : toUser( topic.Creator ) );
                block.Set( "post.ForumBoardName", topic.ForumBoard.Name );
                block.Set( "post.LinkShow", to( ViewDeletedTopic, topic.Id ) );
                block.Set( "post.Hits", topic.Hits );
                block.Set( "post.ReplyCount", topic.Replies );
                block.Set( "post.Created", topic.Created );
                block.Set( "post.Ip", topic.Ip );
                block.Next();
            }
            set( "page", deletedPage.PageBar );
        }


        private void bindTrashPost( DataPage<ForumPost> deletedPage ) {
            IBlock block = getBlock( "list" );
            foreach (ForumPost data in deletedPage.Results) {

                ForumTopic topic = topicService.GetById_ForAdmin( data.TopicId );

                block.Set( "post.Id", data.Id );
                block.Set( "post.Title", data.Title );
                block.Set( "post.TopicTitle", topic.Title );

                block.Set( "post.MemberName", data.Creator == null ? "" : data.Creator.Name );
                block.Set( "post.MemberUrl", data.Creator == null ? "" : toUser( data.Creator ) );
                block.Set( "post.ForumBoardName", topic.ForumBoard.Name );
                block.Set( "post.LinkShow", to( ViewDeletedPost, data.Id ) );
                block.Set( "post.Created", data.Created );
                block.Set( "post.Ip", data.Ip );
                block.Next();
            }
            set( "page", deletedPage.PageBar );
        }

    }
}

