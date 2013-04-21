using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Demo {

    public class EditorController : ControllerBase {

        public void Index() {
        }

        public void SimpleDemo() {
            target( SimpleSave );
        }

        public void SimpleSave() {

            // 如果编辑器没有指定name，则默认使用editorValue这个名称
            //String editorBody = ctx.PostHtml( "editorValue" );

            String editorBody = ctx.PostHtml( "post.Content" );

            content( editorBody );
        }


        public void AjaxForm() {
            target( AjaxFormSave );
        }

        public void AjaxFormSave() {
            String content = ctx.PostHtml( "post.Content" );
            echoError( content );
        }

        public void AutoHeight() {
            target( SimpleSave );
        }

        //-----------------------------------------------------------------------

        public void CustomPluginDemo() {
        }

        public void SubmitFormDemo() {
            set( "ActionLink1", to( SaveFormOne ) );
            set( "ActionLink2", to( SaveFormTwo ) );
        }

        public void SaveFormOne() {
            echo( "编辑器内容=" + ctx.PostHtml( "myEditor" ) );
        }

        public void SaveFormTwo() {
            echo( "编辑器内容=" + ctx.PostHtml( "myEditor1" ) );
        }

        public void ResetDemo() {
        }

        public void TextareaDemo() {
        }

        public void CompleteDemo() {
        }

        public void MultiDemo() {
        }

        public void CustomToolbarDemo() {
        }

        public void HighlightDemo() {
        }

        public void RenderInTable() {
        }

        public void JQueryCompleteDemo() {
        }

        public void JQueryValidation() {
        }

        public void IosFiveDemo() {
        }

        public void Uparsedemo() {
        }

    }
}
