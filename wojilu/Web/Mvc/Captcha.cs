using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 验证码控件
    /// </summary>
    /// <remarks>验证码控件(Captcha=Completely Automated Public Turing Test To Tell Computers and Humans Apart)</remarks>
    /// <example>
    /// <code>
    /// Html.Captcha.ToString() -->获取呈现验证码的 html 和 js
    /// Html.Captcha.Html()  -->获取呈现验证码的 html 和 js
    /// Html.Captcha.HasError() -->验证码是否有误
    /// Html.Captcha.Code() -->正确的验证码
    /// Html.Captcha.UserCode() ->用户实际填写的验证码
    /// </code>
    /// </example>
    public class Captcha {

        /// <summary>
        /// 验证码控件的html (包括一个input  + 右侧的一个验证码 + 点击刷新机制)
        /// 如果要使用ajax验证，必须给form启用 ajaxPostForm 验证
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return this.Html();
        }

        // 避免同一页面的多个验证码冲突
        private int getCount() {

            String CaptchaCountKey = "CaptchaCount";
            object objCount = CurrentRequest.getItem( CaptchaCountKey );
            if (objCount == null) {
                objCount = 1;
                CurrentRequest.setItem( CaptchaCountKey, objCount );
                return 1;
            }

            return (int)objCount + 1;
        }

        /// <summary>
        /// 验证码控件的html (包括一个input  + 右侧的一个验证码 + 点击刷新机制)
        /// 如果要使用ajax验证，必须给form启用 ajaxPostForm 验证
        /// </summary>
        /// <returns></returns>
        public String Html() {

            StringBuilder sb = new StringBuilder();

            int count = getCount();

            sb.Append( "<span id=\"captchaWrap" );
            sb.Append( count );
            sb.Append( "\"><input name=\"" );
            sb.Append( inputName );
            sb.Append( "\" class=\"" );
            sb.Append( inputName );
            sb.Append( "\" type=\"text\"> <span class=\"valid\" msg=\"" );
            sb.Append( lang.get( "exPlsFill" ) );
            sb.Append( "\" ajaxAction=\"" );
            sb.Append( getCheckUrl() );
            sb.Append( "\"></span> <img class=\"validationImg\" src=\"" );
            sb.Append( getCodeImgUrl() );
            sb.Append( "\" style=\"cursor:pointer\"/> <span class=\"refreshImg link\">" );
            sb.Append( lang.get( "refreshPic" ) );
            sb.Append( "</span><span>" );

            sb.Append( "<script type=\"text/javascript\">_run(function(){" );
            sb.Append( "$('#captchaWrap" );
            sb.Append( count );
            sb.Append( " .validationImg').click( function() {" );
            sb.Append( "wojilu.tool.refreshImg($(this));" );
            sb.Append( "});" );
            sb.Append( "$('#captchaWrap" );
            sb.Append( count );
            sb.Append( " .refreshImg').click( function() {" );
            sb.Append( "wojilu.tool.refreshImg($(this).prev());" );
            sb.Append( "});" );
            sb.Append( "" );
            sb.Append( "});</script>" );

            return sb.ToString();
        }

        private static String getCheckUrl() {
            return strUtil.Join( sys.Path.Root, "CaptchaImage.ashx" ) + "?check=true";
        }

        private static String getCodeImgUrl() {
            return strUtil.Join( sys.Path.Root, "CaptchaImage.ashx" );
        }

        //------------------------------------------------------------------------------------

        /// <summary>
        /// 检查用户提交的验证码是否错误
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public Boolean CheckError( MvcContext ctx ) {
            String validationText = ClientCode( ctx );
            if (isCodeError( validationText, ctx )) {
                ctx.errors.Add( lang.get( "exValidateCodeError" ) );
                return true;
            }
            return false;
        }

        private Boolean isCodeError( String userCode, MvcContext ctx ) {
            if (strUtil.IsNullOrEmpty( userCode )) return true;

            String serverCode = CorrectCode( ctx );
            return !strUtil.EqualsIgnoreCase( userCode, serverCode );
        }

        /// <summary>
        /// 服务器端正确的验证码
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public String CorrectCode( MvcContext ctx ) {
            return ctx.web.SessionGet( sessionName ) as string;
        }

        /// <summary>
        /// 用户实际提交的验证码
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public String ClientCode( MvcContext ctx ) {
            return ctx.Post( inputName );
        }

        public static readonly String inputName = "ValidationText";
        public static readonly String sessionName = "CaptchaImageText";

    }


}
