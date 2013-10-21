
<div class="formPanel" style="margin-left:10px;">
	
	
		<form action="#{ActionLink}" method="post" class="ajaxPostForm">
		<table style="width: 100%">
			<tr>
				<td class="ltd">访问限制</td>
				<td><label><input name="NeedLogin" type="checkbox" #{needLoginChecked} /> 只有登录用户才可以访问网站内容</label></td>
			</tr>
			<tr>
				<td class="ltd">注册限制</td>
				<td>#{registerType}</td>
			</tr>
			<tr>
				<td class="ltd">注册审核</td>
				<td><label><input name="UserNeedApprove" type="checkbox" #{userNeedApproveChecked} /> 用户注册之后必须经过人工审核才能登录</label></td>
			</tr>
			<tr>
				<td class="ltd">页顶用户栏</td>
				<td>#{topNavDisplay}</td>
			</tr>	
			
			<tr>
				<td class="ltd">登录限制</td>
				<td>#{loginType}</td>
			</tr>
			<tr>
				<td style="width:120px">_{userNameLength}</td>
				<td>
				_{fromChar} <input name="UserNameLengthMin" type="text" style="width: 50px" value="#{site.UserNameLengthMin}"> 
				_{toChar} <input name="UserNameLengthMax" style="width: 50px" type="text" value="#{site.UserNameLengthMax}"> 
				</td>
			</tr>
			<tr>
				<td class="ltd">用户简介长度</td>
				<td>
				_{fromChar} <input name="UserDescriptionMin" type="text" style="width: 50px" value="#{site.UserDescriptionMin}"> 
				_{toChar} <input name="UserDescriptionMax" style="width: 50px" type="text" value="#{site.UserDescriptionMax}"> 
                </td>
			</tr>
			<tr>
				<td class="ltd">用户签名长度</td>
				<td>
				_{fromChar} <input name="UserSignatureMin" type="text" style="width: 50px" value="#{site.UserSignatureMin}"> 
				_{toChar} <input name="UserSignatureMax" style="width: 50px" type="text" value="#{site.UserSignatureMax}"> 
                </td>
			</tr>	
			
			<tr><td colspan="2">&nbsp;</td></tr>
			<tr>
				<td class="ltd">Email激活提醒</td>
				<td><label><input name="AlertActivation" type="checkbox" #{needAlertActivation} /> 提醒用户email激活<span class="note">(激活的奖励分在“货币和积分”中设置)</span></label></td>
			</tr>
			<tr>
				<td class="ltd">上传头像提醒</td>
				<td><label><input name="AlertUserPic" type="checkbox" #{needAlertUserPic} /> 提醒用户上传头像<span class="note">(上传头像的奖励分在“货币和积分”中设置)</span></label></td>
			</tr>

			<tr>
				<td class="ltd">激活邮件限制</td>
				<td><label>用户在 <input name="confirmEmailInterval" type="text" value="#{site.UserSendConfirmEmailInterval}" style="width:30px;" /> 分钟之内只能发一次激活邮件</td>
			</tr>
			<tr>
				<td class="ltd">邮件激活模板</td>
				<td>请 <a href="#{confirmEmailEditLink}" target="_blank">点击此处</a> 修改邮件激活模板</td>
			</tr>
			<tr><td colspan="2">&nbsp;</td></tr>

			<tr>
				<td colspan="2" style="font-size:16px;font-weight:bold; padding-bottom:10px ">_{newUserRegTip}
				</td>
			</tr>
			<tr>
				<td>_{msgTitle}</td>
				<td>
				<input name="SystemMsgTitle" type="text" style="width: 600px" value="#{site.SystemMsgTitle}">&nbsp;</td>
			</tr>
			<tr>
				<td style="vertical-align:top">_{msgBody}</td>
				<td><div style="width:600px;">#{Editor}</div></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2" style="font-size:16px;font-weight:bold; padding-bottom:10px ">_{regForbidden}<span class="font12 left5 red">(_{wordFilterTip})</span>
				</td>
			</tr>
			<tr>
				<td style="vertical-align:top">_{forbiddenUserName}</td>
				<td>
				<textarea name="ReservedUserName" style="width: 600px; height: 60px">#{site.ReservedUserNameStr}</textarea></td>
			</tr>
			<tr>
				<td style="vertical-align:top">_{forbiddenUserUrl}</td>
				<td>
				<textarea name="ReservedUserUrl" style="width: 600px; height: 60px">#{site.ReservedUserUrlStr}</textarea></td>
			</tr>
			<tr>
				<td style="vertical-align:top">保留关键词</td>
				<td>
				<textarea name="ReservedKeyString" style="width: 600px; height: 150px">#{site.ReservedKeyString}</textarea></td>
			</tr>
	
	
			<tr>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td><input name="Submit1" type="submit" value="_{editSetting}" class="btn">&nbsp;</td>
			</tr>
		</table>
		</form>
	
	
	</div>	
