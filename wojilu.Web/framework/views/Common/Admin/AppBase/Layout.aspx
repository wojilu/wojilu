<style>
.tabList li{ width:130px;}
</style>

<div>
	
    <div style="font-weight:bold; font-size:14px; margin:20px 10px 15px 20px;"><img src="~img/app/s/settings.png" /> _{appAdmin}</div>        
	
    <div style="margin:20px 20px 0px 20px; width:95%;">    
        <ul class="tabList clearfix">
            <li class="firstTab">
                <a href="#{appListLink}" class="frmLink" loadTo="tabMain" nolayout=4>_{myApp}</a>
                <span></span>
            </li>

            <li>
                <a href="#{addAppLink}?linkTarget=blank" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/addcategory.gif" /> _{addApp}</a>
                <span></span>
            </li>            
        </ul>
    </div>
    <div class="tabMain" id="tabMain" style="width:96%;padding-top:1px;"><div>#{layout_content}</div></div>
</div>

<script>
_run( function() {
    wojilu.tool.makeTab( '.tabList', 'currentTab', '' );
});
</script>
