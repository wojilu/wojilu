/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.ORM;

using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Content.Domain;
using wojilu.Common.Comments;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Open {

    public class CommentListController<T> : ControllerBase where T : ObjectBase<T> {



        public IOpenCommentService commentService { get; set; }

        public CommentListController() {
            commentService = new OpenCommentService();
        }

        public void Index() {

            String condition = getCondition();

            DataPage<OpenComment> list = commentService.GetPageAll( condition );
            bindList( "list", "c", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        private String getCondition() {

            String appPrefix = getAppCondition();

            return appPrefix + " TargetDataType='" + typeof( T ).FullName + "' ";

        }

        private String getAppCondition() {
            if (ctx.app == null) return "";
            if (ctx.app.Id <= 0) return "";
            return "AppId=" + ctx.app.Id + " and";
        }

        private void bindLink( IBlock block, String lbl, object obj ) {
            OpenComment c = (OpenComment)obj;
            block.Set( "c.sContent", strUtil.CutString( c.Content, 100 ) );

            Type targetType = Entity.GetType( c.TargetDataType );

            IEntity parent = ndb.findById( targetType, c.TargetDataId );
            if (parent == null) {
                block.Set( "c.TargetTitle", "" );
                block.Set( "c.Link", "#" );
            }
            else {
                Object title = parent.get( "Title" );
                block.Set( "c.TargetTitle", title );

                IAppData objP = (IAppData)parent;
                String clink = objP == null ? "#" : alink.ToAppData( objP );
                block.Set( "c.Link", clink );
            }
        }



    }
}
