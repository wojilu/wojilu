<div style="margin:10px;">
	<form action="#{ActionLink}" method="post">
	
	<table style="width:750px" class="tabHeader" cellpadding="0" cellspacing="0">
		<tr>
			<td class="currentTab" style="width:25%;">:{addSection1}</td>
			<td class="otherTab" style="width:25%;">:{addSection2}</td>
			<td class="otherTab" style="width:25%;">:{addSection3}</td>
			<td class="otherTab" style="width:25%;">&nbsp;</td>
		</tr>
	</table>
	
	<div class="tabMain" style="width:750px;height:200px;">
		<div style="padding:20px">
		<table style="width:99%;">
			<tr>
				<td style="width:100px;">:{plsSelectDataSource}</td>
				<td>#{serviceId}</td>
			</tr>
			<tr>
				<td style="width:100px;">&nbsp;</td>
				<td style="padding:10px 0px;">
					<input name="Submit1" type="submit" value=":{nextStep}" class="btn" />
					<input type="button" value="_{cancel}" class="btnCancel" />
				</td>
			</tr>
		</table></div>
	</div>
	
	</form>
</div>

