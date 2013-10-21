<div style="width:700px; height:300px;">
<div style=" margin:10px;">

    <table class="tabHeader" style="width:96%;" cellpadding="0" cellspacing="0">
	    <td class="otherTab"><a href="#{bgLink}">_{pageBg}</a></td>
	    <td class="otherTab"><a href="#{headerLink}">_{pageHeader}</a></td>
	    <td class="otherTab"><a href="#{navLink}">_{adminNav}</a></td>	    
	    <td class="otherTab"><a href="#{mainLink}">_{pageBody}</a></td>
	    <td class="otherTab"><a href="#{footerLink}">_{pageFooter}</a></td>
	    <td class="otherTab" style="width:10%">&nbsp;</td>
    </table>

    <div class="tabMain" style="width:96%; ">
    <div style="padding:10px;">
    
        <table style="width:100%;">
            <tr>
                <td style="width:60px; "><b>_{bgColor}</b></td>
                <td style="vertical-align:top;">
                    <div style="float:left;">
                    <img src="~js/lib/colorpicker/images/select2.png" class="colorSelector" eleSelector="#site-header-inner" cssKey="background"/></div>
                    <div style="float:left">
                    <span class="left20 strong">文字颜色</span>
                    <img src="~js/lib/colorpicker/images/select2.png" class="colorSelector" cssKey="color" eleSelector="#site-header-inner a" />
                    </div>

                    
                    <div style="float:left; font-weight:bold; padding-top:5px;margin-left:50px;margin-right:5px;">页头高度</div>
                    <div class="cmdHeight" id="cmdUp" eleSelector="#site-header-inner"><img src="~img/up.gif" /></div>
                    <div class="cmdHeight" id="cmdDown" eleSelector="#site-header-inner"><img src="~img/dn.gif" /></div>
                    <div id="lblHeight" class="note">请点击箭头调整高度</div>           
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height:10px;"></td>
            </tr>
            <tr>
                <td style="vertical-align:top;"><b>_{bgPic}</b></td>
                <td style="vertical-align:top;">
                    <table class="tabHeader" style="width:99%;margin:0px;" cellpadding="0" cellspacing="0">
	                    <td class="currentTab"><a href="#{viewer.ProfileUrl}">_{selectPic}</a></td>
	                    <td class="otherTab"><a href="#{uploadLink}" class="frmBox" title="_{uploadPic}" xwidth="530" xheight="100"><img src="~img/s/upload.png" /> _{uploadPic}</a></td>
	                    <td class="otherTab"><a href="#{picUrlLink}" class="frmBox" title="_{fillPicUrl}" xwidth="530" xheight="100">_{fillPicUrl}</a></td>
	                    <td class="otherTab" style="width:20%">&nbsp;</td>
                    </table>
                    <div class="tabMain" style="width:99%; margin:0px;">
                        <div id="frmLoader" class="frmUpdate" href="#{myPicsLink}" loadTo="frmMyPics"></div>
                        <div id="frmMyPics" style="display:none;"></div>
                    </div>
                </td>
            </tr>
            
            <tr>
                <td><strong>图片排列</strong></td><td>
                    <select name="background-repeat" id="background-repeat"> 
                    <option value="repeat-x" >横向</option> 
                    <option value="repeat-y" >纵向</option>
                    <option value="repeat" >纵横</option> 
                    <option value="no-repeat" >不平铺</option> 
                    </select>
                    
                    <select name="background-position" id="background-position" disabled="disabled">
                    <option value="" selected>默认</option>
                    <option value="top center" >上部中间</option> 
                    <option value="top left" >上部左侧</option> 
                    <option value="top right" >上部右侧</option> 
                    <option value="center left" >中间左侧</option> 
                    <option value="center center" >正中间</option> 
                    <option value="center right" >中间右侧</option> 
                    <option value="bottom left" >底部左侧</option> 
                    <option value="bottom center" >底部中间</option> 
                    <option value="bottom right" >底部右侧</option>
                    </select>
                
                <label class="left20"><input id="chkDeleteBgPic" type="checkbox" /> 不使用背景图</label>
                </td>
            </tr>

        </table>

    </div>
    </div>
</div>
</div>

<script>
var eleTag = '#site-header-inner';
var saveUrl = '#{saveUrl}';
</script>
#{autoSaveScript}
