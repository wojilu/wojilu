
	<div class="" style="width:98%;margin:auto;">
	
	<div style="margin-left:10px;"><strong>:{setForumDefaultPermission}</strong></div>
	
<script type="text/javascript">
<!--
_run( function() {
	$('.saveAndApply').click( function() {
		var actionLink = $('#myform').attr( 'action' ) + "?applyAll=true";
		$('#myform').attr( 'action', actionLink );
	});
});
//-->
</script>

<style type="text/css">

.roleMain td {border:1px #efefef solid;}
.actionHeader td {vertical-align:top; background:#efefef;}
.actionHeader td div {width:15px;margin:auto;line-height:110%}

.over {	background:#fff;}
.over td {text-align:center}
.over input {border:0px;}
td.roleName {padding:0px 5px 0px 5px; width:70px; font-weight:bold; text-align:left}
</style>
	
		<div id="forumRoleList" style="padding:5px">				
			
			<form id="myform" method="post" action="#{ActionLink}">
			
			<table style=" width:99%; border-collapse:collapse;border:1px #c4ddff solid;background:#fff " border="1" class="roleMain">	
				
				<tr class="actionHeader"><td>&nbsp;</td>
				<!-- BEGIN actions --><td><div>#{a.Name}</div></td><!-- END actions -->
				</tr>
				
				<!-- BEGIN sysroles -->
				#{seperator}
				<tr class="over">
					<td class="roleName">#{role.Name}</td>
					<!-- BEGIN checkboxs --><td title="#{a.Name}"><input name="SecurityAction" type="checkbox" value="#{a.Value}" #{a.Checked} /></td><!-- END checkboxs -->
				</tr>
				<!-- END sysroles -->
				
			</table>
			
			<table>
				<tr>
					<td>&nbsp;</td>
					<td style="padding:10px 10px 0px 10px;">
					<input name="Submit1" class="saveAndApply btn" type="submit" value=":{saveAndApplyToBoards}">
					<input name="Submit1" type="submit" class=" left20 right20 btn" value=":{onlySaveAsTemplate}" title=":{noApplyToBoards}">
					
					<input class="btnReturn" name="Button1" type="button" value="_{return}">
					</td>
				</tr>
			
			</table>

			</form>
	
		</div>
	
	</div>
