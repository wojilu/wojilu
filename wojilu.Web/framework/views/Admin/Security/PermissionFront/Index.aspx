
<style>
.appCell {text-align:center; padding:3px 5px;}

.sectionTitle {
	border-bottom: 1px #aaa dotted;
	font-family: Microsoft YaHei, Calibri;
	font-size: 18px;
	font-weight: bold;
	padding: 10px 5px 6px 0px;

}

</style>

<div class="" style="padding:20px">

	<form action="#{ActionLink}" method="post" class="ajaxPostForm">

	<table cellpadding="1">
		<tr class="strong">
			<td style="width:80px;">&nbsp;</td>
			
			<!-- BEGIN applist -->
			<td class="appCell">#{app.Name}</td>
			<!-- END applist -->
		</tr>
				
		<!-- BEGIN roles -->
		<tr class="rollover">
			<td>#{role.Name}</td>
			<!-- BEGIN apps -->
			<td class="appCell"><input name="appRole" type="checkbox" value="#{app.Id}_#{role.Id}_#{role.Type}" #{checkedString} /></td>
			<!-- END apps -->
		</tr>
		<!-- END roles -->
	
	</table>
	<div style="margin:10px 10px 10px 85px;"><input name="Submit1" type="submit" value="_{updateSecurity}" class="btn" /></div>
	</form>
	
</div>

