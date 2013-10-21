<div>

    <style>
    .tblSetting { width:700px; margin:10px auto;}
    .tblSetting td { border-bottom:1px #eee solid; padding:4px 5px;}

    .lblLeft { width:150px; text-align:right;}
    </style>

    <form method="post" action="#{ActionLink}" class="ajaxPostForm" style="width:700px; margin:auto;">

        <table class="tblSetting" cellpadding="0" cellspacing="0">
        
            <tr style="font-weight:bold; background:#f2f2f2;">
                <td class="lblLeft">配置名称</td>
                <td>值</td>
            </tr>

            <tr>            
                <td class="lblLeft">头条图片：幻灯数量</td>
                <td><input type="text" name="x.PickImgCount" value="#{x.PickImgCount}" style="width:50px;" /></td>
            </tr>

            <tr>            
                <td class="lblLeft">头条要闻：列表数</td>
                <td><input type="text" name="x.PickDataCount" value="#{x.PickDataCount}" style="width:50px;" />
                <span class="note">(头条下面的列表数)</span>
                </td>
            </tr>

            <tr><td></td><td>&nbsp;</td></tr>

            <tr>            
                <td class="lblLeft">
                博客聚合：首页 Title</td>
                <td><input type="text" name="x.MetaTitle" value="#{x.MetaTitle}" style="width:400px;" /></td>
            </tr>

            <tr>            
                <td class="lblLeft">
                博客聚合：首页 Keywords</td>
                <td><input type="text" name="x.MetaKeywords" value="#{x.MetaKeywords}" style="width:400px;" />
                <span class="note">(用于SEO)</span>
                </td>
            </tr>
            <tr>            
                <td class="lblLeft" style=" vertical-align:top;">
                博客聚合：首页 Description</td>
                <td><textarea name="x.MetaDescription" style="width:400px; height:50px;">#{x.MetaDescription}</textarea>
                <span class="note">(用于SEO)</span></td>
            </tr>

            <tr><td></td><td>&nbsp;</td></tr>


            <!--博客之星-->
            <tr>            
                <td class="lblLeft">博客之星：栏目名称</td>
                <td><input type="text" name="x.BlogStarColumnName" value="#{x.BlogStarColumnName}" style="width:200px;" /></td>
            </tr>
            <tr>            
                <td class="lblLeft">博客之星：用户ID</td>
                <td><input type="text" name="x.BlogStarUserId" value="#{x.BlogStarUserId}" style="width:200px;" />
                <span class="note">(请填写数字)</span>
                </td>
            </tr>
            <tr>            
                <td class="lblLeft">博客之星：用户头衔</td>
                <td><input type="text" name="x.BlogStarUserTitle" value="#{x.BlogStarUserTitle}" style="width:200px;" /></td>
            </tr>
            <tr>            
                <td class="lblLeft" style=" vertical-align:top;">博客之星：用户简介</td>
                <td>
                <textarea name="x.BlogStarUserDescription" style="width:400px; height:50px;">#{x.BlogStarUserDescription}</textarea>
                </td>
            </tr>
            <!--博客之星-->




        </table>
    
        <div style="padding:5px 0px 30px 165px; ">
            <input id="Submit1" class="btn" type="submit" value="_{submit}" />
        </div>

    </form>

</div>
		


