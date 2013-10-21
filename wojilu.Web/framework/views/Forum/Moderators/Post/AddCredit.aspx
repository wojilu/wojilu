<div style="width:500px;height:150px; margin:10px;">
<form method="post" action="#{ActionLink}">

	<table >
		<tr>
			<td>:{currencyList}</td>
			<td>#{CurrencyId}</td>
		</tr>
		<tr>
			<td>:{currencyValue}</td>
			<td>#{CurrencyValue}</td>
		</tr>
		<tr>
			<td>:{currencyReason}</td>
			<td><textarea name="Reason" style="width: 320px; height: 50px"></textarea></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td>
			    <input name="Submit1" type="submit" value="_{submit}" class="btn" />
			    <input type="button" class="btnCancel" value="_{cancel}" />
			</td>
		</tr>
	</table>

</form>
</div>