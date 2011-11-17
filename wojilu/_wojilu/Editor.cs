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
using System.Text;
using wojilu.Web.Context;

namespace wojilu {

    /// <summary>
    /// 在 web 中使用的富文本编辑器
    /// </summary>
    public class Editor {

        /// <summary>
        /// 工具栏类型
        /// </summary>
        public enum ToolbarType {

            /// <summary>
            /// 基本按钮
            /// </summary>
            Basic,

            /// <summary>
            /// 全部按钮
            /// </summary>
            Full
        }

        private String _content;
        private String _controlName;
        private String _height;
        private String _editorPath;
        private ToolbarType _Toolbar;
        private String _width;

        private Boolean _isUnique = false;
        private String _jsVersion;

        private String _uploadUrl;
        private String _mypicsUrl;

        /// <summary>
        /// 图片上传网址
        /// </summary>
        public String UploadUrl {
            get { return _uploadUrl; }
            set { _uploadUrl = value; }
        }

        /// <summary>
        /// 当前用户的所有图片的网址
        /// </summary>
        public String MyPicsUrl {
            get { return _mypicsUrl; }
            set { _mypicsUrl = value; }
        }

        public Boolean IsUnique {
            get { return _isUnique; }
            set { _isUnique = value; }
        }

        /// <summary>
        /// 编辑器名称
        /// </summary>
        public String ControlName {
            get { return _controlName; }
        }

        /// <summary>
        /// 编辑器文件(js、css、图片等)所在路径
        /// </summary>
        public String EditorPath {
            get { return _editorPath; }
        }

        private String FrameId {
            get { return String.Format( "wojiluEditor_{0}_frame", ControlName ); }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public String Height {
            get { return _height; }
        }

        /// <summary>
        /// 工具栏类型
        /// </summary>
        public ToolbarType Toolbar {
            get { return _Toolbar; }
            set { _Toolbar = value; }
        }

        /// <summary>
        /// 宽度，默认是父容器的100%
        /// </summary>
        public String Width {
            get { return _width; }
        }

        /// <summary>
        /// 编辑器变量名称(js中使用)
        /// </summary>
        public String EditVarName {
            get { return String.Format( "{0}Editor", ControlName.Replace( ".", "" ) ); }
        }

        /// <summary>
        /// 需要编辑的内容(html格式)
        /// </summary>
        public String Content {
            get {
                if (strUtil.HasText( _content )) {
                    //return _content.Replace( "'", "&prime;" ).Replace( "\n", "" ).Replace( "\r", "" );
                    return strUtil.EncodeTextarea( _content );
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// 设置图片上传网址
        /// </summary>
        /// <param name="ctx"></param>
        public void AddUploadUrl( MvcContext ctx ) {

            Object objuploadUrl = ctx.GetItem( "editorUploadUrl" );
            Object objmypicsUrl = ctx.GetItem( "editorMyPicsUrl" );

            this.UploadUrl = objuploadUrl == null ? "" : objuploadUrl.ToString();
            this.MyPicsUrl = objmypicsUrl == null ? "" : objmypicsUrl.ToString();

        }

        private Editor( String controlName, String content, String width, String height, String editorPath, ToolbarType toolbarType ) {
            _controlName = String.Format( "{0}", controlName );
            _content = content;
            _width = width;
            _height = height;
            _editorPath = editorPath;
            _Toolbar = toolbarType;
        }

        /// <summary>
        /// 创建编辑器(页面中第一个)
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="content"></param>
        /// <param name="height"></param>
        /// <param name="editorPath"></param>
        /// <param name="jsVersion"></param>
        /// <param name="toolbarType"></param>
        /// <returns></returns>
        public static Editor NewOne( String controlName, String content, String height, String editorPath, String jsVersion, ToolbarType toolbarType ) {
            Editor result = new Editor( controlName, content, "100%", height, editorPath, toolbarType );
            result._isUnique = true;
            result._jsVersion = jsVersion;
            return result;
        }

        private String Render() {
            StringBuilder builder = new StringBuilder();
            if (this.IsUnique) {
                builder.AppendFormat( "<script src=\"{0}editor.js{1}\" type=\"text/javascript\"></script>", EditorPath, getJsVersionString() );
            }

            builder.AppendFormat( "<div id=\"{0}\">", this.ControlName.Replace( ".", "_" ) + "Editor" );

            //builder.Append( "<script type=\"text/javascript\">var " + EditVarName + "=new wojilu.editor( {editorPath:'" + this.EditorPath + "', height:'" + this.Height + "', name:'" + this.ControlName + "', content:'" + this.Content.Replace( "\\", "\\\\" ) + "', toolbarType:'" + this.Toolbar.ToString().ToLower() + "', uploadUrl:'" + this.UploadUrl + "', mypicsUrl:'" + this.MyPicsUrl + "' } );" + EditVarName + ".render();</script>" );

            builder.AppendFormat( "<textarea id=\"{0}\" name=\"{0}\" style=\"display:none;width:99%;height:"+this.Height+";\">{1}</textarea>", this.ControlName, this.Content );

            builder.Append( "<script type=\"text/javascript\">var " + EditVarName + "=new wojilu.editor( {editorPath:'" + this.EditorPath + "', height:'" + this.Height + "', name:'" + this.ControlName + "', content:'', toolbarType:'" + this.Toolbar.ToString().ToLower() + "', uploadUrl:'" + this.UploadUrl + "', mypicsUrl:'" + this.MyPicsUrl + "' } );" + EditVarName + ".render();</script>" );

            builder.Append( "</div>" );

            return builder.ToString();
        }

        /// <summary>
        /// 编辑器生成的js和html内容
        /// </summary>
        /// <returns></returns>
        public override String ToString() {
            return Render();
        }

        private String getJsPath() {
            String result = strUtil.TrimEnd( this.EditorPath, "/" );
            return strUtil.TrimEnd( result.ToLower(), "editor" );
        }

        private String getJsVersionString() {
            return strUtil.HasText( _jsVersion ) ? "?v=" + _jsVersion : "";
        }


    }
}

