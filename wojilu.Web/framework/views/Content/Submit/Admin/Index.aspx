
<div>

<div style="padding:10px;">

<table cellspacing="0" cellpadding="5" border="0" style="width: 98%;margin:10px 10px;" id="dataAdminList" data-action="#{OperationUrl}">
    <tr class="adminBar">
        <td colspan="4" style="padding-left:10px;">
            <span class="btnCmd" cmd="pass"><img src="~img/s/accept.png" /> 批量通过</span>
            <span class="btnCmd left10" cmd="nopass"><img src="~img/s/delete.png" /> 不通过</span>
        </td>
        <td style="text-align:right;padding-right:15px;">        
            
        </td>
    </tr>

	<tr class="tableHeader">
	        <td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
            <td>投递人</td>
            <td>标题</td>
            <td>投递时间</td>
            <td>管理(同时发送消息)</td>
        </tr>
        <!-- BEGIN list -->
	<tr class="tableItems">
	        <td align="center"><input name="selectThis" id="checkbox#{post.Id}" type="checkbox" class="selectSingle"> </td>
            <td><a href="#{post.UserLink}" target="_blank">#{post.Creator.Name}</a>
                </td>
            <td><a href="#{post.ShowLink}">#{post.Title}</a></td>
            <td>#{post.Created}</td>
            <td>
                <img src="~img/s/accept.png" /> <a href="#{post.PassLink}" class="frmBox right10" title="通过">通过</a>
                <img src="~img/s/delete.png" /> <a href="#{post.NoPassLink}" class="frmBox" title="不通过">不通过</a>
                &nbsp;</td>
        </tr><!-- END list -->
	<tr>
		<td colspan="5" class="adminPage">#{page}</td>
	</tr>

    </table>
</div>
</div>

<style>
#dataAdminList {width:90%;margin-top:0px;font-size:12px;  border-collapse:collapse;}
.adminBar td{padding:0px 0px 0px 5px; border-bottom:1px #6699cc solid; background:url("~img/admin/gridtoolbar-back.gif");  height:26px; }
.cmd,.ajaxCmd,.btnCmd {color:#15428b;cursor:pointer;text-decoration:; margin-right:5px; font-size:12px;}
tr.tableHeader { background: #f0f0f0 url("~img/admin/grid-hrow.gif"); color:#000;}								
.tableHeader td{border-bottom:0px #999999 solid;padding:0px 5px 0px 5px; height:26px;}
tr.tableItems { background-color: #fff; }	

.tableItems td a{color:#000;}
.tableItems table td {border-bottom:0px;}
td.adminPage {background:#efefef;}

</style>
