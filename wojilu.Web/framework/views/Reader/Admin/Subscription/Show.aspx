<div class="feedEntriesContainer">
    <div  class="feedTitleContainer">
		<h3>#{feed.Title}</h3>
		<div class="font12">
        <div>:{updated}: #{feed.LastRefreshTime}</div>
        <div>_{url}: <a href="#{feed.Link}" target="_blank">#{feed.Link}</a> </div>
        <div>:{rssUrl}: <a href="#{feed.RssLink}" target="_blank">#{feed.RssLink}</a> </div>
        </div>
    </div>
        
    <!-- BEGIN item -->
    <div class="feedEntry">
        <div class="feedEntryTitle">
            <div class="feedEntryTitleText"><a href="#{item.Link}"><strong>#{item.Title}</strong></a></div>
            <div class="feedEntryTitleDate"><span class="note font12">#{item.PubDate}</span></div>
            <div class="clear"></div>
        </div>
        <div class="feedEntrySummary">#{item.Description}</div>        
    </div>
    <!-- END item -->
    <div class="pager">#{page}</div>
</div>