<div class="formPanel" style="margin-left:10px;">

<style>

.blue { color:Blue;}
.noteList {margin:10px 0px 10px 30px;}
.noteList li {margin:5px 0px; list-style-type:decimal; }

</style>

	<div class="" style="padding:20px;">
	
	    <div style="font-size:14px; font-weight:bold;">你可以填写4种屏蔽规则：</div>
	    <ul class="noteList">
	        <li>直接填写ip地址，此ip将被屏蔽，比如：<span class="blue">23.182.33.86</span></li>
	        <li>使用通配符&quot;*&quot;，相关ip将被屏蔽，比如：<span class="blue">95.27.88.*</span></li>
	        <li>使用横杠&quot;-&quot;指定范围，比如：<span class="blue">192.168.3.2-192.168.3.250</span></li>
	        <li>填写&quot;unknow&quot;以禁止非法的ip地址：<span class="blue right10">unknow</span><span class="note">(如果填写本条，所有不能正常获取的ip将会被屏蔽)</span></li>
	    </ul>

        <div style="margin:20px 0px 10px 0px;">
            <span style="font-size:14px; font-weight:bold;">请填写需要屏蔽的ip地址</span>
            <span class="note font12 left5">(注意：<span class="red strong">每行只能填写一条</span>屏蔽规则)</span>
        </div>
	
		<form action="#{ActionLink}" method="post" class="ajaxPostForm" style="margin-left:10px;">
		<table style="width: 100%">
			
			<tr>
				<td style="width:350px;">
				<textarea name="BannedIp" style="width: 480px; height: 250px">#{site.BannedIp}</textarea></td>
			</tr>
			<tr>
			    <td>
			        <div>给被屏蔽访客的警告信息</div>
			        <div>
                        <textarea name="BannedIpInfo" style="width:480px; height:50px;">#{site.BannedIpInfo}</textarea>
                    </div>
			    </td>
			</tr>
			
			<tr>
				<td style="padding-top:10px;"><input name="Submit1" type="submit" value="_{editSetting}" class="btn">&nbsp;</td>
			</tr>
		</table>
		</form>
	
	
</div>
