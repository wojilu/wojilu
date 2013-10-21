
<div>

	<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;" id="dataAdminList" data-action="#{ActionLink}">
		<tr class="adminBar">
        <td colspan="7">

            <div style="float:left;">
			    <span id="btnPick" class="btnCmd left10" cmd="pick"><img src="~img/pick.gif" /> _{recommend}</span>
                <span style="margin:0px 5px 0px 2px;">#{adminDropCategoryList}</span>
			    <span class="left10 btnCmd" cmd="delete"><img src="~img/delete.gif" /> _{toTrash}</span>
            </div>

            <div style="float:right;">
                <span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span>
            </div>

		</td>
		</tr>


		<tr class="tableHeader">
			<td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
			<td>_{author}</td>
            <td>系统分类</td>
			<td style="width:50%;">_{title}</td>
			<td align="center" style="width:9%;">_{view}/_{comment}</td>
			<td align="center" style="width:14%;">_{created}</td>
			<td align="center" style="width:9%;">_{admin}</td>
		</tr>
		<!-- BEGIN list -->
		<tr class="tableItems">
			<td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox" class="selectSingle"> </td>
			<td><a href="#{post.UserLink}" target="_blank">#{post.UserName}</a></td>
            <td>#{post.SysCategoryName}</td>
			<td class="">#{post.Status}<a href="#{post.Url}" target="_blank">#{post.Title}</a></td>
			<td align="center">#{post.Hits} <span class="note">|</span> #{post.ReplyCount}</td>
			<td align="center">#{post.CreateTime}</td>
			<td align="center" class="underline"><span href="#{post.DeleteLink}" class="deleteCmd"><img src="~img/delete.gif" /> _{delete}</span></td>
		</tr>
		<!-- END list -->
		<tr>
			<td colspan="7" class="adminPage">#{page}</td>
		</tr>
	</table>
</div>
