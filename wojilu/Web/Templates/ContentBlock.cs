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

        internal List<Token> getTokens() {
            return _thisToken.getTokens();
        }

        private List<BlockData> _blockdataList = new List<BlockData>();
        private BlockData _blockData = new BlockData( new Dictionary<String, object>() );

        internal BlockData getData() {
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
        /// <param name="val">被赋予的值，会被转化成字符串</param>
        public void Set( String lbl, Object val ) {

            if (val == null) {
                this.Set( lbl, "" );
            }

            if (val is DateTime) {
                this.Set( lbl, ((DateTime)val).ToString( "g" ) );
            }
            else if (val is decimal) {
                this.Set( lbl, ((decimal)val).ToString( "n2" ) );
            }
            else {
                this.Set( lbl, val.ToString() );
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

            if (this.bindOtherFunc != null)
                bindOtherFunc( this, lbl, obj );

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
                string.Format( lang.get( "exBlockExist" ) , blockName, _templatePath, "&lt;!-- BEGIN " + blockName+ " --&gt;" )                
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
