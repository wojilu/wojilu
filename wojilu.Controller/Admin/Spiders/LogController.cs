/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Spider.Domain;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class LogController : ControllerBase {

        public void List() {

            set( "clearLogUrl", to( Clear ) );
            set( "OperationUrl", to( Admin ) );

            DataPage<SpiderLog> logs = SpiderLog.findPage( "" );
            IBlock block = getBlock( "list" );
            foreach (SpiderLog log in logs.Results) {

                block.Set( "log.Id", log.Id );
                block.Set( "log.Msg", log.Msg );
                block.Set( "log.MsgInfo", strUtil.CutString( log.Msg, 300 ) );
                block.Set( "log.Created", log.Created );
                block.Set( "log.ViewUrl", to( Show, log.Id ) );

                block.Next();
            }
            set( "page", logs.PageBar );
        }

        public void Show( int id ) {

            SpiderLog log = SpiderLog.findById( id );
            bind( "log", log );
        }

        [HttpPost, DbTransaction]
        public void Admin() {
            String ids = ctx.PostIdList( "choice" );
            String action = ctx.Post( "action" );

            if (strUtil.IsNullOrEmpty( ids )) {
                echoError( "请先选择" );
                return;
            }

            if ("delete".Equals( action )) {
                SpiderLog.deleteBatch( "Id in (" + ids + ")" );
            }

            echoAjaxOk();
        }

        public void Clear() {
            target( ClearBegin );
        }

        [HttpPost, DbTransaction]
        public void ClearBegin() {
            SpiderLog.deleteBatch( "1=1" );
            echoRedirect( lang( "opok" ), to( List ) );
        }


    }
}
