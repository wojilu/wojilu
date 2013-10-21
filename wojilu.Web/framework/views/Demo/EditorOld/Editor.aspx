<div style="padding:20px; background:#fff;">

<style>

.tdLeft { vertical-align:top;}
</style>


    <div class="formPanel">
    <form method="post" action="#{savePostLink}">    
        <table style="width:100%;">
            <tr><td colspan="2" class="formTitle">编辑器验证(编辑器名称:postEditor)</td></tr>
            <tr><td class="tdLeft">普通模式</td><td style="width:600px;">#{postEditor}            
            <span class="valid" msg="请填写内容" to="postContent"></span></td></tr>
            <tr><td></td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>

    <div class="formPanel">
    <form method="post" action="#{saveArticleLink}">    
        <table style="width:100%;">
            <tr><td colspan="2" class="formTitle">编辑器验证(编辑器名称:articleEditor)</td></tr>
            <tr><td class="tdLeft">边框模式</td><td style="width:600px;">#{articleEditor}           
            <span class="valid" mode="border" to="articleContent"></span></td></tr>
            <tr><td></td><td><input id="Submit2" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>
    
    <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
    
    <div>
    <div class="strong">客户端代码</div>
    <div>第一个编辑器</div>
    <pre class="brush: c-sharp;">
    &lt;tr&gt;&lt;td class="tdLeft"&gt;普通模式&lt;/td&gt;&lt;td&gt;&#35;{postEditor}&lt;/td&gt;&lt;/tr&gt;
    </pre>
    
    <div>第二个编辑器</div>
    <pre class="brush: c-sharp;">
    &lt;tr&gt;&lt;td class="tdLeft"&gt;边框模式&lt;/td&gt;&lt;td&gt;&#35;{articleEditor}&lt;/td&gt;&lt;/tr&gt;
    </pre>
    </div>
    
    <div>
    <div class="strong">服务端代码</div>
    <pre class="brush: c-sharp;">
        public void Editor() {
            // 如果有多个编辑器，必须提供编辑器名称，各编辑器名称不能相同。
            editor( "postEditor", "postContent", "", "280px" ); // 第一个参数是编辑器名称，第二个是控件名称
            editor( "articleEditor", "articleContent", "", "280px" ); // 第一个参数是编辑器名称，第二个是控件名称
            set( "savePostLink", to( SavePostHtml ) );
            set( "saveArticleLink", to( SaveArticleHtml ) );
        }

        public void SavePostHtml() {
            String html = ctx.PostHtml( "postContent" ); // 根据控件名称获取值
            if (strUtil.IsNullOrEmpty( html )) {
                echoRedirect( "请填写postContent内容" );
            }
            else {
                actionContent( "&lt;div&gt;postContent&lt;/div&gt;&lt;hr/&gt;" + html );
            }
        }

        public void SaveArticleHtml() {
            String html = ctx.PostHtml( "articleContent" ); // 根据控件名称获取值
            if (strUtil.IsNullOrEmpty( html )) {
                echoRedirect( "请填写articleContent内容" );
            }
            else {
                actionContent( "&lt;div&gt;postContent&lt;/div&gt;&lt;hr/&gt;" + html );
            }
        }
    </pre>
    </div>
    
</div>
