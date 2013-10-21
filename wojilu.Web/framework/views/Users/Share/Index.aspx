<style type="text/css">
.feedItemBody img {float:left;margin:5px;}
.feedTypes {float:right; margin-right:20px;}
.feedTypes li{float:left; list-style:none; margin-left:5px;}
</style>

<div style="padding:30px 20px;">
<div style="font-size:20px; font-weight:bold; width:84%; margin:auto; ">_{myShare}</div>

<table style="width: 85%; margin:auto;  margin-top:5px;">
	<tr>

		<td style="vertical-align:top;padding-right:20px;">

			<!-- BEGIN list -->
			<table style="width: 100%; border-top:0px #fff solid; margin:5px 10px 5px 0px;">
				<tr>
					<td rowspan="2" style="width:60px;vertical-align:top;padding-top:5px;"><a href="#{share.UserLink}"><img src="#{share.UserFace}" /></a></td>
					<td style="height:30px;vertical-align:top"><img src="~img/app/s/#{share.DataType}.gif"/> #{share.Title} <span class="note left10">#{share.Created}</span> 
					<span class="commentCmd link" parentId="0" rootId="#{share.Id}">_{reply}</span>
					</td>
				</tr>
				<tr>
					<td style="vertical-align:top" class="feedItemBody">#{share.Body}#{share.BodyGeneral}	#{commentList}
					</td>
				</tr>
			</table>
			<div style="border-top:1px #ccc solid; margin-left:0px;"></div>
			<!-- END list -->
            #{commentForm}
            <style>#commentForm{ display:none;}</style>

			<div style="margin:10px 20px;">#{page}</div>	
		
		</td>
	</tr>
</table>
</div>