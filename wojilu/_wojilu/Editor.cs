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
using wojilu.Web.Mvc;

namespace wojilu {

    public class EditorFactory {

        public static IEditor NewOne( String propertyName, String propertyValue, String height, Editor.ToolbarType toolbar ) {

            IEditor x = new UEditor();
            x.Init( propertyName, propertyValue, height, toolbar );
            return x;
        }
    }

    public interface IEditor {
        void AddUploadUrl( MvcContext ctx );
        void Init( String propertyName, String propertyValue, String height, Editor.ToolbarType toolbar );
        Boolean IsUnique { get; set; }
    }

    public class UEditor : IEditor {

        private String _propertyName;
        private String _propertyValue;
        private String _height;
        private Editor.ToolbarType _toolbar;

        public void Init( String propertyName, String propertyValue, String height, Editor.ToolbarType toolbar ) {
            _propertyName = propertyName;
            _propertyValue = propertyValue;
            _height = height;
            _toolbar = toolbar;
        }

        public void AddUploadUrl( MvcContext ctx ) {
        }

        public Boolean IsUnique { get; set; }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "<script type=\"text/plain\" id=\"{0}\" name=\"{0}\">", _propertyName );
            sb.Append( _propertyValue );
            sb.Append( "</script>" );

            sb.Append( "<script>" );
            sb.Append( "_run(function(){" );

            int line = (_toolbar == Editor.ToolbarType.Basic ? 1 : 2);
            String height = "";
            if (strUtil.HasText( _height )) {
                height = strUtil.TrimEnd( _height.Trim(), "px" ).Trim();
                height = ".height("+height+")";
            }

            sb.AppendFormat( "wojilu.editor.bind('{0}').line({1}){2}.show();", _propertyName, line, height );

            sb.Append( "})" );
            sb.Append( "</script>" );

            return sb.ToString();
        }

    }


    /// <summary>
    /// 在 web 中使用的富文本编辑器
    /// </summary>
    public class Editor : IEditor {

        public void Init( String propertyName, String propertyValue, String height, Editor.ToolbarType toolbar ) {

            _controlName = propertyName;
            _content = propertyValue;
            _height = height;
            _Toolbar = toolbar;

            _width = "100%";
            _editorPath = sys.Path.Editor;

            _isUnique = true;
            _jsVersion = MvcConfig.Instance.JsVersion;

        }

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

        public String RelativePath {
            get { return strUtil.TrimStart( this.EditorPath, sys.Path.Js ); }
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
        /// 需要编辑的内容(html格式)
        /// </summary>
        public String Content {
            get {
                if (strUtil.HasText( _content )) {
                    return strUtil.EncodeTextarea( _content );
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// 编辑器名称。在其他页面和编辑器页面进行交互的时候，需要这个名称。
        /// </summary>
        public String EditVarName {
            get { return String.Format( "{0}Editor", ControlName.Replace( ".", "" ) ); }
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

        public Editor() {
        }

        private String Render() {

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat( "<div id=\"{0}\">", this.ControlName.Replace( ".", "_" ) + "Editor" );

            builder.AppendFormat( "<textarea id=\"{0}\" name=\"{0}\" class=\"editor-textarea\" style=\"display:none;height:" + this.Height + ";\">{1}</textarea>", this.ControlName, this.Content );

            builder.Append( "<script>_run(function(){require([\"" + RelativePath + "editor\"],function(){" );
            builder.AppendLine();
            builder.Append( "window." + EditVarName + "=new wojilu.editor( {editorPath:'" + this.EditorPath + "', height:'" + this.Height + "', name:'" + this.ControlName + "', content:'', toolbarType:'" + this.Toolbar.ToString().ToLower() + "', uploadUrl:'" + this.UploadUrl + "', mypicsUrl:'" + this.MyPicsUrl + "' } );" + EditVarName + ".render();" );
            builder.AppendLine();
            builder.Append( "})});</script>" );

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

