<div style=" padding:50px;">
    <div>--------------------当前窗口：BoxC-------------------</div>
    <div style="background:#f2f2f2;padding:5px 10px; margin-top:20px;">
    <a href="#{link}" class="frmBox" xwidth="420" xheight="250" title="BoxD">点击此处，弹出 BoxD 窗口(D窗口会刷新本窗口)</a>
    </div>
    
    <div style="background:#f2f2f2;padding:5px 10px; margin:20px 0px;">请输入值，返回到父窗口BoxB <input id="txtValue" type="text" /><input id="btn" class="btn btns" type="button" value="提交" /></div>
    
</div>

<script>
_run( function() {

    $( '#btn' ).click( function() {
        alert( '你输入的是：'+$('#txtValue').val() );
        
        // 1)得到父窗口
        var p = wojilu.tool.getBoxParent();
        
        // 2)调用父窗口的 setTextValue 方法
        p.setTextValue( $('#txtValue').val() );
        
        // 3)关闭弹窗(弹窗不可以自己关闭自己，必须由父页面来关闭)
        p.wojilu.ui.box.hideBox();

    });

});
</script>