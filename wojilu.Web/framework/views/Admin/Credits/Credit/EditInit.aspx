<div style="padding:20px;">
		
			<form method="post" action="#{ActionLink}">			
			
			
				<table style="width: 100%">
					<tr>
						<td style="width:60px;">_{user}</td>
						<td><strong>_{regHave}</strong></td>
					</tr>
					<tr>
						<td>_{canHave}</td>
						<td>#{c.Name} <input name="InitValue" type="text" value="#{c.InitValue}" style="width:50px;"><span class="note">#{c.Unit}</span></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td><input name="Submit1" type="submit" value="_{submit}" class="btn">&nbsp;
						</td>
					</tr>
				</table>			
			
			
			</form>

</div>