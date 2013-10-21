<div style="margin:30px;">

<div style=" font-size:14px; margin-bottom:20px;">编辑器实例化到表格内部时，<span style="color:Red;font-weight:bold; ">请在对应的单元格上明确标注宽度值（百分数或者直接数均可）</span>，否则有可能在工具栏浮动等功能状态下出现移位</div>
<div id="div" style="border: 1px solid #fff">
    <table border="1">

        <tr><th style="width:60px;">标题</th><th style="width:600px;">内容</th></tr>

        <tr>
            <td style=" vertical-align:top;">中国</td><td style="width:600px;"><textarea id="editor"></textarea></td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    _run(function () {
        wojilu.editor.bind("editor").show();
    });
</script>

</div>