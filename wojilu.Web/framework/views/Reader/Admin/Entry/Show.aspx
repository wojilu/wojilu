
<div class="feedEntryDetailTitle">
    <h4><a href="#{item.Link}" target="_blank">#{item.Title}</a></h4>
    <div class="font12">
        _{author}:<a href="#{item.FeedLink}"><strong>#{item.FeedAuthor}</strong></a>
        <span class="left10 right10">_{hits}:#{item.Hits}</span>
        :{updated}:#{item.PubDate}
    </div>
</div>

<div class="feedEntryDetailContent">#{item.Description}</div>