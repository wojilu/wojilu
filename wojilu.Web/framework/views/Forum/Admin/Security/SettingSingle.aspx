<div id="forumRoleList">

	<style>
	#forumRoleList h4 { margin-left:20px;}
	#forumRoleList div { margin-left:20px; width:300px; padding-left:10px; border-bottom:1px #aaa dotted;}
	.rName {width:120px; display:inline-block;}
	.rLinkEdit {width:60px;display:inline-block;}
	.rLinkSet {width:60px;display:inline-block;}
	</style>
	
	<h4><a href="#{settingLink}" style="font-weight:normal">_{setSecurity}</a>
	<span style="margin-left:20px;">:{setAllItems}</span></h4>

	

	<!-- BEGIN forumroles -->
	<div class="rollover">
		<span class="rName">#{role.Name}</span>
		<span class="rLinkSet"><a href="#{role.LinkSetForumPermission}" class="frmBox">_{setSecurity}</a></span>
	</div>
	<!-- END forumroles -->
	
<div style="margin-bottom:10px;"></div>
	<!-- BEGIN sysroles -->
	<div class="rollover">
		<span class="rName">#{role.Name}</span>
		<span class="rLinkSet"><a href="#{role.LinkSetPermission}" class="frmBox">_{setSecurity}</a></span>
	</div>
	<!-- END sysroles -->

</div>