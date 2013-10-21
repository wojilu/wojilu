

		<div style="margin:20px 30px;">
			<form method="post" action="#{ActionLink}" class="ajaxPostForm">		
			<div style="font-weight:bold; margin:10px;">添加专辑：</div>
			<table style="width:95%; margin-bottom:10px; text-align:left;">
				<tr>
					<td>_{name}</td>
					<td><input name="photoAlbum.Name" type="text" /></td>
				</tr>

				<tr>
					<td>_{description}</td>
					<td><textarea name="photoAlbum.Description" style="height: 60px; width: 400px"></textarea></td>
				</tr>
				<tr>
					<td></td>
					<td>
					<input type="submit" value="_{submit}" class="btn" />
					<input name="Button1" type="button" value="_{return}" class="btnReturn" />
					</td>
				</tr>
			</table>
			
			</form>
		</div>
