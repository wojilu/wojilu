
<style type="text/css">
.menuHorizontal {margin:0px;padding:0px; clear:both}
.menuHorizontal li{float:left;margin:3px 5px 1px 5px; list-style:none;}
</style>


<table cellspacing="0" cellpadding="2" border="0" style="width:98%;margin:10px auto;clear:both" id="dataAdminList" data-action="#{ActionLink}">
<tr class="adminBar">


    <td colspan="3">
        <img src="~img/list.gif" /> :{draft}
    </td>
    <td colspan="2" style="text-align:right;">
        <span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span>  
    </td>

</td>

</tr>
	<tr class="tableHeader">
		<td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
		<td align="center" style="width:12%;">_{category}</td>
		<td style="width:54%;">_{title}</td>
		<td align="center" style="width:17%;">_{created}</td>
		<td align="center" style="width:13%;">_{admin}</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
		<td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox" class="selectSingle"> </td>
		<td align="center"><a href="#{post.CategoryUrl}">#{post.CategoryName}</a></td>
		<td class=""><a href="#{post.EditUrl}">#{post.Title}</a></td>
		<td align="center">#{post.Created}</td>
		<td align="center" class="underline"><a href="#{post.EditUrl}" class="edit">_{edit}</a>
		<a href="#{post.Url}" target="_blank">预览</a>
		</td>
	</tr>
	<!-- END list -->
	<tr>
		<td colspan="5" class="adminPage">#{page}</td>
	</tr>
</table>

