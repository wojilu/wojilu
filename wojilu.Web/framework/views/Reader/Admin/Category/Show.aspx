<div class="feedEntriesContainer">
        
    <div class="feedTitleContainer">
        <h3>_{category}: #{c.Name}</h3>
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