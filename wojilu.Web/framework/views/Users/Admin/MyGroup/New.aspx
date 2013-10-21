<style>
.regLeft { padding:3px 5px; vertical-align:top;}
</style>
<form method="post" action="#{ActionLink}">

	<table class="tabHeader" style=" width:96%;" cellpadding="0" cellspacing="0">
		<tr>
		    <td class="otherTab" style="width:5%;"></td>
			<td class="currentTab" style="width:20%;">_{createGroup1}</td>
			<td class="otherTab" style="width:20%">_{createGroup2}</td>
			<td class="otherTab" style="width:20%">_{createGroup3}</td>
			<td class="otherTab" style="width:35%">&nbsp;</td>
		</tr>
	</table>
	<div class="tabMain" style=" width:96%;" >
	<div style="padding:10px">

	<table style="margin:0px 10px 10px 10px; width:580px;" border="0">

		<tr>
			<td class="regLeft cbg">_{groupName}<span class="red">*</span></td>
			<td><input name="Name" type="text" > <span class="valid" msg="_{groupNameNote}" rule="name_cn" ajaxAction="#{isNameValidLink}"></span> </td>
		</tr>
		<tr>
			<td class="regLeft cbg">_{groupUrl}<span class="red">*</span></td>
			<td>#{siteUrl}/#{groupPath}/<input name="FriendUrl" type="text" style="width:80px;" />#{urlExt}			
			<br>
			<span class="valid" to="FriendUrl" msg="_{groupUrlNote}" show="true" rule="name" ajaxAction="#{isUrlValidLink}"></span>
			</td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
			<td class="regLeft cbg">_{groupCategoryIs}<span class="red">*</span></td>
			<td>#{Category} <span class="note">_{groupCategoryNote}</span></td>
		</tr>
		<tr>
			<td class="regLeft cbg" style="vertical-align:top;">_{groupDescription}<span class="red">*</span></td>
			<td><textarea name="Description" style="width: 310px; height: 119px"></textarea>
			<span class="valid" msg="_{exGroupDescription}"></span>
			</td>
		</tr>
		<tr>
			<td class="regLeft cbg">_{groupPrivacy}<span class="red">*</span></td>
			<td class="groupAccess">
			<input id="ac0" name="AccessStatus" type="radio" value="0" #{accessChecked0} /><label for="ac0">_{groupOpen}</label><br/>
			<span class="note">_{groupOpenInfo}</span><br/>
			<input id="ac1" name="AccessStatus" type="radio" value="1" #{accessChecked1} /><label for="ac1">_{groupClosed}</label><br/>
			<span class="note">_{groupClosedInfo}</span><br/>
			<input id="ac2" name="AccessStatus" type="radio" value="2" #{accessChecked2} /><label for="ac2">_{groupSecret}</label><br/>
			<span class="note">_{groupSecretInfo}</span>
			
			</td>
		</tr>
		<tr>
			<td></td><td>
			<input name="Submit1" type="submit" value="_{createGroup}" class="btn">
			<input name="Button1" type="button" value="_{return}" class="btnReturn" />
			</td>
		</tr>
	</table>
	</div>
	</div>
</form>
