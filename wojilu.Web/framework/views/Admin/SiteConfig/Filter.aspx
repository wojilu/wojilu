<div class="formPanel" style="margin-left:10px;">

	
		<div style="padding:10px 10px;" class="red">_{wordFilterTip}</div>	
	
		<form action="#{ActionLink}" method="post" class="ajaxPostForm">
		<table style="width: 100%">
			
			<tr>
				<td style="width:50px;vertical-align:top;text-align:right">_{sourceWord}</td>
				<td style="width:350px;">
				<textarea name="BadWords" style="width: 350px; height: 250px">#{site.BadWordsStr}</textarea></td>
				<td style="width:65px;vertical-align:top;text-align:right">_{replacedWord}</td>
				<td style="vertical-align:top"><input name="BadWordsReplacement" type="text" value="#{site.BadWordsReplacement}"></td>
			</tr>
			
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{editSetting}" class="btn">&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
			</tr>
		</table>
		</form>
	
	
	</div>	

