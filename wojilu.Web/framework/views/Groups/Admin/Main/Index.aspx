

<style>
td.regLeft {font-weight:bold; vertical-align:top;}
.groupAccess .note {margin-left:10px; }
.groupAccess div { margin:0px 0px 5px 0px;}

#settingTable {width:98%;}
#settingTable td { border-bottom:1px #ccc solid; padding:10px 5px;}
#settingTable .bottomRow td {border-bottom:0px;}
</style>

<div class="formPanel" style="background:#fff;border:0px;">
<form method="post" action="#{ActionLink}" style="padding:10px 10px 20px 20px;" class="ajaxPostForm">


	<table id="settingTable" cellspacing="0" border="0">
		<tr>
			<td class="regLeft">_{groupName}</td>
			<td style="">
			    <span style="font-weight:bold; font-size:18px; color:Red;">#{g.Name}</span>
			    <span class="left10">#{siteUrl}/#{groupPath}/#{g.Url}#{cfg.UrlExt}</span>
			</td>
		</tr>
		<tr>
			<td class="regLeft">_{groupCategoryIs}</td>
			<td>#{Category} <span class="note">_{groupCategoryNote}</span></td>
		</tr>
		<tr>
			<td class="regLeft">_{groupPrivacy}</td>
			<td class="groupAccess">
			<div>
			    <input id="ac0" name="AccessStatus" type="radio" value="0" #{accessChecked0} /><label for="ac0">_{groupOpen}</label>
			    <span class="note">_{groupOpenInfo}</span>
			</div>			
			<div>
			    <input id="ac1" name="AccessStatus" type="radio" value="1" #{accessChecked1} /><label for="ac1">_{groupClosed}</label>
			    <span class="note">_{groupClosedInfo}</span>			
			</div>
			<div>
			    <input id="ac2" name="AccessStatus" type="radio" value="2" #{accessChecked2} /><label for="ac2">_{groupSecret}</label>
			    <span class="note">_{groupSecretInfo}</span>
			
			</div>
			
			</td>
		</tr>
		<tr class="bottomRow">
			<td class="regLeft">
                <div>_{groupDescription}</div>
                <div style=" font-weight:normal; color:#666;">(一句话)</div>
            </td>
			<td><textarea name="Description" style="width: 98%; height: 100px">#{g.Description}</textarea>
			</td>
		</tr>
		<tr class="bottomRow">
			<td class="regLeft">申请加入</td>
			<td>
			    <label>
                <input name="IsCloseJoinCmd" type="checkbox" #{g.IsCloseJoinCmd} /> 关闭 “<strong>申请加入</strong>”  按钮
                </label>
                <span class="note left10">(关闭之后，除非小组管理员邀请，用户不能加入小组。)</span>
			</td>
		</tr>
		<tr class="bottomRow">
			<td></td>
			<td>
			<input name="Submit1" type="submit" value="_{saveUpdate}" class="btn"></td>
		</tr>
	</table>
</form>
</div>
