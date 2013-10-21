
<style>
#PhotoAlbumId {width:150px;}
</style>
<div style="width:500px; height:400px;">

	<form method="post" action="#{ActionLink}" class="ajaxPostForm2" style="margin:10px 10px 30px 20px;">
		<table style="width: 98%;">
			<tr>
				<td>_{photo}</td>
				<td><img src="#{data.ImgUrl}"/></td>
			</tr>
			<tr>
				<td>_{name}</td>
				<td><input name="Title" type="text" style="width: 180px" value="#{data.Title}"></td>
			</tr>
			<tr>
				<td>:{myalbum}</td>
				<td>#{PhotoAlbumId}</td>
			</tr>
			<tr><td>:{sysCategory}</td><td>
		#{SystemCategoryId}
		</td></tr>

			<tr>
				<td>_{description}</td>
				<td><textarea name="Description" style="width: 350px; height: 60px">#{data.Description}</textarea>&nbsp;</td>
			</tr>
			<tr>
				<td>_{tag}</td>
				<td><input name="TagList" type="text" style="width: 268px" value="#{data.TagList}"> </td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>
				<input type="hidden" name="returnUrl" value="#{returnUrl}" />
				<input name="btnUpdatePhoto" type="submit" value=":{editPhoto}" class="btn" />
                <input type="button" value="_{cancel}" class="btnCancel" />
                </td>
			</tr>
		</table>
	</form>
</div>
