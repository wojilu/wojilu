<table class="postList" border="0" cellpadding="0" cellspacing="0">	
	<tr class="psHeader">

            <td style="width:180px; text-align:center;">图片</td><td>帖子标题</td><td>创建时间</td></tr>

            <!-- BEGIN list -->
            <tr class="dataRow">
                <td style="padding:10px;"><a href="#{f.Url}"><img src="#{f.ImgUrl}" class="max150" /></a></td>
                <td><a href="#{f.Url}" target="_blank">#{f.Title}</a></td>
                <td>#{f.Created}</td>
            </tr>
            <!-- END list -->
		</table>

<div class="pageWrap">#{page}</div>        
        <style>
.max150 { max-width:150px; max-height:150px;}
</style>