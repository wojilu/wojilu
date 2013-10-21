		<div class="userFace" style="text-align:center;margin-top:5px;"><a href="#{profile}"><img src="#{user.Face}" class="avm" /></a></div>
		
		
		<div class="sideBar_Panel" style="border:0px;">
			<div class="sideBar_Title hide">_{userMenu}</div>
			<table class="sideBar_UserTool" cellpadding="2" style="margin:auto;">
				<tr>
					<td>
					
					<a href="#{user.AddFriendLink}" class="frmBox cmd" xwidth="500" title="_{addAsFriend}"><span>_{addAsFriend}</span></a><a href="#{user.AddFollowLink}" class="frmBox cmd left10" title="_{followcmd}"><span>_{followcmd}</span></a>
					</td>
				</tr>
				<tr>
					<td><a href="#{sendMsgLink}" class="cmd" id="sendMsgCmd"><span>_{sendMsg}</span></a>
					<a href="#{shareLink}" class="frmBox left5 cmd" xwidth="500" title="_{shareUser}"><span>_{shareUser}</span></a></td>
				</tr>
			</table>
			<table class="friendCountTable" style="margin:auto;">
				<tr>
					<td style="border-right:1px #ccc solid"><a href="#{user.FriendLink}"><span class="friendCount">#{user.FriendCount}</span><br/>_{friend}</a></td>
					<td style="border-right:1px #ccc solid"><a href="#{user.FollowingLink}"><span class="friendCount">#{user.FollowingCount}</span><br/>_{follow}</a></td>
					<td><a href="#{user.FollowerLink}"><span class="friendCount">#{user.FollowersCount}</span><br/>_{fans}</a></td>
				</tr>
			</table>
		</div>
