<div style="margin:10px 10px 20px 10px;">			
	<form method="post" action="#{ActionLink}">			
	
		<table style="width:99%">
		<tr><td>
				<div>:{styleCodeExample}:

				<span style="background:#fff;color:green;padding:5px 0px;font-size:11px;">
					#row1_column1 {width:60%;} <span class="note">// :{styleTip1}</span></span></div>
		</td></tr>
			<tr>
				<td style="padding-top:10px">
				 <span class="note">:{styleEditTip}</span><br/>
				<textarea name="Style" style="width: 99%; height: 250px;padding:5px;font-family:Verdana;">#{styleContent}</textarea></td>
			</tr>
		</table>		
	
		<div>
		    <input name="Submit1" type="submit" value=":{applyStyle}" class="btn" />
		    <input type="button" value="_{cancel}" class="btnCancel" />
		</div>
	</form>

</div>		
