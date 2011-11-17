using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Mvc;
using wojilu.Web;

namespace wojilu.cms {
    
    public class RenderHelper : IMvcFilter {

        public void Process( MvcEventPublisher publisher ) {
            publisher.Begin_Render += new EventHandler<MvcEventArgs>( publisher_Begin_Render );
        }

        void publisher_Begin_Render( object sender, MvcEventArgs e ) {

            // 获取即将输出的内容
            string output = e.ctx.utils.getCurrentOutputString();

            // 修改输出的内容
            output = output.Replace( "#{pageElapsedMilliseconds}", WebStopwatch.Stop().ElapsedMilliseconds.ToString( "0.0000" ) );

            // 将修改后的内容放回上下文
            e.ctx.utils.setCurrentOutputString( output );

        }

    }

}
