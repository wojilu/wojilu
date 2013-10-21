
<style>
.uploadTable { margin:10px 0px 15px 0px;width:600px; text-align:right; border-collapse:collapse; background:#eee;}
.uploadTable td {padding:3px 5px;}

td.uploadDescription {	width:500px; text-align:left;}
.uploadInput {	text-align:left;}
.uploadInput input {width:400px;}
.uploadDescription input{width:400px;}
</style>


		<div class="" style="margin:10px 20px;">


			<h4 style="margin:5px 5px 5px 5px;">:{uploadAlbum}:<span class="red font16 strong">《#{photoAlbum}》</span></h4>
			
			<form action="#{ActionLink}" method="post" enctype="multipart/form-data">
			<input type="hidden" name="PhotoAlbumId" value="#{PhotoAlbumId}" />
			<input name="SystemCategoryId" type="hidden" value="#{SystemCategoryId}" />
			
			<!-- BEGIN list -->
			<table class="uploadTable">
				<tr>
					<td>_{photo}</td>
					<td class="uploadInput"><input name="File#{photoIndex}" type="file"></td>
				</tr>
				<tr>
					<td>_{note}</td>
					<td class="uploadDescription"><input name="Text#{photoIndex}" type="text" size="20"></td>
				</tr>
			</table>
			<!-- END list -->
			
			
			<div style="margin:20px 10px 10px 100px;"><input name="Submit1" type="submit" value=":{uploadPhoto}" class="btn" /></div>
			
			</form>

	</div>
