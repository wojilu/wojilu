using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Serialization;

namespace wojilu.Web.Utils {


    public class WebHelper {


        /// <summary>
        /// 获取 flash 的 html 嵌入代码。TODO http://code.google.com/p/swfobject/wiki/documentation 
        /// </summary>
        /// <param name="srcUrl">flash 的网址</param>
        /// <param name="width">flash 的宽度，整数值</param>
        /// <param name="height">flash 的高度，整数值</param>
        /// <returns>返回嵌入 flash 的 html 代码</returns>
        public static String GetFlash( String srcUrl, Object width, Object height ) {

            return string.Format( "<embed src=\"{0}\" allowFullScreen=\"true\" quality=\"high\" width=\"{1}\" height=\"{2}\" align=\"middle\" allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\"></embed>", srcUrl, width, height );

        }


        /// <summary>
        /// 得到表情名称和图片网址的键值对 name=>picPath
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetEmotions() {
            return Emotions.GetEmotions();
        }




    }

}
