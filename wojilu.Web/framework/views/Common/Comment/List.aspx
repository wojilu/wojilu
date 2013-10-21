<style type="text/css">
#spaceMain {background:#fff;}
.commentList td {padding:3px 5px 2px 5px;}
</style>

<div style="padding:10px;">
	<div class="editPanel">

	<div style="text-align:left; margin:10px 0px; font-size:16px;"><strong>_{allComments}</strong></div>
		
		<table class="commentList" border="1" style="width:100%; font-size:12px; border-collapse:collapse">
			<tr style="background:#efefef;">
				<td>_{author}</td>
				<td>_{content}</td>
				<td>_{created}</td>
			</tr>
			<!-- BEGIN comment -->
			<tr>
				<td>#{c.Author}</td>
				<td><a href="#{c.Url}">#{c.Content}</a></td>
				<td>#{c.Created} </td>
			</tr>
			<!-- END comment -->	
		</table>
		
		<div>#{page}</div>

	</div>
</div>
