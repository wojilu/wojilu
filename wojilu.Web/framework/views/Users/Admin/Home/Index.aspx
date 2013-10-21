<div style="margin:20px;">#{publisher}</div>
<script>_run( function() {$('#srcType').val( 'mbHome' );});</script>

<div id="blog-item-wrap">
    <div id="newMicroblogPanel" style="margin:15px 15px 20px 15px;"></div>
    <div>
    <div style="margin:20px;">#{blogList}</div>
    </div>
</div>

<div style="margin-left:25px;margin-bottom:30px;">#{page}</div>

<script>
    _run(function () {
        $('#blog-item-wrap a').click(function () {
            $(this).attr('href', wojilu.str.trimEnd($(this).attr('href'), '?nolayout=1'));
            $(this).attr('target', '_blank');
        });
    });
</script>




