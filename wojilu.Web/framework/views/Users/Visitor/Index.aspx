<div style="margin:0px;padding:0px;">

<style>

.userList { margin:5px 5px 5px 25px; }
.userList li { float:left; width:65px;height:80px; margin:5px 10px 5px 5px;}
.userList li div {
    width:65px;
     white-space:nowrap;
     text-overflow:clip;
     -moz-text-overflow:clip;
     overflow: hidden;
     text-align:center;
}
</style>


<div style="padding:20px; font-size:20px; font-weight:bold;">_{recentVisitors}</div>
<ul class="userList">
	<!-- BEGIN list -->
	<li>
	    <div><a href="#{user.Link}"><img src="#{user.Face}" class="avs"/></a></div>
	    <div><a href="#{user.Link}">#{user.Name}</a></div>
	</li>
	<!-- END list -->		
</ul>
<div style="margin:0px;padding:10px 10px 20px 20px;clear:both;">#{page}</div>

</div>