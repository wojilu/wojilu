using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Serialization;
using System.Threading;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Demo {

    public class EditorOldController : ControllerBase {

        public override void Layout() {
        }


        public void EditorSimple() {
            editor( "simpleContent", "", "280px" );
            target( EditorSimpleSave );
        }

        public void EditorSimpleSave() {
            String html = ctx.PostHtml( "simpleContent" );
            if (strUtil.IsNullOrEmpty( html )) {
                echoRedirect( "请填写simpleContent内容" );
            }
            else {
                content( "<div>simpleContent</div><hr/>" + html );
            }
        }

        public void Editor() {
            editor( "postEditor", "postContent", "", "280px" );
            editor( "articleEditor", "articleContent", "", "280px" );
            set( "savePostLink", to( SavePostHtml ) );
            set( "saveArticleLink", to( SaveArticleHtml ) );
        }

        public void SavePostHtml() {
            String html = ctx.PostHtml( "postContent" );
            if (strUtil.IsNullOrEmpty( html )) {
                echoRedirect( "请填写postContent内容" );
            }
            else {
                content( "<div>postContent</div><hr/>" + html );
            }
        }

        public void SaveArticleHtml() {
            String html = ctx.PostHtml( "articleContent" );
            if (strUtil.IsNullOrEmpty( html )) {
                echoRedirect( "请填写articleContent内容" );
            }
            else {
                content( "<div>articleContent</div><hr/>" + html );
            }
        }

        public void EditorFullbar() {

            String content = @"
<div>abcd...eeeeeeeeeeeeeeeeeeeeeeeee</div>
<div>abcd...eeeee<span style=""font-size:18px;color:red;"">eeeeeeeeeeee</span>eeeeeeee</div>
<div>abcd...zzzzzzzzzzzzzzzzzz</div>
";
            if (strUtil.HasText( ctx.Post( "simpleContent" ) )) content = ctx.PostHtmlAll( "simpleContent" );

            target( EditorFullbar );

            editor( "myEditor", "simpleContent", content, "280px", wojilu.Editor.ToolbarType.Full );
        }

        public void EditorCustom() {
        }

        public void EditorUpload() {
        }

        public void EditorExt() {
            editor( "myEditor", "simpleContent", "", "280px", wojilu.Editor.ToolbarType.Basic );
            set( "savePostLink", to( EditorCode ) );
        }

        public void EditorCode() {
            content( ctx.PostHtml( "simpleContent" ) );
        }
        
    }

}
