<div style="padding:10px;">

<style>
.ftitle {font-size:14px;padding:5px 10px; border-radius:4px; }
.userList{font-size:12px;}
.fUserItem{padding-top:5px;width:120px; height:100px; border-radius:4px; }
.fUserPic {border-radius:4px;width:48px;}
</style>

	<div class="ftitle cbg">_{friendsOfFriends}</div>	

	<table style="width: 100%" class="userList">
		<tr>
			<td>
				<!-- BEGIN flist --><div style="float:left;margin:10px;text-align:center" class="fUserItem cborder">
				<a href="#{user.Link}" target="_blank"><img src="#{user.Face}" class="fUserPic" /></a><br/>
				<a href="#{user.Link}" target="_blank">#{user.Name}</a><br/>
				<a href="#{user.AddLink}" class="frmBox" xwidth="500" title="_{addAsFriend}"><img src="~img/add.gif" /> _{addAsFriend}</a>
				</div><!-- END flist -->
			</td>
		</tr>
	</table>
	
	<div class="ftitle cbg top20">_{whoIsOnline}</div>	

	<table style="width: 100%" class="userList">
		<tr>
			<td>
				<!-- BEGIN online --><div style="float:left;margin:10px;text-align:center;" class="fUserItem cborder">
				<a href="#{user.Link}" target="_blank"><img src="#{user.Face}" class="fUserPic" /></a><br/>
				<a href="#{user.Link}" target="_blank">#{user.Name}</a><br/>
				<a href="#{user.AddLink}" class="frmBox" xwidth="500" title="_{addAsFriend}"><img src="~img/add.gif" /> _{addAsFriend}</a>
				</div><!-- END online -->			
			</td>
		</tr>
	</table>
	
	<div class="ftitle cbg top20">_{userHotest}</div>
	
	<table style="width: 100%" class="userList">
		<tr>
			<td>
				<!-- BEGIN top --><div style="float:left;margin:10px;text-align:center;" class="fUserItem cborder">
				<a href="#{user.Link}" target="_blank"><img src="#{user.Face}" class="fUserPic" /></a><br/>
				<a href="#{user.Link}" target="_blank">#{user.Name}</a><br/>
				<a href="#{user.AddLink}" class="frmBox" xwidth="500" title="_{addAsFriend}"><img src="~img/add.gif" /> _{addAsFriend}</a>
				</div><!-- END top -->			
			</td>
		</tr>
	</table>	
	

</div>
