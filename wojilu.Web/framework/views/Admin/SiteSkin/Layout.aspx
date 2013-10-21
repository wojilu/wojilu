
<style>
#skinCmdList li.separator {margin-top:10px; margin-bottom:-15px;padding:0px;}
.separator div{border-top:1px #ccc solid; border-bottom:1px #fff solid;}
</style>


<div>
	<div></div>
	<table class="tabHeader" style="width:98%; font-size:12px;" cellpadding="0" cellspacing="0">
		<tr>
		    <td class="otherTab" style="width:5%">&nbsp;</td>
			<td class="tabCmd otherTab" style="width:20%"><a href="#{lnkSkin}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/img.gif" /> _{siteSkin}</a></td>
			<td class="tabCmd otherTab" style="width:20%">
		    <span class="menuMore" list="catlink">皮肤素材 <img src="~img/down.gif" /> 		
		    <ul id="catlink" class="menuItems" style="width:120px;">
		    <li><a href="#{lnkResourceTexture}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/arrowRight.gif" />背景纹理</a></li>
		    <li><a href="#{lnkResourceIcon}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/arrowRight.gif" />各类图标</a></li>
		    <li><a href="#{lnkResourcePic}" class="frmLink" loadTo="tabMain" nolayout=3><img src="~img/arrowRight.gif" />主题图片</a></li>
		    </ul></span>	
			</td>
			
			<td class="tabCmd otherTab" style="width:20%">
		    <span class="menuMore" list="tplItems">模板管理 <img src="~img/down.gif" /> 		
		    <ul id="tplItems" class="menuItems" style="width:120px;">
				<li><a href="#{lnkTemplateFile}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/doc.gif"/> 模板修改</a></li>
				<li><a href="#{lnkTemplateSearch}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/doc.gif"/> 模板搜索</a></li>				
				<li><a href="#{lnkCssFile}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/doc.gif"/> css 修改</a></li>				
				<li><a href="#{lnkSkinFile}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/doc.gif"/> 皮肤修改</a></li>

		    </ul></span>	
			
			</td>
				
			<td class="otherTab" style="width:35%">&nbsp;</td>
		</tr>
	</table>
	
	<div class="tabMain" id="tabMain" style="width:98%; padding:20px;">
        <div class="warning">切换皮肤或者自定义皮肤之后，请重启网站，因为网站默认开启了页面缓存，如果不重启则无法生效。</div>
	#{layout_content}
	</div>

</div>

