
	
		<div style="padding:15px;">
		<div style="margin:10px 10px;" class="warning">		
			<table style="width: 100%;border-collapse:collapse;border:1px #aaa solid;" border="1">
				<tr>
					<td style="width:60px;">:{deleteUser}</td>
					<td>#{log.UserName} <span class="note">(ip:#{log.Ip})</span></td>
				</tr>
				<tr>
					<td>:{operationInfo}</td>
					<td>#{log.MsgInfo}</td>
				</tr>
				<tr>
					<td>:{operationTime}</td>
					<td>#{log.Created}</td>
				</tr>
			</table>		
		</div>	
		
			<table border="1" style="width: 100%;border-collapse:collapse;border:1px #fff solid;" cellpadding="5" cellspacing="0">
				<tr>
					<td colspan="2" style="font-size:20px;padding:10px;font-weight:bold;">#{p.Title}</td>
				</tr>
				<tr>
					<td style="width:80px">:{itsTopic}</td>
					<td class="strong"><a href="#{t.Link}" target="_blank">#{t.Title}</a></td>
				</tr>
				<tr>
					<td>:{pubTime}</td>
					<td class="note">#{p.Created} _{author}：<a href="#{p.CreatorLink}" target="_blank">#{p.Creator.Name}</a></td>
				</tr>
				<tr>
					<td style="vertical-align:top">_{content}:</td>
					<td style="">#{p.Content}</td>
				</tr>
			</table>
		
		</div>

