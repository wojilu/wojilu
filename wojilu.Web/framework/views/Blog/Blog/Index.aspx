
	<div class="blogTop">

	<!-- BEGIN topblog -->
	<div style="padding:5px 10px;border-radius:5px;margin:10px 0px 15px 0px;" class="cbg">
		<div class="blog-item-title-wrap">
			<div class="blog-item-title"><img src="~img/sticky.gif"/> _{sticky}  <a href="#{blogpost.Url}" class="left10 strong font14">#{blogpost.Title}</a></div>
			<div class="blogItemTime blog-item-info left10">
                    <span class="blog-item-time-wrap">
                        <span class="blog-item-day">#{blogpost.CreateDate}</span>
                        <span class="blog-item-time">#{blogpost.CreateTime}</span>
                        by <a href="#{authroUrl}">#{author}</a>
                    </span>
                    <span class="blog-item-hits">_{view}(#{blogpost.Hits}) </span>
                    <span class="blog-item-comments">#{blogpost.ReplyCount}</span>
                    <span class="left10 right10">所在分类：<a href="#{blogpost.CategoryLink}">#{blogpost.CategoryName}</a></span>
                    #{blogpost.TagList}
            </div>
            <div style="clear:both;"></div>
		</div>
		<div class="blogItemBody" style="padding:5px 0px 10px 10px">#{blogpost.Body}</div>
		<div class="blogItemMore hide"><a href="#{blogpost.Url}">_{more}...</a></div>

	</div>
	<!-- END topblog -->

	</div>
	
	<!-- BEGIN bloglist -->
	<div class="blogItem">
	
		<div class="blogItemTitle blog-item-title-wrap">
			<div class="blog-item-title">#{blogpost.Status}<a href="#{blogpost.Url}">#{blogpost.Title}</a></div>
			<div class="blogItemTime blog-item-info">
                    <span class="blog-item-time-wrap">
                        <span class="blog-item-day">#{blogpost.CreateDate}</span>
                        <span class="blog-item-time">#{blogpost.CreateTime}</span>
                        by <a href="#{authroUrl}">#{author}</a>
                    </span>
                    <span class="blog-item-hits">_{view}(#{blogpost.Hits}) </span>
                    <span class="blog-item-comments">#{blogpost.ReplyCount}</span>
            </div>
            <div style="clear:both;"></div>
		</div>
		<div class="blogItemBody">#{blogpost.Body}</div>
        <div class="blog-item-category" style="padding:5px 5px; color:#666;">
            <span class="right10">所在分类：<a href="#{blogpost.CategoryLink}">#{blogpost.CategoryName}</a></span>
            #{blogpost.TagList}
        </div>
		<div class="blogItemMore"><a href="#{blogpost.Url}">_{more}...</a></div>
		<div class="blogItemInfo"> posted by <a href="#{authroUrl}">#{author}</a> at #{blogpost.CreateTime} 
		</div>
		
	</div>
	<!-- END bloglist -->
	
	<div>#{pager}</div>
