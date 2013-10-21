
<div style="padding:20px">
			<table style="width: 100%">
				<tr>
					<td style="width:80px;">_{userName}</td>
					<td><strong>#{user.Name}</strong></td>
				</tr>
				<tr>
					<td>_{role}</td>
					<td>#{user.RoleName}</td>
				</tr>
				<tr>
					<td>_{rank}</td>
					<td>#{user.RankName} #{user.StarHtml}</td>
				</tr>
				<tr>
					<td>_{forumPost}</td>
					<td>#{user.PostCount}</td>
				</tr>
				<tr>
					<td>_{regTime}</td>
					<td>#{user.Created}</td>
				</tr>
				<tr>
					<td>_{lastLoginTime}</td>
					<td>#{user.LastLogin}</td>
				</tr>
				<tr>
					<td>_{lastUpdate}</td>
					<td>#{user.LastUpdate}</td>
				</tr>
			</table>		
	
			<table id="dataAdminList" style="width:100%;margin-top:10px; border-left:0px; border-right:0px;">
				<tr class="tableHeader" style="font-weight:bold;">
					<td style="width:100px;">_{currency}</td><td>_{income}</td>
				</tr>
				<!-- BEGIN list -->
				<tr class="tableItems">
					<td>#{c.CurrencyName}</td><td>#{c.Income}</td>
				</tr><!-- END list -->
			
			</table>		

</div>