<div>


	<table id="dataAdminList1" border="0" cellpadding="2" cellspacing="0" style="width: 97%; margin:10px auto;">

		<tr class="tableItems">
		<td style="background:#fff; padding-top:10px; padding-right:10px;">
			
    <!-- BEGIN cat -->
    <div style="border-bottom:1px #ccc solid;"><a href="#{cat.Link}" style="font-weight:bold; font-size:14px;">#{cat.Name}</a>#{cat.ThumbIcon}
    </div>
    <div style="margin-bottom:20px;">
        <!-- BEGIN subcat -->#{subcat.ThumbIcon}<a href="#{subcat.Link}">#{subcat.Name}</a><span class="note right10">(#{subcat.DataCount})</span><!-- END subcat -->
    </div>
    <!-- END cat -->
			
		</td>
		</tr>
	</table>

</div>