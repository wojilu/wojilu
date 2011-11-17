/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Comments;
using wojilu.Common.AppBase.Interface;
using wojilu.Data;

namespace wojilu.Web.Controller.Common {

    public class CommentAdminController : ControllerBase {

        public CommentAdminService commentService { get; set; }

        public CommentAdminController() {
            commentService = new CommentAdminService();
        }

        public void List() {

            checkTypeName();

            set( "ActionLink", to( Admin ) + "?type=" + ctx.Get( "type" ) );
            set( "searchTarget", to( Search ) );
            set( "typeFullName", ctx.Get( "type" ) );
            set( "filterLink", to( List ) + "?type=" + ctx.Get( "type" ) );

            set( "searchKey", ctx.Get( "q" ) );
            setDroplist( ctx.Get( "t" ) );

            String condition = getCondition();

            initService();
            IPageList list = commentService.GetPageAll( condition );
            bindList( "list", "c", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        private void bindLink( IBlock block, String lbl, object obj ) {
            IComment c = (IComment)obj;
            block.Set( "c.sContent", strUtil.CutString( c.Content, 100 ) );

            IEntity parent = ndb.findById( c.GetTargetType(), c.RootId );
            if (parent == null) {
                block.Set( "c.TargetTitle", "" );
                block.Set( "c.Link", "#" );
            }
            else {
                String title = parent.get( "Title" ).ToString();
                block.Set( "c.TargetTitle", title );

                String clink = alink.ToAppData( (IAppData)parent );
                block.Set( "c.Link", clink );
            }
        }


        public void Search() {

            checkTypeName();

            String t = ctx.Get( "t" );
            String q = ctx.Get( "q" );
            if (isInputValid( t, q ) == false) {
                redirect( List );
                return;
            }

            //------------------------------------------------------

            set( "searchTarget", to( Search ) );
            set( "typeFullName", ctx.Get( "type" ) );
            set( "filterLink", to( List ) + "?type=" + ctx.Get( "type" ) );

            q = strUtil.SqlClean( q, getInputMaxLength() );

            set( "searchKey", q );
            setDroplist( t );

            String condition = getCondition( q, t );

            initService();
            IPageList list = commentService.GetPageAll( condition );
            bindList( "list", "c", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public void Admin() {

            checkTypeName();

            String ids = ctx.PostIdList( "choice" );
            initService();
            commentService.DeleteBatch( ids );

            echoAjaxOk();

        }

        //--------------------------------------------------------------------------------------------------------------

        private void initService() {
            String commentTypeName = ctx.Get( "type" );
            IComment comment = Entity.New( commentTypeName ) as IComment;
            commentService.setComment( comment );
        }


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
                    return t + " like '%" + q + "%'";
                }
            }
            return "";
        }

        private String getCondition() {
            String fTime = ctx.Get( "filter" );
            if (strUtil.IsNullOrEmpty( fTime )) return "";
            DateTime ft = getValidTime( fTime );

            String tquote = getEntityInfo().Dialect.GetTimeQuote();
            return "Created>" + tquote + ft.ToShortDateString() + tquote + "";
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

        private void checkTypeName() {
            if (isTypeError( ctx.Get( "type" ) )) throw new Exception( lang( "exTypeError" ) );
        }

        private Boolean isTypeError( String typeName ) {
            if (strUtil.IsNullOrEmpty( typeName )) return true;
            if (Entity.GetType(typeName) == null) return true;
            return false;
        }

        private EntityInfo getEntityInfo() {
            return Entity.GetInfo( ctx.Get( "type" ) );
        }

    }

}
