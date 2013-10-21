
	
		<div id="forumRoleList" style="padding:10px;">	
			<div class="strong" style="margin:2px 0px 10px 0px;">:{currentBoard}: #{forumBoard.Name} </div>
	

<style type="text/css">
.actionHeader td {vertical-align:top; background:#efefef;}
.actionHeader td div {width:15px;margin:auto;line-height:110%}
.rollover td {text-align:center}
td.roleName {padding:0px 5px 0px 5px; width:80px; font-weight:bold; text-align:left}
</style>		
			
			
			<form method="post" action="#{ActionLink}">				
				
			<table style="width: 98%;border-collapse:collapse;border:1px #c4ddff solid;background:#fff " border="1" >	
				
				<tr class="actionHeader"><td>&nbsp;</td>
				<!-- BEGIN actions --><td><div>#{a.Name}</div></td><!-- END actions -->
				</tr>
				
				<!-- BEGIN sysroles -->
				#{seperator}
				<tr class="rollover">
					<td class="roleName">#{role.Name}</td>
					<!-- BEGIN checkboxs --><td title="#{a.Name}"><input name="SecurityAction" type="checkbox" value="#{a.Value}" #{a.Checked} /></td><!-- END checkboxs -->
				</tr>
				<!-- END sysroles -->
				
			</table>
			
			<table style="width: 95%; " >
				<tr>
					<td>&nbsp;</td>
					<td style="padding:10px 10px 0px 10px;"><input name="Submit1" type="submit" value=":{saveSecurity}" class="btn">&nbsp;&nbsp;
					<span href="#{resetLink}?frm=true" class="postCmd cmd left20 right20">:{restoreDefault}</span>
					<input class="btnCancel" name="Button1" type="button" value="_{cancel}"></td>
				</tr>
			</table>

			</form>
		</div>
	
