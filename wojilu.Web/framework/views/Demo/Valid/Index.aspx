<div style="padding:20px; background:#fff;">

<style>
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}
</style>

    <h2>完整语法：</h2>


    <pre class="brush: html;">
        &lt;span class="valid" to="name" type="select" rule="email" msg="请填写用户名" show="true" ajaxAction="url"&gt;&lt;/span&gt;
    </pre>
    <div style=" margin:10px;">其中span也可以写成div，看你的实际布局需要。</div>

    <h2 style="margin-bottom:10px;">边框高亮模式：</h2>
    <div>从1.9开始，边框模式可以简化为：</div>
    <pre class="brush: html;">
        &lt;span class="valid border" 其他验证属性xxx&gt;&lt;/span&gt;
    </pre>
    
    <div>当然，原来的 <strong>mode="border"</strong> 仍然有效</div>

    <div class="formPanel" style="margin-top:30px;">
    <form>    
        <table class="table">
            <tr><td colspan="2" class="formTitle">文本控件验证(input)</td></tr>
            <tr><td class="tdLeft">普通模式</td><td><input name="title" type="text" /> <span class="valid" msg="请填写内容"></span></td></tr>
            <tr><td class="tdLeft">边框高亮模式</td><td><input name="content" type="text" /><span class="valid border"></span></td></tr>
            <tr><td></td><td><input id="Submit1" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>
    
    <div class="formPanel">
    <form>    
        <table class="table">
            <tr><td colspan="2" class="formTitle">textarea验证</td></tr>
            <tr><td class="tdLeft">普通模式</td><td><textarea name="title" type="text" ></textarea>
            <div class="valid" msg="请填写内容"></div>
            </td></tr>
            <tr><td class="tdLeft">边框高亮模式</td><td><textarea name="content" type="text" ></textarea>
            <div class="valid border"></div>
            </td></tr>
            <tr><td></td><td><input id="Submit2" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>
    
    <div class="formPanel">
    <form>
        <div class="strong">将valid控件写在其他地方</div>
        <div class="top5">
            <div class="valid" to="title" msg="请填写标题"></div>
            <div class="valid top5 bottom5" to="content" msg="请填写内容"></div>
        </div>
        <table class="table">
            <tr><td class="tdLeft">标题</td><td><input name="title" type="text" /></td></tr>
            <tr><td class="tdLeft">内容</td><td><input name="content" type="text" /></td></tr>

            <tr><td></td><td><input id="Submit3" type="submit" value="submit" class="btn btn-primary" /></td></tr>        
        </table>
    </form>
    </div>
    


</div>



