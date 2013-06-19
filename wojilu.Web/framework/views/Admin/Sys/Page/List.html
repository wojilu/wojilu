
	<div class="adminMainPanel">
	    <div class="hide">_{pageAdmin}(_{pageTopic}:#{category.Name})</div>
		<table id="dataAdminList" data-sortAction="#{sortAction}" border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
			<tr class="adminBar">
				<td colspan="10">
                    <span class="right20"><a href="#{addLink}"><img src="~img/add.gif" /> _{addPage}</a></span> 
                    <select id="transDrop">
                        <option value="0">转移到其他专题...</option>
                        <!-- BEGIN transList -->
                        <option value="#{x.Id}" data-url="#{x.data.TransLink}">#{x.Name}</option>
                        <!-- END transList -->
                    </select>
                </td>
			</tr>
			<tr class="tableHeader">
			<td align="center" style="width:4%;"><input id="selectAll" class="selectAll" type="checkbox" title="_{checkAll}" /></td>
				<td style="width:7%">_{orderId}</td>
				<td style="width:10%">_{subPage}</td>
                <td></td>
				<td >_{title}</td>
				<td style="width:13%">_{created}</td>
				<td style="width:5%">_{view}</td>
				<td style="width:5%">_{comment}</td>
				<td style="width:10%;">_{allowComment}</td>
				<td style="width:15%">_{admin}</td>
			</tr>
			<!-- BEGIN list -->
			<tr class="tableItems">
			<td align="center"><input name="selectThis" id="checkbox#{data.Id}" type="checkbox" class="selectSingle"/> </td>
		         <td class="sort">
			        <img src="~img/up.gif" class="cmdUp right10" data-id="#{data.Id}"/>
			        <img src="~img/dn.gif" class="cmdDown" data-id="#{data.Id}"/>
		        </td>
				<td><a href="#{data.AddSubLink}"><img src="~img/add.gif" /> _{addSubPage}</a></td>
                <td>#{data.IsCollapseStr}</td>
				<td style="#{data.Indent}"><a href="#{data.LinkShow}" target="_blank"><strong>#{data.Title}</strong></a>
				<a class="frmBox" href="#{data.ViewUrl}" title="_{viewUrl}" style="font-weight:normal;color:blue;margin-left:5px;">&rsaquo;&rsaquo; _{viewUrl}</span>
				</td>
				<td>#{data.Created}</td>
				<td>#{data.Hits}</td>
				<td>#{data.ReplyCount}</td>
				<td>#{data.IsAllowReplyStr}</td>
				<td>
					<a href="#{data.LinkEdit}"><img src="~img/edit.gif" /> _{edit}</a>
					<span href="#{data.LinkDelete}" class="deleteCmd left10"><img src="~img/delete.gif" /> _{delete}</span>
				</td>
			</tr>
			<!-- END list -->

		</table>
	</div>

<script>
    _run(function () {



        $('#transDrop').change(function () {
            var choiceList = wojilu.ui.select();
            if (choiceList.length == 0) {
                alert(lang.exSelect);
                return;
            }

            var url = $(this).find(":selected").attr('data-url');
            if (!url) return;

            $.post(url, { 'ids': choiceList }, function (data) {
                if (data == 'ok') {
                    wojilu.tool.reloadPage();
                }
                else {
                    alert(data);
                }
            });

        });

    });
</script>