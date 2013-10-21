<div style="padding:20px; background:#fff;">

<style>

.tdLeft { vertical-align:top;}
</style>

    <div class="formPanel">
    
   
    
    <form method="post" action="#{ActionLink}">    
        <table style="width:100%;">
            <tr><td colspan="2" class="formTitle">编辑器验证(编辑器名称:postContent)</td></tr>
            <tr><td class="tdLeft">请填写</td><td style="width:600px;">#{myEditor}            
            <span class="valid" msg="请填写内容" to="simpleContent"></span></td></tr>
            <tr><td></td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>

    <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
    
    <div>
    <div class="strong">客户端代码</div>
    <pre class="brush: c-sharp;">
    &lt;tr&gt;&lt;td class="tdLeft"&gt;请填写&lt;/td&gt;&lt;td&gt;&#35;{myEditor}&lt;/td&gt;&lt;/tr&gt;
    </pre>
    </div>
    
    <div style="margin:15px 0px;">
    <div class="strong">服务端代码</div>
    <pre class="brush: c-sharp;">    
        public void EditorFullbar() {
        
            // 这里第5个参数，是工具栏类型，这里是Full，表示全部显示。默认是Basic，只显示基本命令。
            editor( "myEditor", "simpleContent", "", "280px", wojilu.Editor.ToolbarType.Full );
            
        }
    </pre>
    </div>    
</div>
