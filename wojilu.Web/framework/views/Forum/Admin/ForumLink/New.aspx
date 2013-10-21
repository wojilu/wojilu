
	
		<div class="">

			<form method="post" action="#{ActionLink}">
			<table style="width: 98%;" >
				<tr>
					<td style="width:100px;">_{name}</td>
					<td>
					<input name="Name" type="text" value="#{link.Name}" style="width: 450px" /></td>
				</tr>
				<tr>
					<td>_{url}</td>
					<td>
						<input name="Url" type="text" style="width: 450px" value="#{link.Url}">&nbsp;</td>
				</tr>
				<tr>
					<td>:{forumLogoUrl}</td>
					<td>
						<input name="Logo" type="text" style="width: 450px" value="#{link.Logo}">&nbsp;</td>
				</tr>
				<tr>
					<td>_{description}</td>
					<td><textarea name="Description" style="height: 60px; width: 450px">#{link.Description}</textarea></td>
				</tr>
				<tr>
					<td></td>
					<td>
					<input type="submit" value="_{submit}"  class="btn" />
					<input type="button" value="_{return}" class="btnReturn"  />
					</td>
				</tr>
			</table>
			</form>

		</div>
		


