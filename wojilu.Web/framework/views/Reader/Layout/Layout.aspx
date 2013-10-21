
<script type="text/javascript">
<!--
_run(function() {
	wojilu.ui.ajaxUpdate();
});
//-->
</script>

<style>
#feedSidebar li{margin:3px 0px 6px 0px; list-style:none;}
#feedSidebar li.feedItemCat {font-size:14px; margin-top:10px;}


.feedEntriesContainer {margin:0px 0px 30px 0px;line-height:150%;}
.feedTitleContainer {margin:0px 10px 10px 5px; color:#666;}
.feedTitleContainer h4 {color:#222;}

#feedMain {font-size:14px;}
#feedMain p {margin:0px;padding:0px;}

.feedEntry {margin:5px 5px 10px 5px;}
    .feedEntryTitle {background:#f2f2f2; padding:5px; border-bottom:0px #dedede solid;}
        .feedEntryTitleText { float:left; }
        .feedEntryTitleDate { float:right; }
    .feedEntrySummary {padding:5px 10px 10px 20px;}

.feedEntryDetailTitle {margin:10px 0px 15px 0px; padding:10px 0px 8px 15px; color:#666;}
.feedEntryDetailTitle h4 {font-weight:bold; margin:3px 0px 6px 0px; font-size:18px;}
.feedEntryDetailContent {margin:0px 10px 20px 15px;}
</style>

<table cellpadding="0" cellspacing="0" class="adminContainer" style="width:100%;margin:0px auto;">
	<tr>
		<td class="adminSidebar" style="width:160px; vertical-align:top;padding:20px 0px 50px 20px;background:#f2f2f2;border-right:0px #ccc solid;">
			
			<ul id="feedSidebar">
			
				<!-- BEGIN admin -->
				<li><a href="#{addSubscription}"><img src="~img/feed.gif"/> :{subscriptionAdmin}</a></li>
				<li><a href="#{categoryAdmin}"><img src="~img/category.gif"/> :{categoryAdmin}</a></li>
				<!-- END admin -->
				
				<li class="feedItemCat" style="margin-top:20px;"><a href="#{rssHome}"><strong>:{allSubscription}</strong></a></li>
                <!-- BEGIN categoryList -->
				<li class="feedItemCat"><a href="#{category.Url}"><strong>#{category.Title}</strong></a></li>
				<!-- BEGIN list -->	
				<li style="margin:8px 0px 5px 5px;"><a href="#{feed.Url}">#{feed.Title}</a></li>
				<!-- END list -->
                <!-- END categoryList -->	
				
			</ul>
			
			

		</td>
		<td id="feedMain" style="background:#fff;vertical-align:top;padding:0px 10px 10px 10px;">
		#{layout_content}</td>
	</tr>
</table>


