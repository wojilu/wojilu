	<div style="margin:15px 0px;">
    <form method="post" action="#{ActionLink}" style="margin:20px;">
    <table style="width:100%;">
        <tr>
            <td style="text-align:left; vertical-align:top;">
                <div><textarea id="cbody" name="content" style="width:100%;height:80px;"> #{content}</textarea><span class="valid" mode="border"></span></div>
                <div>
                    <input type="submit" value="_{comment}" class="btn btns" id="btnComment" />
                    <input name="rootId" type="hidden" value="#{c.RootId}" />
                    <input name="parentId" type="hidden" value="#{c.ParentId}" />                
                </div>
            </td>
        </tr>
    </table>
    </form>
    </div>

<script>

_run( function() {

    wojilu.ui.valid();
    
    $('#cbody').focus();
    

});

</script>