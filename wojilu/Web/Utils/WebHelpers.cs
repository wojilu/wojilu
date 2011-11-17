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

            // 简单形式
            //return "<object data=\"" + srcUrl + "\" type=\"application/x-shockwave-flash\" width=\"" + width + "\" height=\"" + height + "\"><param name=\"movie\" value =\"" + srcUrl + "\" /></object>";

            return string.Format( "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0\" width=\"{1}\" height=\"{2}\"><param name=\"wmode\" value=\"opaque\"/><param name=\"movie\" value=\"{0}\" /><param name=\"quality\" value=\"high\" /><embed src=\"{0}\" quality=\"high\" pluginspage=\"http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash\" type=\"application/x-shockwave-flash\" width=\"{1}\" height=\"{2}\"></embed></object>", srcUrl, width, height );

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
