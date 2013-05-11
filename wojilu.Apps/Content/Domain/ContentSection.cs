/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Serialization;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentSection : ObjectBase<ContentSection> {

        public static readonly int DefaultPostCount = 10;

        public int AppId { get; set; }
        public int OrderId { get; set; }

        public int RowId { get; set; }
        public int ColumnId { get; set; }

        public String Title { get; set; }
        public int HideTitle { get; set; }

        // SEO
        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }


        public String CombineIds { get; set; }

        public String MoreLink { get; set; }

        public String CssClass { get; set; } // 区块css类型class

        private int _listCount;

        public int ListCount {
            get {
                if (_listCount <= 0) return DefaultPostCount;
                return _listCount;
            }
            set { _listCount = value; }
        }

        public int CustomTemplateId { get; set; } // 自定义模板ID


        // 不使用 SOA ，直接使用controller产生内容
        public String SectionType { get; set; }


        public int ServiceId { get; set; } // 调用哪个服务对象
        public int TemplateId { get; set; } // 数据源对应的模板

        /// <summary>
        /// 需要给服务对象传递的参数(序列化的原始字符串，从0开始) 
        /// <para>param0=3;param1=strA;param2=2234</para>
        /// </summary>
        public String ServiceParams { get; set; }

        private Dictionary<string, string> _paramValues;

        // 返回 [键/值] 对的列表
        public Dictionary<string, string> GetServiceParamValues() {

            if (_paramValues == null) {
                _paramValues = getParamValues( ServiceParams );
            }
            return _paramValues;
        }

        public String GetServiceParamValue( String pname ) {
            if (this.GetServiceParamValues().ContainsKey( pname )) return this.GetServiceParamValues()[pname];
            return null;
        }

        // paramString
        // param0=3;param1=strA;param2=2234
        private Dictionary<string, string> getParamValues( String valueString ) {

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (strUtil.IsNullOrEmpty( valueString )) return dictionary;

            string[] strParams = valueString.Split( ';' );
            foreach (String paramItem in strParams) {

                if (strUtil.IsNullOrEmpty( paramItem )) continue;


                string[] pair = paramItem.Split( new char[] { '=' }, 2 );
                if (pair.Length == 1) {
                    dictionary[pair[0]] = "";
                }
                else {
                    dictionary[pair[0]] = pair[1];
                }

            }

            return dictionary;
        }

        //--------------------------------------------------------------------------

        public String Effect { get; set; }

        public String GetMarquee() {
            Object obj = this.getExt( "Effect", "marquee" );
            return obj == null ? null : obj.ToString();
        }

        public void SetMarquee( String val ) {
            this.setExt( "Effect", "marquee", val );
        }

    }
}

