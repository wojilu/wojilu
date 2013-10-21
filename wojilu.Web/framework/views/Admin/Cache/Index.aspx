
<div style="padding:10px 13px;">

    <div style="float:left;">共有缓存 #{cacheCount} 项，这里最多只显示500项。
    <a href="#{addLink}" class="frmBox left10" title="添加缓存"><img src="~img/add.gif" /> 添加缓存</a>
    <a href="#{veiwLink}" class="frmBox left10" title="查看某项"><img src="~img/s/doc.png" /> 查看某项</a>
    <a href="#{deleteKeyLink}" class="frmBox left10" title="删除某项"><img src="~img/s/delete.png" /> 删除某项</a>
    </div>


    <div style="float:right;">
        <span href="#{clearLink}" class="deleteCmd btnOther font12 left20" style="padding-bottom:3px;"><img src="~img/delete.gif" /> 清除所有缓存</span>
    </div>
    
    <div class="clear"></div>

</div>
<div style="padding:0px 10px 10px 10px;">
<table border="1" style="width:100%; border-collapse:collapse; " cellpadding="3">
<tr style="background:#666; color:#fff;"><td style="width:60px; text-align:center">操作</td><td>key</td></tr>

<!-- BEGIN list -->
<tr><td style="text-align:center"><a href="#{removeLink}" class="deleteCmd">清除本项</a></td><td><a href="#{viewLink}" class="frmBox">#{key}</a></td></tr>
<!-- END list -->

</table>
</div>