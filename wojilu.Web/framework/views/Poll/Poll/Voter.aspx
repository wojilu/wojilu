<div style="margin:10px;">

<div style="background:#f2f2f2; border-bottom:1px #aaa solid; padding:5px 10px; font-size:20px; font-weight:bold;">_{voteResult}</div>

<div style="margin:10px 20px; margin-bottom:30px">#{voteResult}</div>

<div style="background:#f2f2f2; border-bottom:1px #aaa solid; padding:5px 10px; font-size:20px; font-weight:bold;">_{voter}</div>

<table border="1" style="width: 98%; margin:10px 5px 20px 10px; border-collapse:collapse; border:1px #ccc solid;" cellpadding="3">
	<tr>
		<th>_{voter}</th>
		<th>_{option}</th>
		<th>_{voteTime}</th>
	</tr>
<!-- BEGIN list -->
	<tr>
		<td>#{user.Name}</td>
		<td>#{user.Choice}</td>
		<td>#{user.Created}</td>
	</tr>
	<!-- END list -->
</table>
<div>#{page}</div>
</div>