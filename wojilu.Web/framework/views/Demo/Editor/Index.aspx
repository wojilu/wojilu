
<style>
    .editor-wrap {margin:30px;}
    .editor-list ul { margin-bottom:20px;}
    .editor-list ul li { margin:5px 10px 10px 15px; font-size:14px;}
</style>

<div class="editor-wrap">
<h2>UEditor各种实例演示</h2>
<h3>基础示例</h3>

<div class="editor-list">
<ul>
    <li>
        <a href="SimpleDemo.aspx" target="_self">简单示例</a><br/>
        使用基础的按钮实现简单的功能
    </li>
</ul>
<h3>应用展示</h3>
<ul>
    <li>
        <a href="CustomPluginDemo.aspx" target="_self">自定义插件</a><br/>
        在编辑器的基础上开发自己的插件
    </li>
    <li>
        <a href="SubmitFormDemo.aspx" target="_self">表单应用</a><br/>
        编辑器的内容通过表单提交到后台
    </li>
    <li>
        <a href="ResetDemo.aspx" target="_self">重置编辑器</a><br/>
        将编辑器的内部变量清空，重置。
    </li>
    <li>
        <a href="TextareaDemo.aspx" target="_self">文本域渲染编辑器</a><br/>
        将编辑器渲染到文本域，并且将文本域的内容放到编辑器的初始化内容里
    </li>
</ul>
<h3>高级案例</h3>
<ul>
    <li>
        <a href="CompleteDemo.aspx" target="_self">完整示例</a><br/>
        编辑器的完整功能
    </li>
    <li>
        <a href="MultiDemo.aspx" target="_self">多编辑器实例</a><br/>
        一个页面实例化多个编辑器，互不影响
    </li>
    <li>
        <a href="CustomToolbarDemo.aspx" target="_self">自定义Toolbar</a><br/>
        用自己的皮肤，设计自己的编辑器
    </li>
    <li>
        <a href="HighlightDemo.aspx" target="_self">代码高亮</a><br/>
        代码高亮展示及其编辑
    </li>
    <li>
        <a href="RenderInTable.aspx" target="_self">在表格中渲染编辑器</a><br/>
        表格中渲染编辑器
    </li>
    <li>
        <a href="JQueryCompleteDemo.aspx" target="_self">jquery</a><br/>
        jquery中使用编辑器
    </li>

    <li>
        <a href="IosFiveDemo.aspx" target="_self">ipad.ios.5.0+</a><br/>
        编辑器在ios5+中的使用
    </li>
    <li style="display:none;">
        <a href="Uparsedemo.aspx" target="_self">展示页面uparse.js解析</a><br/>
        通过调用uparse.js在展示页面中自动解析编辑内容
    </li>
    <li style="display:none;">
        <a href="JQueryValidation.aspx" target="_self">jqueryValidation</a><br/>
        编辑器在jqueryValidation中验证
    </li>
</ul>

</div>

</div>
<script type="text/javascript">
    var href = location.href;
    if(href.indexOf("http") != 0 ){
        alert("您当前尚未将UEditor部署到服务器环境，部分浏览器下所有弹出框功能的使用会受到限制！");
    }
</script>
