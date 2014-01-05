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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Templates;
using wojilu.Web.Templates.Tokens;

namespace wojilu.Members.Users.Domain {
    internal class __temp1 {
    }
}
namespace wojilu.Common.AppBase.Interface {
    internal class __temp1 {
    }
}

namespace wojilu.Web {

    /// <summary>
    /// 模板区块的基类
    /// </summary>
    public class ContentBlock : IBlock {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ContentBlock ) );

        protected Boolean _isTemplateExist = true;
        protected String _templatePath;


        public String Name { get; set; }

        internal BlockToken _thisToken;

        public List<Token> getTokens() {
            return _thisToken.getTokens();
        }


        private static readonly Dictionary<String, String> viewFunctionMap = loadViewFunctionMap();

        private static Dictionary<String, String> loadViewFunctionMap() {

            Dictionary<String, String> map = new Dictionary<String, String>();

            map.Add( "v.data", "vdata.get" );
            map.Add( "v.end()", "return sb" );
            map.Add( "v.load(", "ctx.load( sb, " );

            map.Add( "page.header", "System.Web.HttpContext.Current.Response.AppendHeader" );
            map.Add( "page.status", "ctx.web.ResponseStatus" );

            map.Add( "page.go(", "return goUrl(sb," );
            map.Add( "page.exit(", "return exitMsg(sb," );
            map.Add( "page.exitText(", "return exitText(sb," );
            map.Add( "page.json(", "return exitJson(sb," );

            map.Add( "page.title", "ctx.Page.SetTitle" );
            map.Add( "page.description", "ctx.Page.SetDescription" );
            map.Add( "page.keywords", "ctx.Page.SetKeywords" );

            map.Add( "html.show", "sb.Append" );
            map.Add( "Response.Write", "sb.Append" );
            map.Add( "html.showLine", "sb.AppendLine" );
            map.Add( "html.encode", "System.Web.HttpUtility.HtmlEncode" );

            map.Add( "ctx.get", "ctx.Get" );
            map.Add( "ctx.getInt", "ctx.GetInt" );
            map.Add( "ctx.getLong", "ctx.GetLong" );
            map.Add( "ctx.post", "ctx.Post" );
            map.Add( "ctx.postInt", "ctx.PostInt" );
            map.Add( "ctx.postLong", "ctx.PostLong" );
            map.Add( "ctx.getItem", "ctx.GetItem" );
            map.Add( "ctx.setItem", "ctx.SetItem" );

            map.Add( "ctx.ip", "ctx.Ip" );
            map.Add( "ctx.method", "ctx.HttpMethod" );
            map.Add( "ctx.agent", "ctx.web.ClientAgent" );

            map.Add( "url.text", "ctx.web.Url.ToString()" );
            map.Add( "url.path", "ctx.url.Path" );
            map.Add( "url.query", "ctx.url.Query" );
            map.Add( "url.from", "ctx.web.PathReferrer" );

            map.Add( "link.to", "__to" );
            map.Add( "link.user", "__toUser" );
            map.Add( "link.app", "wojilu.Web.Mvc.alink.ToApp" );
            map.Add( "link.data", "wojilu.Web.Mvc.alink.ToAppData" );

            map.Add( "path.pic", "sys.Path.GetPhotoThumb" );
            map.Add( "path.face", "sys.Path.GetAvatarThumb" );
            map.Add( "path.map", "System.Web.HttpContext.Current.Server.MapPath" );

            return map;
        }

        private String replaceCode( String codeItem ) {
            String ret = codeItem;
            foreach (KeyValuePair<String, String> kv in viewFunctionMap) {
                ret = ret.Replace( kv.Key, kv.Value );
            }
            return ret;
        }

        public String GetCode( String clsName ) {
            StringBuilder code = new StringBuilder();

            List<String> nsList = new List<String>();
            nsList.Add( "System" );
            nsList.Add( "System.Collections.Generic" );
            nsList.Add( "System.Text" );
            nsList.Add( "wojilu" );
            nsList.Add( "wojilu.Web" );
            nsList.Add( "wojilu.Web.Mvc" );
            nsList.Add( "wojilu.Web.Context" );
            nsList.Add( "wojilu.Web.Templates" );
            nsList.Add( "wojilu.Members.Users.Domain" );
            nsList.Add( "wojilu.Members.Interface" );
            nsList.Add( "wojilu.Common.AppBase.Interface" );

            List<String> funcList = new List<String>();

            code.AppendLine( "namespace wojilu {" );

            code.AppendLine( "public class " + clsName + " : ITemplateResult {" );
            code.AppendLine( "		private wojilu.Web.Context.MvcContext ctx;" );
            code.AppendLine( "		private ViewData vdata;" );
            code.AppendLine( "		public void SetData(ViewData viewData) {" );
            code.AppendLine( "			vdata = viewData;" );
            code.AppendLine( "			ctx = vdata[\"__ctx\"] as wojilu.Web.Context.MvcContext;" );
            code.AppendLine( "}" );

            code.AppendLine( @"
        private String __toUser( IMember user ) {
            return Link.ToMember( user );
        }

        private String __to( aAction action ) {
            return ctx.link.To( action );
        }

        private String __to( aActionWithId action, long id ) {
            return ctx.link.To( action, id );
        }

        private StringBuilder goUrl( StringBuilder sb, String url ) {
            ctx.controller.redirectDirect( url );
            return sb;
        }

        private StringBuilder exitMsg( StringBuilder sb, String msg ) {
            return exitMsg( sb, msg, null );
        }

        private StringBuilder exitMsg( StringBuilder sb, String msg, String httpStatusCode ) {
            ctx.utils.endMsg( msg, httpStatusCode );
            return sb;
        }

        private StringBuilder exitText( StringBuilder sb, String msg ) {
            return exitText( sb, msg, null );
        }

        private StringBuilder exitText( StringBuilder sb, String msg, String httpStatusCode ) {
            ctx.utils.endMsgByText( msg, httpStatusCode );
            return sb;
        }

        private StringBuilder exitJson( StringBuilder sb, String jsonStr ) {
            ctx.controller.echoJson( jsonStr );
            return sb;
        }

        private StringBuilder exitJson( StringBuilder sb, Object jsonStr ) {
            ctx.controller.echoJson( jsonStr );
            return sb;
        }
" );

            code.AppendLine( "public StringBuilder GetResult() {" );

            code.AppendLine( "try {" );

            code.AppendLine( "StringBuilder sb = new StringBuilder();" );
            List<Token> tokenList = this.getTokens();
            for (int i = 0; i < tokenList.Count; i++) {

                Token x = tokenList[i];

                if (x.getType() == TokenType.Var) {
                    code.AppendLine( "sb.Append( vdata[\"" + x.getName() + "\"] );" ); // "vdata[ x.getName() ]"
                }

                else {

                    if (x.getType() == TokenType.Code) {

                        String codeItem = x.getValue().Trim();
                        if (strUtil.IsNullOrEmpty( codeItem )) continue;

                        if (codeItem.StartsWith( "=" )) {
                            code.AppendLine( "sb.Append( " + replaceCode( codeItem.TrimStart( '=' ) ) + ");" );
                        }

                        else if (isPageDirective( codeItem )) {
                            // <%@ Page Language="C#" %> --skip
                            continue;
                        }
                        else {
                            String aNamespace = getNamespace( codeItem );
                            if (aNamespace != null) {
                                if (nsList.Contains( aNamespace ) == false) {
                                    nsList.Add( aNamespace );
                                }
                            }
                            else {
                                if (codeItem.EndsWith( ";" ) == false) codeItem += ";";
                                code.AppendLine( replaceCode( codeItem ) );
                            }
                        }
                    }
                    else if (x.getType() == TokenType.Function) {
                        funcList.Add( replaceCode( x.getValue() ) );

                    }
                    else if (x.getType() == TokenType.Block) {
                        code.AppendLine( "sb.Append( vdata.getBlockResultByIndex(" + i + ") );" );
                    }
                    else {
                        if (strUtil.IsNullOrEmpty( x.getValue() )) continue;
                        code.AppendLine( "sb.Append(@\"" + x.getValue().TrimStart( '\r' ).TrimStart( '\n' ).Replace( "\"", "\"\"" ) + "\");" );
                    }
                }
            }
            code.AppendLine( "return sb;" );

            code.AppendLine( "}catch (Exception ex) {" );
            code.AppendLine( "throw new TemplateRunException( \"模板运行错误\", ex );" );
            code.AppendLine( "}" );

            code.AppendLine( "}" );

            foreach (String func in funcList) {
                code.AppendLine( func );
            }

            code.AppendLine( "}" );
            code.AppendLine( "}" );

            // namespace
            StringBuilder sbNs = new StringBuilder();
            foreach (String ns in nsList) {
                sbNs.AppendFormat( "using {0};", ns );
                sbNs.AppendLine();
            }

            return sbNs + code.ToString();
        }



        private String getNamespace( String codeItem ) {
            // <%@ Import Namespace="System.Collections.Generic" %>
            if (codeItem.StartsWith( "@" ) == false) return null;
            if (codeItem.IndexOf( "Import" ) < 0) return null;
            if (codeItem.IndexOf( "Namespace" ) < 0) return null;

            String[] arrItem = codeItem.TrimStart( '@' ).Trim().Split( '"' );
            if (arrItem.Length != 3) return null;

            return arrItem[1].Trim();
        }

        private bool isPageDirective( string codeItem ) {
            // <%@ Page Language="C#" %>
            if (codeItem.StartsWith( "@" )) {
                return codeItem.TrimStart( '@' ).Trim().StartsWith( "Page" );
            }
            return false;
        }

        private List<BlockData> _blockdataList = new List<BlockData>();
        private BlockData _blockData = new BlockData( new Dictionary<String, object>() );

        public BlockData getData() {
            return _blockData;
        }

        internal List<BlockData> getDataList() {
            return _blockdataList;
        }

        public String getTemplatePath() {
            return _templatePath;
        }

        /// <summary>
        /// 绑定一行完毕，进入下一行绑定
        /// </summary>
        public void Next() {

            BlockData saved = _blockData;
            _blockdataList.Add( saved );

            _blockData = new BlockData( saved.getParentDic() );
        }

        /// <summary>
        /// 给模板中的变量赋值
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="lblValue"></param>
        public void Set( String lbl, String lblValue ) {
            _blockData.getDic()[lbl] = (lblValue == null ? "" : lblValue);
        }

        /// <summary>
        /// 给模板中的变量赋值
        /// </summary>
        /// <param name="lbl">模板中的变量</param>
        /// <param name="val">被赋予的值，如果是DateTime和Decimal，会被转化成字符串。如果不想转成字符串，请用Bind方法</param>
        public void Set( String lbl, Object val ) {

            if (val == null) {
                this.Set( lbl, "" );
                return;
            }

            if (val is DateTime) {
                this.Set( lbl, ((DateTime)val).ToString( "g" ) );
            }
            else if (val is decimal) {
                this.Set( lbl, ((decimal)val).ToString( "n2" ) );
            }
            else {
                _blockData.getDic()[lbl] = val;
            }

        }

        /// <summary>
        /// 将对象绑定到模板。模板中变量前缀使用camelCase格式：#{blogPost.Title}
        /// </summary>
        /// <param name="obj">被绑定的对象</param>
        public void Bind( Object obj ) {
            String lbl = getCamelCase( obj.GetType().Name );
            Bind( lbl, obj );
        }

        /// <summary>
        /// 将对象绑定到模板。可以自定义模板中变量前缀：#{x.Title}
        /// </summary>
        /// <param name="lbl">比如变量#{x.Title}中的x</param>
        /// <param name="obj">被绑定的对象</param>
        public void Bind( String lbl, Object obj ) {

            if (this.bindOtherFunc != null) {
                bindOtherFunc( this, lbl, obj );
            }

            bindPrivate( lbl, obj );
        }

        private void bindPrivate( String lbl, Object obj ) {
            if (this.bindFunc != null && !(obj is IDictionary)) {
                bindFunc( this, cvt.ToInt( rft.GetPropertyValue( obj, "Id" ) ) );
            }

            _blockData.getDic()[lbl] = obj;
        }

        private static String getCamelCase( String str ) {
            return str[0].ToString().ToLower() + str.Substring( 1 );
        }

        /// <summary>
        /// 判断区块是否存在
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        public Boolean HasBlock( String blockName ) {

            if (_isTemplateExist == false) {
                throw new TemplateException( lang.get( "exTemplateNotExist" ) + ": " + _templatePath );
            }

            return getBlockToken( blockName ) != null;
        }

        /// <summary>
        /// 返回一个做数据容器的block（可以作为数据容器，并包括tokens信息）
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        public IBlock GetBlock( String blockName ) {

            if (_isTemplateExist == false) {
                throw new TemplateException( lang.get( "exTemplateNotExist" ) + ": " + _templatePath );
            }

            BlockToken blockToken = getBlockToken( blockName );
            if (blockToken == null) throw new TemplateException(
                string.Format( lang.get( "exBlockExist" ), blockName, _templatePath, "&lt;!-- BEGIN " + blockName + " --&gt;" )
                );

            ContentBlock block = new ContentBlock();
            block.Name = blockName;
            block._thisToken = blockToken;
            block._templatePath = _templatePath;

            // 将上级变量放入下级循环可用
            block._blockData = new BlockData( _blockData.getDic() );


            _blockData.addBlock( block );

            return block;
        }

        private BlockToken getBlockToken( String blockName ) {
            foreach (Token tk in _thisToken.getTokens()) {
                if (tk.getName() == blockName) return tk as BlockToken;
            }
            return null;

            // map 反而更慢
            // return _thisToken.getByName( blockName ) as BlockToken;
        }

        /// <summary>
        /// 获取模板绑定之后的最终结果
        /// </summary>
        /// <returns></returns>
        public override String ToString() {

            StringBuilder resultBuilder = new StringBuilder();
            addResultOne( resultBuilder, this.getData() );

            return resultBuilder.ToString();
        }

        internal void addResultOne( StringBuilder sb, BlockData data ) {
            foreach (Token tk in this.getTokens()) {
                tk.appendData( sb, this, data );
            }
        }

        //-----------------------------------------------------------------------------------

        /// <summary>
        /// 绑定对象列表
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="lbl"></param>
        /// <param name="objList"></param>
        public void BindList( String listName, String lbl, System.Collections.IList objList ) {

            this.bindPrivate( listName, objList );

            wojilu.Web.IBlock block = this.GetBlock( listName ) as wojilu.Web.IBlock;
            if (block == null) return;

            block.bindOtherFunc = this.bindOtherFunc;
            block.bindFunc = this.bindFunc;

            foreach (Object obj in objList) {
                block.Bind( lbl, obj );
                block.Next();
            }
        }


        public bindFunction bindFunc { get; set; }
        public otherBindFunction bindOtherFunc { get; set; }

    }
}
