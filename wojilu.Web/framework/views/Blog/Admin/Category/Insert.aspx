
<div style="margin:20px;">
    <div style="font-size:20px;font-weight:bold;font-family:Microsoft YaHei;text-align:;color:red;padding-left:100px;">_{opok}</div>
    <script>
        var categories = '#{CategoryId}';
        setTimeout( function() {
            var p = wojilu.tool.getBoxParent();
            p.refreshCategories( categories, #{cid} );
            p.wojilu.ui.box.hideBox();
        }, 500 );
    </script>

</div>
