
<script>
    _run(function () {
        wojilu.editor.bind("myEditor").show();
        
        $('#btn').click(function () {
            alert('btn clicked');
            //手动提交需要手动同步编辑器数据
            UE.getEditor('myEditor').sync();
            $('#form')[0].submit();
        });
        
    });
</script>
</head>
<div style="margin:30px;">
<form id="form" method="post" target="_blank" action="/Demo/Editor/SimpleSave.aspx">
    <script type="text/plain" id="myEditor" name="post.Content">
        <p>欢迎使用UEditor！</p>
    </script>
    <div style=" margin-top:10px;">
    <input type="button" id="btn" class="btn btn-primary" value="提交数据">
    </div>
</form>
</body>
</html>