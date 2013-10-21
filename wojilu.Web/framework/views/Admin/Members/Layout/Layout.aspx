<div>
	<table class="tabHeader" style="width:98%; font-size:12px;" cellpadding="0" cellspacing="0">
		<tr>
		    <td class="otherTab" style="width:5%">&nbsp;</td>
			<td class="tabCmd otherTab" style="width:13%"><a href="#{userListLink}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/list.gif" /> _{userList}</a></td>
			<td class="tabCmd otherTab" style="width:13%"><a href="#{siteMsgLink}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/msg.gif" /> _{msgToAll}</a></td>
            <td class="tabCmd otherTab" style="width:13%"><a href="#{regLink}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/addUser.gif" /> 快速注册</a></td>
			<td class="tabCmd otherTab" style="width:13%"><a href="#{importLink}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/user.gif" /> 用户导入</a></td>
            <td class="tabCmd otherTab" style="width:13%"><a href="#{settingLink}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/user.gif" /> 用户设置</a></td>

			<td class="tabCmd otherTab" style="width:13%"><a href="#{confirmTemplateLink}" target="_blank"><img src="~img/img.gif" /> _{confirmTemplate}</a></td>
			
			<td class="otherTab" style="width:43%">&nbsp;</td>
		</tr>
	</table>
	
	<div class="tabMain" id="tabMain" style="width:98%; ">
	#{layout_content}
	</div>

</div>
