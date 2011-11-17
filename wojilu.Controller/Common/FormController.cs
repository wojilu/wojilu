using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Common {

    public class FormController : ControllerBase {

        public void TextStyle() {
        }

        public static String GetTitleStyle( MvcContext ctx ) {

            String fontStyle = ctx.Post( "fontStyle" );
            String fontColor = ctx.Post( "fontColor" );
            String fontSize = ctx.Post( "fontSize" );
            String fontFamily = ctx.Post( "fontFamily" );

            StringBuilder builder = new StringBuilder();

            if (strUtil.HasText( fontColor )) {
                builder.Append( "color:" );
                builder.Append( fontColor );
                builder.Append( ";" );
            }

            if (strUtil.HasText( fontSize )) {
                builder.Append( "font-size:" );
                builder.Append( fontSize );
                builder.Append( ";" );
            }

            if (strUtil.HasText( fontFamily )) {
                builder.Append( "font-family:" );
                builder.Append( fontFamily );
                builder.Append( ";" );
            }

            if (strUtil.HasText( fontStyle )) {
                builder.Append( fontStyle.Replace( ",", ";" ) );
            }

            return builder.ToString();
        }

    }

}
