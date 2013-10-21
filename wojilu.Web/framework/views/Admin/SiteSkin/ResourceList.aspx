<div>

    <div style="font-size:14px; font-weight:bold; margin:0px 0px 15px 10px;">#{resourceTypeName}</div>

    <div style="margin:10px;">
    <form method="post" enctype="multipart/form-data" action="#{ActionLink}">
    <input id="File1" name="myfile" type="file" /> 
    <input id="Submit1" type="submit" value="_{uploadPic}" class="btn btns" />
    
    </form>
    
    </div>
	
	<!-- BEGIN list -->
	<table style="width:100px;text-align:center; float:left;margin:10px;">
		<tr>
			<td style="background:#fff;vertical-align:top; padding:10px;">
			<a href="#{r.ImgUrl}" target="_blank">
			<img src="#{r.ImgThumbUrl}" class="photoThumb" style=" max-width:80px; max-height:80px;" /></a>
			</td>
		</tr>
		<tr>
			<td align='center'><a href="#{r.DeleteLink}" class="deleteCmd">_{delete}</a></td>
		</tr>
	</table>
	<!-- END list -->
	<div style="clear:both"></div>
	<div>#{page}</div>


</div>