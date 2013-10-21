
<style type="text/css">
.putCmd {border:1px #ccc outset; padding:5px; background:#efefef;}
.statsPanel {padding:10px;}
.statsPanel ul{	margin:10px;}
.statsPanel li {	margin:20px; list-style:none}
</style>

<div style="padding:20px;">

	<table style="width: 100%">
		<tr>
			<td style="width:35px;"><img src="~img/big/warning.png" /></td>
			<td><h2 class="red">_{recountUserStatsTip}</h2></td>
		</tr>
	</table>
	
	<div>_{currentTime}: #{time}</div>
	
	
	<div class="statsPanel">
	<h4>_{countBlogs}</h4>
	<ul>
		<li><span href="#{blogCountLink}" class="putCmd"><img src="~img/arrowright.gif" />_{recountUserBlogs}</span></li>
		<li><span href="#{blogCommentLink}" class="putCmd"><img src="~img/arrowright.gif" />_{recountUserBlogComments}</span></li>
	</ul>
	
	</div>

</div>