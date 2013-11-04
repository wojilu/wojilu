/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// mvc 处理流程中的事件发布者
    /// </summary>
    public class MvcEventPublisher {

        public static MvcEventPublisher Instance = new MvcEventPublisher();

        private MvcEventPublisher() {
        }

        public event EventHandler<MvcEventArgs> Begin_ProcessMvc;

        public event EventHandler<MvcEventArgs> Begin_UrlRewrite;

        public event EventHandler<MvcEventArgs> Begin_ParseRoute;
        public event EventHandler<MvcEventArgs> Begin_InitContext;

        public event EventHandler<MvcEventArgs> Begin_CheckActionMethod;
        public event EventHandler<MvcEventArgs> Begin_CheckForbiddenAction;
        public event EventHandler<MvcEventArgs> Begin_CheckLoginAction;
        public event EventHandler<MvcEventArgs> Begin_CheckHttpMethod;
        public event EventHandler<MvcEventArgs> Begin_CheckPermission;

        public event EventHandler<MvcEventArgs> Begin_ProcessAction;
        public event EventHandler<MvcEventArgs> End_ProcessAction;

        public event EventHandler<MvcEventArgs> Begin_AddLayout;
        public event EventHandler<MvcEventArgs> Begin_AddNsLayout;
        public event EventHandler<MvcEventArgs> Begin_Render;
        public event EventHandler<MvcEventArgs> Begin_EndRender;

        public virtual void BeginProcessMvc( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_ProcessMvc;
            if (handler != null) handler( this, new MvcEventArgs( "BeginProcessMvc", ctx ) );
        }

        public virtual void BeginUrlRewrite( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_UrlRewrite;
            if (handler != null) handler( this, new MvcEventArgs( "BeginUrlRewrite", ctx ) );
        }

        public virtual void BeginParseRoute( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_ParseRoute;
            if (handler != null) handler( this, new MvcEventArgs( "BeginParseRoute", ctx ) );
        }

        public virtual void BeginInitContext( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_InitContext;
            if (handler != null) handler( this, new MvcEventArgs( "BeginInitContext", ctx ) );
        }

        public virtual void BeginCheckActionMethod( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_CheckActionMethod;
            if (handler != null) handler( this, new MvcEventArgs( "CheckActionMethod", ctx ) );
        }

        public virtual void BeginCheckForbiddenAction( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_CheckForbiddenAction;
            if (handler != null) handler( this, new MvcEventArgs( "CheckForbiddenAction", ctx ) );
        }

        public virtual void BeginCheckLoginAction( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_CheckLoginAction;
            if (handler != null) handler( this, new MvcEventArgs( "CheckLoginAction", ctx ) );
        }

        public virtual void BeginCheckHttpMethod( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_CheckHttpMethod;
            if (handler != null) handler( this, new MvcEventArgs( "CheckHttpMethod", ctx ) );
        }

        public virtual void BeginCheckPermission( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_CheckPermission;
            if (handler != null) handler( this, new MvcEventArgs( "CheckPermission", ctx ) );
        }

        public virtual void BeginProcessAction( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_ProcessAction;
            if (handler != null) handler( this, new MvcEventArgs( "ProcessAction", ctx ) );
        }

        public virtual void EndProcessAction( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.End_ProcessAction;
            if (handler != null) handler( this, new MvcEventArgs( "EndProcessAction", ctx ) );
        }

        public virtual void BeginAddLayout( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_AddLayout;
            if (handler != null) handler( this, new MvcEventArgs( "AddLayout", ctx ) );
        }

        public virtual void BeginAddNsLayout( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_AddNsLayout;
            if (handler != null) handler( this, new MvcEventArgs( "AddNsLayout", ctx ) );
        }

        public virtual void BeginRender( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_Render;
            if (handler != null) handler( this, new MvcEventArgs( "Render", ctx ) );
        }

        public virtual void EndRender( MvcContext ctx ) {
            EventHandler<MvcEventArgs> handler = this.Begin_EndRender;
            if (handler != null) handler( this, new MvcEventArgs( "EndRender", ctx ) );
        }


    }

}
