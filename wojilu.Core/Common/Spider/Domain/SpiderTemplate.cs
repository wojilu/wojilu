using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.Common.Spider.Domain {

    public class TemplateAndLog {

        public SpiderTemplate Template { get; set; }
        public StringBuilder log { get; set; }

    }

    public class SpiderTemplate : ObjectBase<SpiderTemplate>, ISort {

        public String SiteName { get; set; }
        public String SiteUrl { get; set; }

        public String ListUrl { get; set; }

        public String ListEncoding { get; set; }
        public String DetailEncoding { get; set; }

        public int IsSavePic { get; set; }

        public String SpiderType { get; set; } // 自定义抓取工具

        //--------------------------------------------------------------

        [LongText]
        [HtmlText]
        public String ListBodyBegin { get; set; }

        [LongText]
        [HtmlText]
        public String ListBodyEnd { get; set; }

        [LongText]
        [HtmlText]
        public String ListBodyPattern { get; set; }

        public String GetListBodyPattern() {
            if (strUtil.HasText( this.ListBodyPattern )) return this.ListBodyPattern;
            return this.ListBodyBegin + ".+?" + this.ListBodyEnd;
        }

        //--------------------------------------------------------------

        [LongText]
        [HtmlText]
        public String ListPattern { get; set; }

        //--------------------------------------------------------------

        [LongText]
        [HtmlText]
        public String DetailPattern { get; set; }

        [LongText]
        [HtmlText]
        public String DetailBegin { get; set; }

        [LongText]
        [HtmlText]
        public String DetailEnd { get; set; }

        public String GetDetailPattern() {
            if (strUtil.HasText( this.DetailPattern )) return this.DetailPattern;
            return this.DetailBegin + "(.+?)" + this.DetailEnd;
        }

        /// <summary>
        /// 详细页需要清除的tag。比如 "script,iframe,frame,object,table,font,span,img,a,br" 等
        /// </summary>
        [LongText]
        public String DetailClearTag { get; set; }

        //--------------------------------------------------------------


        public DateTime Created { get; set; }

        public int IsDelete { get; set; }

        public int OrderId  { get; set; }

        public void updateOrderId() {
            this.update();
        }

    }

}
