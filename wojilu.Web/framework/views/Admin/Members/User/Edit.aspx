

	<div class="">

<div style="padding:5px 20px; padding-left:40px; background:#f2f2f2; margin:10px 20px;">

<form method="post" action="#{ActionLink}" class="ajaxPostForm">
	<table style="width: 95%;  ">
		<tr>
			<td  valign="top" class="profile_panel">

			<table border="0" style="">
				<tr>
					<td class="tdLeft">_{nickName}</td>
					<td><strong>#{m.NickName}</strong>
                    <a href="#{lnkEditName}" class="frmBox left10" title="修改用户名"><img src="~img/edit.gif" /> 修改</a>
                    </td>
				</tr>
				<tr>
					<td class="tdLeft">个性网址</td>
					<td>#{m.Url}
                    <a href="#{lnkEditUrl}" class="frmBox left10" title="修改个性网址"><img src="~img/edit.gif" /> 修改</a>
                    </td>
				</tr>
				<tr>
					<td class="tdLeft">Email</td>
					<td>#{m.Email}
                    <a href="#{lnkEditEmail}" class="frmBox left10" title="修改email"><img src="~img/edit.gif" /> 修改</a>
                    </td>
				</tr>
				<tr>
					<td class="tdLeft">_{userId}</td>
					<td>#{m.Id}</td>
				</tr>
				<tr>
					<td class="tdLeft">_{realName}</td>
					<td><input name="Name" type="text" value="#{m.Name}"> </td>
				</tr>
				<tr>
					<td class="tdLeft">_{birthday}</td>
					<td>#{Year}&nbsp;#{Month}&nbsp;#{Day}
				</tr>
				<tr>
					<td class="tdLeft">_{gender}</td>
					<td>#{Gender}</td>
				</tr>
				<tr>
					<td class="tdLeft">_{zodiac}</td>
					<td>#{Zodiac}</td>
				</tr>
				<tr>
					<td class="tdLeft">_{blood}</td>
					<td>#{Blood}</td>
				</tr>
				<tr>
					<td class="tdLeft">_{relationship}</td>
					<td>#{Relationship}</td>
				</tr>
				<tr>
					<td class="tdLeft">_{degree}</td>
					<td>#{Degree}</td>
				</tr>
				

				<tr>
					<td class="tdLeft">&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="tdLeft">_{registerReason}</td>
					<td>#{Purpose}</td>
				</tr>
				<tr>
					<td class="tdLeft">_{hometown}</td>
					<td>#{ProvinceId1}
					<input name="City1" type="text" value="#{m.City1}" style="width: 82px"></td>
				</tr>
				<tr>
					<td class="tdLeft">_{region}</td>
					<td>#{ProvinceId2}
					<input name="City2" type="text" value="#{m.City2}" style="width: 82px"></td>
				</tr>
				
			</table>

			</td>
		</tr>
		<!-- BEGIN sexyInfo -->
		<!-- END sexyInfo -->
		<tr>
			<td colspan="2" class="profileHeader">&nbsp;</td>
		</tr>
		<tr>
			<td valign="top" class="profile_panel" colspan="2">
			_{selfIntroductionTip}<br>
			<textarea name="Description" style="width: 90%; height: 96px">#{m.Description}</textarea>
			<br /><br/>
			_{signatureTip}<br>
			<textarea name="Signature" style="width: 90%; height: 106px">#{m.Signature}</textarea>
			</td>
		</tr>
	</table>
	<div style="margin-left:120px;">
	<input name="Submit1" type="submit" class="btnSave btn" value="_{saveUpdate}">&nbsp;
	</div>
</form>

</div>


	
	</div>


