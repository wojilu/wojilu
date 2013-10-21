<div style="margin:20px;">
<form action="#{ActionLink}" method="post" enctype="multipart/form-data">
<table style="width:100%;">
    <tr>
        <td>:{uploadAttachment}</td>
        <td><input id="UploadFile" name="UploadFile" type="file" /></td>
    </tr>
    <tr>
        <td style="vertical-align:top;">:{attachmentNameOrDescription}</td>
        <td><textarea style="width:380px; height:40px;" name="FileDescription"></textarea></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td><input id="Submit1" type="submit" value=":{uploadAttachment}" class="btn" />
            <input id="Button1" type="button" value="_{cancel}" class="btnCancel" />
        </td>
    </tr>
    
    <tr style="display:none;" id="morePanel">
        <td colspan="2">
        <table>
        <tr>
            <td>:{readPermission}</td>
            <td><input name="FileReadPermission" type="text" /></td>
        </tr>
        <tr>
            <td>:{price}</td>
            <td><input name="FilePrice" type="text" /></td>
        </tr>
         </table>
        </td>
    </tr>
    
    
</table>
<div><ul class="note">:{uploadNote}</ul></div>
</form>
</div>