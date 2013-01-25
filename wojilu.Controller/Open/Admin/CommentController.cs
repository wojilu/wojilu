/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.ORM;

using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Content.Domain;
using wojilu.Common.Comments;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Open.Admin {


    public class CommentBaseController<T> : ControllerBase where T : ObjectBase<T> {


        public IOpenCommentService commentService { get; set; }

        public CommentBaseController() {
            commentService = new OpenCommentService();
        }

        public void List() {

            set( "ActionLink", to( Admin ) );
            set( "searchTarget", to( Search ) );
            set( "filterLink", to( List ) );

            set( "searchKey", ctx.Get( "q" ) );
            setDroplist( ctx.Get( "t" ) );

            String condition = getCondition();

            DataPage<OpenComment> list = commentService.GetPageAll( condition );
            bindList( "list", "c", list.Results, bindLink );
            set( "page", list.PageBar );
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


        public void Search() {

            view( "/Open/Admin/CommentBase/List" );

            String t = ctx.Get( "t" );
            String q = ctx.Get( "q" );
            if (isInputValid( t, q ) == false) {
                redirect( List );
                return;
            }

            //------------------------------------------------------

            set( "searchTarget", to( Search ) );
            set( "filterLink", to( List ) );

            q = strUtil.SqlClean( q, getInputMaxLength() );

            set( "searchKey", q );
            setDroplist( t );

            String condition = getCondition( q, t );

            DataPage<OpenComment> list = commentService.GetPageAll( condition );
            bindList( "list", "c", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            commentService.DeleteBatch( ids );

            echoAjaxOk();

        }

        //--------------------------------------------------------------------------------------------------------------

        private Boolean isInputValid( String t, String q ) {

            if (strUtil.IsNullOrEmpty( t )) return false;
            if (strUtil.IsNullOrEmpty( q )) return false;

            if (q.Length > getInputMaxLength()) return false;

            return getTypeList().Contains( t );
        }

        private int getInputMaxLength() {
            return 20;
        }

        private List<string> getTypeList() {
            List<string> typeList = new List<string>();
            typeList.Add( "author" );
            typeList.Add( "content" );
            typeList.Add( "ip" );
            return typeList;
        }

        private void setDroplist( String inputType ) {

            foreach (String t in getTypeList()) {

                String optionKey = "t" + t;
                if (strUtil.IsNullOrEmpty( inputType )) {
                    set( optionKey, "" );
                }
                else if (inputType.Equals( t )) {
                    set( optionKey, "selected=\"selected\"" );
                }
                else
                    set( optionKey, "" );
            }
        }

        private String getCondition( String q, String inputType ) {
            foreach (String t in getTypeList()) {
                if (inputType.Equals( t )) {
                    return getAppTypeCondition() + " and " + t + " like '%" + q + "%'";
                }
            }
            return getAppTypeCondition();
        }

        private String getCondition() {
            String fTime = ctx.Get( "filter" );
            if (strUtil.IsNullOrEmpty( fTime )) return getAppTypeCondition();

            DateTime ft = getValidTime( fTime );
            String tquote = Entity.GetInfo( typeof( OpenComment ) ).Dialect.GetTimeQuote();
            return getAppTypeCondition() + " and Created>" + tquote + ft.ToShortDateString() + tquote + "";
        }

        private String getAppTypeCondition() {

            String appPrefix = getAppCondition();

            if (typeof( T ) == typeof( NullCommentTarget )) return appPrefix;

            return appPrefix + " TargetDataType='" + typeof( T ).FullName + "' ";
        }

        private String getAppCondition() {
            if (ctx.app == null) return "";
            if (ctx.app.Id <= 0) return "";
            return "AppId=" + ctx.app.Id + " and";
        }

        private static DateTime getValidTime( String time ) {
            DateTime t = DateTime.Now.AddYears( 99 );
            if (time.Equals( "day" ))
                t = DateTime.Now.AddDays( -1 );
            else if (time.Equals( "day2" ))
                t = DateTime.Now.AddDays( -2 );
            else if (time.Equals( "week" ))
                t = DateTime.Now.AddDays( -7 );
            else if (time.Equals( "month" ))
                t = DateTime.Now.AddMonths( -1 );
            else if (time.Equals( "month3" ))
                t = DateTime.Now.AddMonths( -3 );
            else if (time.Equals( "month6" ))
                t = DateTime.Now.AddMonths( -6 );
            return t;
        }


    }

}
