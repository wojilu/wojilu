
<div style="">

<style>
.tabList {margin-left:30px; margin-top:20px;width:96%;}
.tabList li {width:130px;}
</style>
	
	<ul class="tabList">
	    <li class="firstTab currentTab"><a href="#{groupAdminHome}" class="frmLink" loadTo="tabMain" nolayout=3>_{adminGroup}</a><span></span></li>
	    <li><a href="#{postLink}" class="frmLink" loadTo="tabMain" nolayout=3>_{allPosts}</a><span></span></li>
	    <li><a href="#{groupLink}" class="frmLink" loadTo="tabMain" nolayout=3>_{allGroup}</a><span></span></li>
	    <li><a href="#{groupCategoryLink}" class="frmLink" loadTo="tabMain" nolayout=3>_{groupCategory}</a><span></span></li>
	    <div style="clear:both;"></div>	
	</ul>
	
	<div class="tabMain" id="tabMain" style="width:98%; ">
	#{layout_content}
	</div>

</div>

