<div class="summary-wrap">
    <!-- BEGIN list -->
    <div class="summary-item">
    <h3><a href="#{post.Url}" title="#{post.TitleFull}" style="#{post.TitleCss}">#{post.TitleShow}</a></h3>
    <div class="note font12">#{post.AuthorShow} 发布于 #{post.Created} <span class="left10">#{post.HitsShow}</span> <span class="left10">#{post.RepliesShow}</span></div>
    <div style=" " class="clearfix summary-body">
        <a href="#{post.Url}" class="summary-pic">#{post.PicSHtml}</a>
        #{post.SummaryShow} <a href="#{post.Url}" class="left15">[查看全文]</a>
    </div>
    <div class="summary-tag">#{post.TagShow}</div>
    </div>
    <!-- END list -->
    <div class="summary-page" style=" text-align:center;"><a href="#{lnkList}" class="btn"><i class="icon-hand-right"></i> 浏览更多文章</a></div>
</div>