using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Demo {

    public class TPObject {
        public int Age { get; set; }
        public String Name { get; set; }
    }

    public class CtxPostController : ControllerBase {

        public void Index() {

            set( "objectLink", to( SaveObject ) );
            set( "objectLabelLink", to( SaveObjectLabel ) );

            set( "valueLink", to( SaveValue ) );
            set( "valueLabelLink", to( SaveValueLabel ) );
        }

        public void SaveObject() {
            TPObject obj = ctx.PostObject<TPObject>();
            echoText( obj.Age + "\n" + obj.Name );
        }

        public void SaveObjectLabel() {
            TPObject obj = ctx.PostObject<TPObject>( "x1" );
            echoText( obj.Age + "\n" + obj.Name );
        }

        public void SaveValue() {
            String tname = strUtil.GetCamelCase( typeof( TPObject ).Name );
            TPObject obj = ctx.PostValue<TPObject>();
            echoText( "camelCase=" + tname + "\n" + obj.Age + "\n" + obj.Name );
        }
        public void SaveValueLabel() {
            TPObject obj = ctx.PostValue<TPObject>( "x2" );
            echoText( obj.Age + "\n" + obj.Name );
        }

    }

}
