<div style="background:#2980b9; height:280px;">
<div style="padding:50px 20px 10px 20px;">

    <div><a href="#{lnkBottomBox}" class="frmBox" style="color:#fff;">在本局部页面中，还可以弹窗</a></div>
    <div style="margin-top:10px;">
        <textarea id="TextArea1"  style="width:280px; height:50px;"></textarea>
    </div>


</div>
</div>


<script>
// 这是供弹窗调用的方法
function setTextValue( boxText ) {

    $('#TextArea1').val( boxText );

}
</script>
