	<table style="width: 100%;margin:0px;background:#fff">
		<tr>
			<td style="vertical-align:top;width:85%;" id="friendsImg">
                <div>
				<!-- BEGIN list -->				
				<div style="float:left; display:inline; width:140px; height:155px; margin:7px; background:#f2f2f2;text-align:center;">
				    <div style="padding:5px;">
					<div style=""><a href="#{post.Link}" target="_blank"><img src="#{post.ImgUrl}" class="max100" /></a></div>
					<div style="margin-top:5px;"><a href="#{author.Link}" target="_blank"><span class="note">_{author}:</span>#{author.Name}</a></div>
					<div class="note">#{post.Created}</div>
					</div>
				</div>
				<!-- END list -->
				<div style="clear:both"></div>
				</div>
				<div>#{page}</div>			
			</td>
			<td style="width:15%;vertical-align:top;padding:10px;">
			
				<div><strong>_{viewByFriends}</strong></div>
				<ul>
					<!-- BEGIN friends -->
					<li><a href="#{user.BlogLink}">#{user.Name}</a></li>
					<!-- END friends -->
				</ul>
			
			</td>
		</tr>
	</table>

<style>
.max100 {max-width:100px; max-height:100px;width:expression(width>100?"100px":width+"px");height:expression(height>100?"100px":height+"px");}
</style>