<div style="padding:20px; background:#fff;">

<style>
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}
</style>
    <div></div>

    <div class="formPanel">
    <form method="post" action="#{savePostLink}">    
        <table  class="table">
            <tr><td colspan="2" class="formTitle">ajax验证</td></tr>
            <tr><td class="tdLeft">请输入</td><td>
                <input name="userName" type="text" /> <span class="valid" msg="请填写内容" ajaxAction="#{ajaxCheckUrl}"></span></td></tr>
            <tr><td></td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
        
        <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
        
        <div>
        <div class="strong">客户端</div>
        <div style="margin-bottom:20px;">
            <div>只要加上ajaxAction属性即可，此属性必须指向服务端验证url</div>
            <pre class="brush: c-sharp;">
                &lt;input name="userName" type="text" /&gt;
                &lt;span class="valid" msg="请填写内容" ajaxAction="&#35;{ajaxCheckUrl}"&gt;&lt;/span&gt;
            </pre>
        
        </div>
        
        <div class="strong">服务端代码</div>
        <pre class="brush: c-sharp;">

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
        </pre>
        </div>
        
        
    </form>
    </div>

    
</div>

