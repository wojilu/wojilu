using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Demo {

    public class JsonTestController : ControllerBase {


        public void JsonTest() {
            set( "jsonLink", to( JsonResult ) );
        }

        public void JsonResult() {

            // 获取客户端提交的值
            int id = ctx.PostInt( "Id" );

            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add( "Name", "你选择了" + id );

            // 使用JsonString的Convert方法构造json字符串，可以避免手工拼接的错误
            String jsonStr = Json.ToString( dic );

            // 使用 echoJson 方法返回 json 字符串，客户端jquery可以直接使用
            echoJson( jsonStr );
        }

    }
}
