	<div class="" style="padding:20px;">
		<form method="post" action="#{ActionLink}">
		<table style="width: 400px;">
			<tr>
				<td>_{name}</td>
				<td><input name="photoSysCategory.Name" type="text" value="#{category.Name}" /></td>
			</tr>
			<tr>
				<td>_{orderId}</td>
				<td>
					<input name="photoSysCategory.OrderId" type="text" style="width: 32px;" value="#{category.OrderId}">
				</td>
			</tr>
			<tr>
				<td></td>
				<td><input type="submit" value="_{editCategory}" class="btn" />&nbsp;
				<input name="Button1" type="button" class="btnCancel" value="_{cancel}" /></td>
			</tr>
		</table>
		</form>
		</div>
	

