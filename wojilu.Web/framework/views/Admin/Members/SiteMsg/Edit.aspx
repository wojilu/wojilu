
	
<div class="" style="padding:20px;">
<form method="post" action="#{ActionLink}" class="ajaxPostForm">

        <table style="width:100%;">

    <tr>
        <td>_{title}</td>
        <td><input name="Title" type="text" style="width:500px;" value="#{Title}" /><span class="valid border"></span></td>
    </tr>
    <tr>
        <td>_{receiver}</td>
        <td>#{siteRole}</td>
    </tr>
    <tr>
        <td>_{content}</td>
        <td>
<script type="text/plain" id="Body" name="Body">#{Body}</script>
<script>
    _run(function () {
        wojilu.editor.bind('Body').height(350).line(1).show();
    });
</script>
<span class="valid border" to="Body"></span>
        </td>
    </tr>


    <tr>
        <td>&nbsp;</td>
        <td><input type="submit" name="btnSubmit" value="_{editSiteMsg}" class="btn" id="btnSubmit" />
            <input type="button" value="_{return}" class="btnReturn" />
    </td>
    </tr>

</table>

</form>

</div>
