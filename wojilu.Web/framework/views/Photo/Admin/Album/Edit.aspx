
<style>
/*图片列表*/

.photoAlbumTitle { border-bottom:1px #ccc solid; padding:5px 0px 5px 0px; margin-bottom:10px;}

.photoAlbumTitle .left10 {font-size:12px;font-weight:normal;}

#photoThumbList li {float:left; display:inline; width:140px; height:155px; margin:5px 20px 20px 5px; padding:5px;text-align:center; border-radius:4px; border:1px #ccc solid; box-shadow:0px 0px 10px #ccc; }
#photoThumbList li div {margin-bottom:3px;}
#photoThumbList li div a {font-size:12px;}

#photoThumbList li div.editTitle { height:20px;
    width:140px;
    white-space:nowrap;
    text-overflow:clip;
    -moz-text-overflow:clip;
    overflow: hidden;
}

.currentCover {color:Red; font-weight:bold;}
.max100 {max-width:100px; max-height:100px;width:expression(width>100?"100px":width+"px");height:expression(height>100?"100px":height+"px");}

</style>


<div>


    <div style=" margin:0px 0px 10px 10px; padding:10px; width:95%; border-radius:4px;" class="cbg">
	<form method="post" action="#{ActionLink}" class="ajaxPostForm">
	<table style="width: 700px;text-align:left; ">
		<tr>
			<td>_{name}</td>
			<td><input name="photoAlbum.Name" type="text" value="#{category.Title}" /></td>
		</tr>
		<tr>
			<td>_{description}</td>
			<td><textarea name="photoAlbum.Description" style="height: 60px; width: 400px">#{category.Description}</textarea></td>
		</tr>
		<tr>
			<td></td>
			<td><input type="submit" value="_{edit}"  class="btn" /> </td>
		</tr>
	</table>
	</form>
	</div>
	
    <div style="margin-left:20px;margin-top:20px;">

        <ul id="photoThumbList">
            <!-- BEGIN list -->
            <li>
                <div style="margin-top:10px;"><img src="#{data.ImgThumbUrl}" class="max100"  /></div>
                <div class="editTitle"><a href="#{data.EditLink}" class="frmBox"><img src="~img/edit.gif" /> #{data.Title}</a></div>
                <div>#{data.SetCoverLink}</div>
            </li>    
            <!-- END list -->
        </ul>
        <div style="clear:both;"></div>
        <div>#{page}</div>
	
    </div>

</div>

