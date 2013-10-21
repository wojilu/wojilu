
<style>
.lblTop {font-weight:bold;color:red; margin-right:3px;}
#adminDropCategoryList {width:150px;}
</style>

<table cellspacing="0" cellpadding="2" border="0" style="width: 100%;clear:both;margin:auto;" id="dataAdminList" data-action="#{ActionLink}">
<tr class="adminBar"><td colspan="6">

    <div style="float:left;">
	    <span class="gridSpace">&nbsp;</span>

	    <span id="btnTop" class="btnCmd" cmd="top"><img src="~img/sticky.gif" /> _{sticky}</span>
	    <span id="btnPick" class="btnCmd" cmd="pick"><img src="~img/star.gif" /> :{pickblog}</span>
    	
	    <span class="gridSpace">&nbsp;</span>
    	
	    <span id="btnUnTop" class="btnCmd" cmd="untop"><img src="~img/cancel.gif" /> _{unsticky}</span>
	    <span id="btnUnPick" class="btnCmd" cmd="unpick"><img src="~img/cancel.gif" /> :{unpick}</span>
    	
	    <span class="gridSpace">&nbsp;</span>
    	
	    <span style="margin:0px 5px 0px 2px;">#{blog.CategoryList}</span>	
	    <span id="btnDelete" class="btnCmd" cmd="delete"><img src="~img/trash.gif" /> _{toTrash}</span>
    </div>

    <div style="float:right;padding-top:3px;">
	    <span id="btnDeleteTrue" class="btnCmd" cmd="deletetrue"><img src="~img/delete.gif" /> _{deleteTrue}</span>
    </div>

</td>

</tr>
	<tr class="tableHeader">
		<td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
		<td align="center" style="width:9%;">_{category}</td>
		<td style="width:45%;">_{title}</td>
		<td align="center" style="width:11%;">_{view}/_{comment}</td>
		<td align="center" style="width:17%;">_{created}</td>
		<td align="center" style="width:9%;">_{admin}</td>
	</tr>
	<!-- BEGIN list -->
	<tr class="tableItems">
		<td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox" class="selectSingle"> </td>
		<td align="center"><a href="#{post.CategoryUrl}">#{post.CategoryName}</a></td>
		<td class="">#{post.Status}<a href="#{post.Url}" target="_blank">#{post.Title}</a></td>
		<td align="center">#{post.Hits} <span class="note">|</span> #{post.ReplyCount}</td>
		<td align="center">#{post.Created}</td>
		<td align="center" class="underline"><a href="#{post.EditUrl}" class="edit"><img src="~img/edit.gif" /> _{edit}</a></td>
	</tr>
	<!-- END list -->
	<tr>
		<td colspan="6" class="adminPage">#{page}</td>
	</tr>
</table>

