

    <div class="downloadSidebar">
    <div class="downloadSidebarMain">
        <div >
            <div class="sidebarTitle">下载排行(当前类别)</div>
            
            <ul class="unstyled">
                <!-- BEGIN ctops -->
                <li><div class="cat-list"><a href="#{data.Link}">#{data.Title}</a></div></li>
                <!-- END ctops -->
            </ul>
        
        </div>
    
    
        <div>
            <div class="sidebarTitle">下载排行(所有)</div>
            
            <ul class="unstyled">
                <!-- BEGIN atops -->
                <li><div class="cat-list"><a href="#{data.Link}">#{data.Title}</a></div></li>
                <!-- END atops -->
            </ul>
        
        </div>
    </div>
    </div>
    
    
    
    <div class="downloadMain">
    
        <div class="downloadLocation">当前路径: #{location}</div>
        <div class="downloadCategoryLocation">分类导航：#{categories}</div>

        <div class="fdTitle">#{f.Title}</div>
        <div style="margin:5px 10px 15px 15px;" class="note">浏览:#{f.Hits} <span class="left5">评论:<span id="contentReplies">#{f.Replies}</span></span> <span class="left5">下载次数:#{f.Downloads}</span></div>
        <div>
        
            <table style="width:100%;margin-left:10px; margin-bottom:10px;">
                <tr>
                    <td class="dowloadLeftLabel">
                        文件大小</td>
                    <td>
                        #{f.SizeMB} MB</td>
                </tr>
                <tr>
                    <td class="dowloadLeftLabel">
                        更新日期</td>
                    <td>
                        #{f.Updated}</td>
                </tr>
                <tr>
                    <td class="dowloadLeftLabel">
                        软件语言</td>
                    <td>
                        #{f.Lang}</td>
                </tr>
                <tr>
                    <td class="dowloadLeftLabel">
                        提供商</td>
                    <td>
                        #{f.Provider}</td>
                </tr>
                <tr>
                    <td class="dowloadLeftLabel">
                        软件授权</td>
                    <td>
                        #{f.License}</td>
                </tr>
                <tr>
                    <td class="dowloadLeftLabel">
                        软件评级</td>
                    <td>
                        #{f.RankStar}</td>
                </tr>
                <tr>
                    <td class="dowloadLeftLabel">
                        应用平台</td>
                    <td>
                        #{f.PlatformStr}</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="dowloadLeftLabel">演示网址</td>
                    <td>#{f.DemoLink}</td>
                </tr>
                <tr>
                    <td style="vertical-align:top;" class="dowloadLeftLabel">
                        下载网址</td>
                    <td>#{downloadLink1}<br />
                        #{downloadLink2}<br />
                        #{downloadLink3}</td>
                </tr>
            </table>
            
            <div class="fdLine">简介</div>
            <div style="margin:15px 0px 10px 0px; padding:5px 5px 5px 10px; word-wrap:break-word; overflow:hidden; ">
            #{f.Description}
            #{previewPic}
            </div>
            
            <div><a name="comments"></a></div>
		<div class="frmLoader" url="#{commentUrl}"></div>
		
		<script>
		    _run(function () {
		        wojilu.ctx.changeUrl = false;
		        wojilu.ui.frmLoader();
		    });
		</script>

        
        </div>
        
    </div>
