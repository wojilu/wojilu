<div style="padding:20px;">


<style>
.formPanel h2 {font-size:18px;font-weight:bold;margin:20px 0px;}
.addMenu {margin-left:10px;}
.mcmd {margin-left:2px;}

#linkTitle {font-size:23px; font-weight:bold; margin-bottom:5px;}
#linkMain {margin:20px 10px;}
#linkMain h2 { font-size:14px; font-weight:bold; margin:20px 10px 10px 0px;}
</style>



<script type="text/javascript">
<!--
_run( function() {
	$('.mcmd').text( '_{addToSpaceMenu}' );
});
//-->
</script>

<div id="linkTitle"><img src="~img/app/m/link.png" /> _{adminSiteUrl}</div>
<div class="note left10">_{addMenuUrlTip}</div>

<div id="linkMain">
		
		
		
		<h2>_{adminSiteUrl}</h2>
		<ul>
			<li>_{aboutMe}: <a href="#{aboutMe}" target="_blank">#{aboutMe}</a> <a href="#{addMenu}?url=#{aboutMe}&name=About" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
			<li>_{myMicroblog}: <a href="#{microBlog}" target="_blank">#{microBlog}</a> <a href="#{addMenu}?url=#{microBlog}&name=_{microblog}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
			<li>_{myShare}: <a href="#{shareLink}" target="_blank">#{shareLink}</a> <a href="#{addMenu}?url=#{shareLink}&name=_{share}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
			<li>_{recentVisitors}: <a href="#{recentVisitor}" target="_blank">#{recentVisitor}</a> <a href="#{addMenu}?url=#{recentVisitor}&name=_{visitor}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
			<li>_{myFriends}: <a href="#{friends}" target="_blank">#{friends}</a> <a href="#{addMenu}?url=#{friends}&name=_{friend}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
			<li>_{myFeedback}: <a href="#{feedback}" target="_blank">#{feedback}</a> <a href="#{addMenu}?url=#{feedback}&name=_{guestbook}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
			<li>我的论坛帖子: <a href="#{forumTopic}" target="_blank">#{forumTopic}</a> <a href="#{addMenu}?url=#{forumTopic}&name=我的论坛主题" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
		</ul>
		
		<h2>_{appUrl}</h2>
		<ul>
			<!-- BEGIN list -->
			<li>#{app.Name}: <a href="#{app.Link}" target="_blank">#{app.Link}</a> <a href="#{addMenu}?url=#{app.Link}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li><!-- END list -->
		</ul>
		
</div>

</div>
