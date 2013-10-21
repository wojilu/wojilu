
<style>
.groupMy td{vertical-align:top;}
.groupMy .title{background:#fff;color:#222; width:98%; margin:auto; margin-top:0px; margin-bottom:15px; font-size:14px; padding:2px 5px}

td.regLeft {font-weight: bold; width:80px;}
.groupAccess .note {margin-left: 20px;margin-bottom: 5px;}

.tabList li {width:100px;}
</style>


<div style="margin:0px">

<div style="padding:10px 20px; ">
	<form method="post" action="#{searchActionUrl}">
	<table class="cbg" style="width: 100%;margin:5px 0px 15px 0px; border-radius:4px;" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td style="padding:5px 5px 5px 10px"><img src="~img/app/m/group.png"/> <span class="strong">_{group}</span> </td>
			<td align="right" style="padding:5px 10px 5px 5px">
			<input name="term" type="text" />&nbsp;<input name="Submit1" type="submit" value="_{search}" class="btn btns" style="width:55px;" />
			<a href="#{allGroupLink}" target="_blank" class="left5">&rsaquo;&rsaquo; _{allGroup}</a>
			</td>
		</tr>
	</table>
	</form>
    <ul class="tabList" style="margin-bottom:20px;">
        <li class="firstTab"><a href="#{groupHome}" class="frmLink" loadto="groupList" nolayout=4>群组首页</a><span></span></li>
        <li><a href="#{lnkMyGroup}" class="frmLink" loadto="groupList" nolayout=4>我的群组</a><span></span></li>
        <li class="hide"><a href="#{lnkMyPost}" class="frmLink" loadto="groupList" nolayout=4>我的帖子</a><span></span></li>
        <li style="width:150px;"><a href="#{lnkFriendGroup}" class="frmLink" loadto="groupList" nolayout=4>_{friendJoinedGroup}</a><span></span></li>
        <!-- BEGIN addGroup -->
        <li><a href="#{addGroupUrl}" class="frmLink" loadto="groupList" nolayout=4><img src="~img/add.gif" /> _{createGroup}</a><span></span></li>
        <!-- END addGroup -->
    </ul>
	

		
	<div id="groupList">#{layout_content}</div>

</div>

</div>

<script>
_run( function() {
    wojilu.tool.makeTab('.tabList', 'currentTab', '');
});
</script>
    