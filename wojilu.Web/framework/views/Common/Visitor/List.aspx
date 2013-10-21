	<h4>_{showVisitLogLable}</h4>
	<a name="visitorList"></a>
	<table style="width: 100%">
		<tr>
			<td>
			<!-- BEGIN visitors -->
			<div style="float:left;margin:10px;text-align:center">
			<a href="#{user.Link}" target="_blank"><img src="#{user.Face}" class="avs" /></a><br/>
			<a href="#{user.Link}" target="_blank">#{user.Name}</a>
			</div>
			<!-- END visitors -->
			
			<div style="float:left;margin:10px;text-align:center" href="#{visitLink}" class="putCmd visitCmd"><span>_{showVisitLog}</span></div>										
			</td>
		</tr>
	</table>