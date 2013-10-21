		<form action="#{ActionLink}" method="post" class="ajaxPostForm">
		<table style="width: 580px;">
			<tr>
				<td>:{linkName}</td>
				<td><input name="Name" style="width: 153px" type="text" value="#{roll.Name}"></td>
			</tr>
			<tr>
				<td>:{linkUrl}</td>
				<td><input name="Link" style="width: 450px" type="text" value="#{roll.Link}"></td>
			</tr>
			<tr>
				<td>_{orderId}</td>
				<td><input name="OrderId" style="width: 47px" type="text" value="#{roll.OrderId}"></td>
			</tr>
			<tr>
				<td>_{description}</td>
				<td><textarea name="Description" style="width: 451px; height: 74px">#{roll.Description}</textarea></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value=":{editLink}" class="btn">&nbsp;<input name="Button1" class="btnReturn" type="button" value="_{return}"></td>
			</tr>
		</table>
		</form>
