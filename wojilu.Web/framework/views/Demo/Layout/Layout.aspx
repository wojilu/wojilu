<link type="text/css" rel="stylesheet" href="~js/lib/syntaxhighlighter/styles/sh.css"/>
<script type="text/javascript">
    _run(function () {
        require(['lib/syntaxhighlighter/scripts/sh'], function () {
            SyntaxHighlighter.config.clipboardSwf = "~js/lib/syntaxhighlighter/scripts/clipboard.swf";
            SyntaxHighlighter.all();
        });
    });
</script>

<div style="background:#e1f0fa; margin-top:10px;">
    <div style="padding:10px;font-size:18px; font-weight:bold; text-align:center; color:#657788; ">“我记录快速开发框架” 示例演示( wojilu framework demo)</div>    
    <div style="padding:0px 0px 0px 0px; border-top:0px #aaa dotted;">
    
<table style="width:100%;" cellpadding="0" cellspacing="0">
<tr>

<td style="width:150px; vertical-align:top; padding:10px 5px 30px 20px; background:#f2f2f2;">
<style>
.demoSidebar { border:0px blue solid;margin:0px;margin-right:20px;}
.demoSidebar li { display:block; padding:3px 10px;}
.demoSidebar li:hover { background:#fff;}

</style>

<ul class="demoSidebar unstyled">


    <li><a href="/Demo/Valid/Index.aspx" class="frmLink2" loadTo="validMain" nolayout=3>验证：语法入门</a></li>
    <li><a href="/Demo/Valid/Drop.aspx" class="frmLink2" loadTo="validMain" nolayout=3>验证：下拉框</a></li>
    <li><a href="/Demo/Valid/Checkbox.aspx" class="frmLink2" loadTo="validMain" nolayout=3>验证：多选框</a></li>
    <li><a href="/Demo/Valid/RadioList.aspx" class="frmLink2" loadTo="validMain" nolayout=3>验证：单选框</a></li>
    <li><a href="/Demo/Valid/Pwd.aspx" class="frmLink2" loadTo="validMain" nolayout=3>验证：密码框</a></li>
    <li><a href="/Demo/Valid/Rule.aspx" class="frmLink2" loadTo="validMain" nolayout=3>验证：内置规则</a></li>
    <li><a href="/Demo/Valid/CustomRule.aspx" class="frmLink2" loadTo="validMain" nolayout=3>验证：自定义规则</a></li>
    <li><a href="/Demo/Valid/Ajax.aspx">验证：ajax检验</a></li>
    <li><a href="/Demo/Valid/Tip.aspx">验证：输入框提示</a></li>
    <li><a href="/Demo/Valid/Editor.aspx">验证：编辑器验证</a></li> 



    <li style="margin-top:20px;"><a href="/Demo/Editor/SimpleDemo.aspx">新编辑器</a></li>
    <li><a href="/Demo/Editor/AjaxForm.aspx" >编辑器Ajax提交</a></li>
    <li><a href="/Demo/Editor/AutoHeight.aspx" >编辑器自动增高</a></li>
    <li><a href="/Demo/Editor/CustomPluginDemo.aspx" >自定义插件</a></li>
    <li><a href="/Demo/Editor/SubmitFormDemo.aspx" >表单处理</a></li>
    <li><a href="/Demo/Editor/CompleteDemo.aspx" >完整示例</a></li>

    <li><a href="/Demo/Editor/ResetDemo.aspx" >重置编辑器</a></li>
    <li><a href="/Demo/Editor/TextareaDemo.aspx" >文本渲染</a></li>
    <li><a href="/Demo/Editor/MultiDemo.aspx" >多编辑器</a></li>
    <li><a href="/Demo/Editor/CustomToolbarDemo.aspx">自定义工具栏</a></li>
    <li><a href="/Demo/Editor/HighlightDemo.aspx">代码高亮</a></li>
    <li><a href="/Demo/Editor/RenderInTable.aspx">在表格中</a></li>
    <li><a href="/Demo/Editor/JQueryCompleteDemo.aspx">jQuery</a></li>
    <li><a href="/Demo/Editor/IosFiveDemo.aspx">ios平台</a></li>


    <li style="margin-top:20px;"><a href="/Demo/Menu/Simple.aspx">菜单：简易菜单</a></li>
    <li><a href="/Demo/Menu/Multi.aspx">菜单：多级菜单</a></li>

    <li style="margin-top:20px;"><a href="/Demo/Valid/Box.aspx">弹窗效果</a></li>
    <li><a href="/Demo/Valid/BoxReturn.aspx">弹窗返回值</a></li>
    <li><a href="/Demo/Valid/BoxRefresh.aspx">弹窗刷新</a></li>
    <li><a href="/Demo/Box/NBox.aspx">无限级弹窗</a></li>
    
    <li style="margin-top:20px;"><a href="/Demo/FrmLink/Index.aspx">局部刷新</a></li>
    <li><a href="/Demo/Valid/FrmLinkRedirect.aspx">局部刷新跳转</a></li>
    <li><a href="/Demo/FrmTest/Main/Data/Post/Index.aspx">局部无限级嵌套</a></li>
 
    <li style="margin-top:20px;"><a href="/Demo/Valid/AjaxForm.aspx">ajax表单</a></li>
    <li><a href="/Demo/Valid/AjaxFormRedirect.aspx">ajax表单跳转</a></li>
    <li><a href="/Demo/Valid/AjaxFormUpdate.aspx">ajax提交更新</a></li>
    <li><a href="/Demo/Valid/AjaxDelete.aspx">ajax删除</a></li>
    
    
    <li style="margin-top:20px;"><a href="/Demo/Tree/Simple.aspx">树：简单绑定</a></li>
    <li><a href="/Demo/Tree/Index.aspx">树：综合实例</a></li>

    <li style="margin-top:20px;"><a href="/Demo/Valid/Calendar.aspx">日历控件</a></li>
    <li><a href="/Demo/Upload/FrameUpload.aspx">上传:简单异步上传</a></li>
    <li><a href="/Demo/Upload/FlashUpload.aspx">上传:flash进度条</a></li>
    <li><a href="/Demo/Valid/Slider.aspx">简易幻灯</a></li>
    <li><a href="/Demo/Valid/Tab.aspx">tab选项卡</a></li>
    
    <li><a href="/Demo/JsonTest/JsonTest.aspx">json处理</a></li>
    
    <li style="margin-top:20px;"><a href="/Demo/Valid/DataGrid.aspx">数据表格</a></li>
    <li><a href="/Demo/Valid/Pager.aspx">分页显示</a></li>
    <li><a href="/Demo/Valid/PagerList.aspx">内存分页</a></li>
    <li><a href="/Demo/Valid/PagerCustom.aspx">自定义分页</a></li>

    <li style="margin-top:20px;"><a href="/Demo/Valid/Cookie.aspx">自定义cookie</a></li>


</ul>

</td>
<td style="vertical-align:top; background:#fff;" id="validMain" class="edui">#{layout_content}</td>

</tr>

</table>


    
    </div>
</div>


<script>

_run( function() {
    wojilu.ui.frmLink();
    wojilu.ui.valid();
    wojilu.ui.tip();
    wojilu.ui.ajaxPostForm();

});

</script>



