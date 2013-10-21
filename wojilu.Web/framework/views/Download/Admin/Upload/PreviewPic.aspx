<div>
    <table style="margin: 20px 10px;">
        <tr>
            <td style="width:150px;">
                <!-- BEGIN previewPic -->
                <img src="#{picPath}" />
                <!-- END previewPic -->
            </td>
            <td>
                <form method="post" action="#{ActionLink}" enctype="multipart/form-data">
                <div style="margin: 10px 10px 10px 10px;">
                    <input id="File1" name="myfile" type="file" /></div>
                <div style="">
                    <input id="Submit1" type="submit" value="上传预览图片" class="btn" /></div>
                </form>
            </td>
        </tr>
    </table>
    <div>
    </div>
</div>
