

<div style="padding:10px; width:90%; margin-left:50px;">

<form method="post" action="#{ActionLink}" class="ajaxPostForm">

    <table class="tblSetting" cellpadding="0" cellspacing="0">
        <tr>
            <td class="lblLeft">:{allowComment}</td>
            <td><label><input name="contentSetting.AllowComment" type="checkbox" #{s.AllowComment} /></label></td>
        </tr>
        <tr style="display:none;">
            <td class="lblLeft">:{allowAnonymousComment}</td>
            <td><label><input name="contentSetting.AllowAnonymousComment" type="checkbox" #{s.AllowAnonymousComment} /></label></td>
        </tr>
        <tr>
            <td class="lblLeft">:{enableSubmit}</td>
            <td><label><input name="contentSetting.EnableSubmit" type="checkbox" #{s.EnableSubmit} /></label>
            <a href="#{submitSettingLink}" class="left20 right10"><img src="~img/user.gif" /> 设置投递者</a>
            |
            <a href="#{submitLink}" target="_blank" class="left10"><img src="~img/add.gif" /> 投递网址</a>
            <a href="#{submitAdminLink}" target="_blank" class="left10"><img src="~img/s/settings.png" /> 投递管理网址</a>
            </td>
        </tr>
        <tr>
            <td class="lblLeft">:{listPostPerPage}</td>
            <td>#{s.ListPostPerPage}</td>
        </tr>
        <tr>
            <td class="lblLeft">:{listPicPerPage}</td>
            <td>#{s.ListPicPerPage}</td>
        </tr>
        <tr>
            <td class="lblLeft">:{listVideoPerPage}</td>
            <td>#{s.ListVideoPerPage}</td>
        </tr>
        
        <tr>
            <td class="lblLeft">:{rankPosts}</td>
            <td>#{s.RankPosts}</td>
        </tr>
        <tr>
            <td class="lblLeft">:{rankPics}</td>
            <td>#{s.RankPics}</td>
        </tr>

        <tr>
            <td class="lblLeft">:{rankVideos}</td>
            <td>#{s.RankVideos}</td>
        </tr>
         <tr>
            <td class="lblLeft">文章列表模式</td>
            <td>#{contentSetting.ArticleListMode}</td>
        </tr>
        <tr style=" display:none;">
            <td class="lblLeft">列表中摘要字数</td>
            <td>#{s.SummaryLength}</td>
        </tr>
        
        <tr>
            <td class="lblLeft">SEO Keywords</td>
            <td><input type="text" name="contentSetting.MetaKeywords" value="#{s.MetaKeywords}" style="width:450px;" /></td>
        </tr>
        <tr>
            <td class="lblLeft">SEO Description</td>
            <td><textarea name="contentSetting.MetaDescription" style="width:450px; height:50px;">#{s.MetaDescription}</textarea></td>
        </tr>

    </table>
    
    <div style="padding:10px 0px 10px 150px;">
        <input id="Submit1" class="btn" type="submit" value="_{submit}" /></div>

</form>

</div>

<style>
.tblSetting { width:90%;}
.tblSetting td { border-bottom:1px #eee solid; padding:4px 5px;}

.lblLeft { text-align:right;width:150px;}
</style>