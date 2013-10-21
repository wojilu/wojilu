
	
<div class="">

<style>
.tblSetting { width:750px; margin:10px auto;}
.tblSetting td { border-bottom:1px #eee solid; padding:4px 5px;}

.lblLeft { }
</style>

<form method="post" action="#{ActionLink}" class="ajaxPostForm" style="width:700px; margin:auto;">

    <table class="tblSetting" cellpadding="0" cellspacing="0">
        
        <tr style="font-weight:bold; background:#f2f2f2;">
            <td class="lblLeft" style="width:90px; ">区域</td>
            <td class="lblLeft" style="width:120px;">配置名称</td>
            <td>值</td>
        </tr>
        
        <tr>
            <td class="lblLeft" style="width:80px; ">论坛首页</td>
            <td class="lblLeft" style="width:90px;">图片数量</td>
            <td>#{s.HomeImgCount}</td>
        </tr>
        
        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">帖子数量</td>
            <td>#{s.HomeListCount}</td>
        </tr>
        
        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">热帖天数</td>
            <td>#{s.HomeHotDays}</td>
        </tr>

        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">SEO Keywords</td>
            <td><input type="text" name="forumSetting.MetaKeywords" value="#{s.MetaKeywords}" style="width:450px;" /></td>
        </tr>
        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">SEO Description</td>
            <td><textarea name="forumSetting.MetaDescription" style="width:450px; height:50px;">#{s.MetaDescription}</textarea></td>
        </tr>
        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">隐藏头条新帖</td>
            <td><label><input name="forumSetting.IsHideTop" type="checkbox" #{s.IsHideTop} /></label></td>
        </tr>
        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">隐藏统计信息</td>
            <td><label><input name="forumSetting.HideShowStats" type="checkbox" #{s.HideShowStats} /></label></td>
        </tr>

        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">隐藏在线统计</td>
            <td><label><input name="forumSetting.IsHideOnline" type="checkbox" #{s.IsHideOnline} /></label></td>
        </tr>
        <tr>
            <td class="lblLeft">论坛首页</td>
            <td class="lblLeft">隐藏友情链接</td>
            <td><label><input name="forumSetting.IsHideLink" type="checkbox" #{s.IsHideLink} /></label></td>
        </tr>
        
        <tr>
            <td class="lblLeft">主题列表页</td>
            <td class="lblLeft">新帖最近天数</td>
            <td>#{s.NewDays}</td>
        </tr>

        <tr>
            <td class="lblLeft">主题列表页</td>
            <td class="lblLeft">每页数量</td>
            <td>#{s.PageSize}</td>
        </tr>
        
        <tr>
            <td class="lblLeft">主题详细页</td>
            <td class="lblLeft">每页回复数量</td>
            <td>#{s.TopicPageSize}</td>
        </tr>

        <tr>
            <td class="lblLeft">论坛</td>
            <td class="lblLeft">回复间隔时间不能少于</td>
            <td>#{s.ReplyInterval} 秒</td>
        </tr>

    </table>
    
    <div style="padding:10px 0px 10px 150px; ">
        <input id="Submit1" class="btn" type="submit" value="_{submit}" /></div>

</form>

</div>
		


