
<div>

<div style="padding:10px;">

<table cellspacing="0" cellpadding="2" border="0" style="width: 98%;margin:10px 10px;" id="dataAdminList">
    <tr class="adminBar"><td colspan="4" style="padding-left:10px;"><img src="~img/add.gif" /> <a href="#{addUrl}" class="frmBox">添加记者</a></td></tr>

	<tr class="tableHeader">
            <td>
                用户</td>
            <td>
                角色</td>
            <td>
                加入时间</td>
            <td>
                管理</td>
        </tr>
        <!-- BEGIN list -->
	<tr class="tableItems">
            <td><a href="#{s.UserLink}" target="_blank">#{s.User.Name}</a>
                </td>
            <td>
                #{s.RoleName}</td>
            <td>
                #{s.Created}</td>
            <td>
                <a href="#{s.DeleteLink}" class="deleteCmd">_{delete}</a>
            
                &nbsp;</td>
        </tr><!-- END list -->
	<tr>
		<td colspan="4" class="adminPage">#{page}</td>
	</tr>

    </table>
</div>
</div>
