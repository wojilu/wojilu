<div style="margin:30px;">

<h1>UEditor多实例</h1>

<!--style给定宽度可以影响编辑器的最终宽度-->
<script type="text/plain" id="myEditor1" style="margin-bottom:100px;">
</script>
<script type="text/plain" id="myEditor2" style="margin-bottom:100px;">
    <p>这里我可以写一些输入提示</p>
</script>
<script type="text/plain" id="myEditor3" style="margin-bottom:100px;">
</script>

<script type="text/javascript">
    _run(function () {

        wojilu.editor.bind('myEditor1').config({
            theme: "default", //皮肤
            lang: 'zh-cn' //语言
        }).show();

        wojilu.editor.bind('myEditor2').config({
            autoClearinitialContent: true, //focus时自动清空初始化时的内容
            wordCount: false, //关闭字数统计
            elementPathEnabled: false//关闭elementPath
        }).show();

        wojilu.editor.bind('myEditor3').config({
            toolbars:[['FullScreen', 'Source', 'Undo', 'Redo','Bold']],//这里可以选择自己需要的工具按钮名称,此处仅选择如下五个
            lang: "en"
            //更多其他参数，请参考editor_config.js中的配置项
        }).show();


    });
</script>


</div>