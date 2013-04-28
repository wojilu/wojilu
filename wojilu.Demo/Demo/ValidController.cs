using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Serialization;
using System.Threading;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Demo {

    public class ValidController : ControllerBase {

        public override void Layout() {
        }


        public void HttpMethod() {
            set( "postLink", to( SaveHttp ) );
        }

        public void SaveHttp() {
            echo( "当前提交方法："+ctx.HttpMethod );
        }

        public void HtmlPost() {
            target( HtmlPostShow );
        }

        public void HtmlPostShow() {

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<String, String> kv in wojilu.Web.Utils.Tags.TagWhitelist.GetInstance()) {
                sb.Append( kv.Key + "<br/>" );
            }

            String str = ctx.PostHtml( "content" );

            echoText( sb + "<hr>" + str );
        }


        private static readonly String cookieName = "__wojilu_demo_cookie_test";

        public void Cookie() {

            set( "addCookieLink", to( CookieAdd ) );
            set( "deleteCookieLink", to( CookieDelete ) );

            // 2) 【获取】根据自定义的cookie名称，获取在客户端加密存储的 userId
            set( "userId", ctx.web.UserId( cookieName ) );

        }

        public void CookieAdd() {

            // 1) 【增加】添加一个名为 cookieName 的自定义加密cookie，同时将cookie的值 888 放入
            ctx.web.UserLogin( cookieName, 888, "zhangsan", wojilu.Common.LoginTime.Never );

            echoRedirect( "操作成功", Cookie );
        }

        public void CookieDelete() {

            // 3) 【删除】删除名为 cookieName 的自定义加密cookie 
            ctx.web.UserLogout( cookieName );

            echoRedirect( "操作成功", Cookie );
        }

        public void Slider() {
        }

        //----------------------------------------------------------------------------------------------------------------

        public void Index() {
        }

        public void Editor() {

            set( "ActionLink1", to( EdiotrShow ) );
            set( "ActionLink2", to( EdiotrShowAjax ) );
        }

        public void EdiotrShow() {
            String postContent = ctx.PostHtml( "postContent" );
            content( "您输入的内容="+ postContent );
        }

        public void EdiotrShowAjax() {
            echoError( "您输入的内容=" + ctx.PostHtml( "myContent" ) );
        }

        public void Tip() {
        }

        public void Pwd() {
        }

        public void Rule() {
        }

        public void CustomRule() {
        }

        public void Ajax() {
            set( "ajaxCheckUrl", to( AjaxCheck ) );
        }

        public void AjaxCheck() {

            String userName = ctx.Post( "userName" );
            if ("孙中山".Equals( userName )) {
                echoJsonMsg( "验证成功", true, null );
            }
            else {
                echoJsonMsg( "验证错误", false, null );
            }
        }


        public void Drop() {

            Dictionary<string, string> book = new Dictionary<string, string>();
            book.Add( "请选择", "" );
            book.Add( "小说", "1" );
            book.Add( "诗歌", "2" );
            book.Add( "散文", "3" );
            book.Add( "戏剧", "4" );

            dropList( "book", book, null );


            Dictionary<string, string> music = new Dictionary<string, string>();
            music.Add( "请选择", "" );
            music.Add( "民谣", "1" );
            music.Add( "流行", "2" );
            music.Add( "摇滚", "3" );
            music.Add( "电子", "4" );

            dropList( "music", music, null );
        }

        public void Checkbox() {

            Dictionary<string, string> book = new Dictionary<string, string>();
            book.Add( "小说", "1" );
            book.Add( "诗歌", "2" );
            book.Add( "散文", "3" );
            book.Add( "戏剧", "4" );

            checkboxList( "book", book, null );

            Dictionary<string, string> music = new Dictionary<string, string>();
            music.Add( "民谣", "1" );
            music.Add( "流行", "2" );
            music.Add( "摇滚", "3" );
            music.Add( "电子", "4" );

            checkboxList( "music", music, null );
        }


        public void RadioList() {

            Dictionary<string, string> book = new Dictionary<string, string>();
            book.Add( "小说", "1" );
            book.Add( "诗歌", "2" );
            book.Add( "散文", "3" );
            book.Add( "戏剧", "4" );

            radioList( "book", book, null );

            Dictionary<string, string> music = new Dictionary<string, string>();
            music.Add( "民谣", "1" );
            music.Add( "流行", "2" );
            music.Add( "摇滚", "3" );
            music.Add( "电子", "4" );

            radioList( "music", music, null );
        }

        //----------------------------------------------------------------------------------------------------------------
        
        public void Box() {
            view( "BoxShow" );
            set( "boxLink", to( BoxBody ) );
        }

        public void BoxBox() {
        }

        public void BoxBody() {
        }

        //---------------

        public void BoxReturn() {
            set( "boxLink", to( BoxReturnBody ) );
        }

        public void BoxReturnBody() {
        }
        //---------------

        // 这是父页面
        public void BoxRefresh() {
            set( "boxLink", to( BoxRefreshBody ) );
        }

        // 这是弹窗页面
        public void BoxRefreshBody() {
            echoToParent( "操作成功，刷新父页面" ); // 显示提示信息，然后刷新弹窗的父页面
            // 你也可以使用 echoToParent( String msg, String url ) 指定父页面跳转的网址
        }

        //----------------------------------------------------------------------------------------------------------------

        public void FrmLink() {
        }

        public void FrmLinkRedirect() {
            view( "Box" );
        }

        //----------------------------------------------------------------------------------------------------------------

        public void AjaxForm() {
            target( AjaxFormSave );
        }

        public void AjaxFormSave() {

            Thread.Sleep( 1000 * 3 );

            if (strUtil.IsNullOrEmpty( ctx.Post( "Name" ) )) {
                echoError( "请填写Name" );
                return;
            }

            Thread.Sleep( 1000 * 3 );
            echoRedirect( "操作成功", Index );
        }

        public void AjaxFormRedirect() {
            view( "Box" );
        }

        public void AjaxFormUpdate() {
            view( "Box" );
        }

        public void AjaxDelete() {
            view( "Box" );
        }

        //----------------------------------------------------------------------------------------------------------------



        public void Tab( int id ) {
        }

        public void Tree() {
            view( "Box" );
        }

        public void Calendar() {
            view( "Box" );
        }

        public void ProcessBar() {
            view( "Box" );
        }


        //----------------------------------------------------------------------------------------------------------------

        public void DataGrid() {
            view( "Box" );
        }

        public void Pager() {
            view( "Box" );
        }

        public void PagerList() {
            view( "Box" );
        }

        public void PagerCustom() {
            view( "Box" );
        }

        //----------------------------------------------------------------------------------------------------------------

    }

}
