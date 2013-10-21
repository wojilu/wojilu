
<style>
.tabList li{ width:130px;}
</style>

<div>
	
    <div style="font-weight:bold; font-size:14px; margin:20px 10px 15px 20px;"><img src="~img/app/s/menus.png" /> _{menuAdmin}</div>        
	
    <div style="margin:20px 20px 0px 20px; width:95%;">    
        <ul class="tabList">
            <li class="firstTab">
                <a href="#{listLink}" class="frmLink" loadTo="tabMain" nolayout=4>_{menuList}</a>
                <span></span>
            </li>

            <li id="addMenu">
                <a href="#{addLink}" class="frmLink" loadTo="tabMain" nolayout=4><img src="~img/addcategory.gif" /> _{addMenu}</a>
                <span></span>
            </li>            
        </ul>
    </div>
	
	<div class="tabMain" id="tabMain" style="width:96%;padding-top:1px; padding-bottom:10px;">#{layout_content}</div>
</div>

<script>
    _run(function () {
        wojilu.tool.makeTab('.tabList', 'currentTab', '');
        if (window.location.href.toLowerCase().indexOf('addmenu') > 0) {
            $('#addMenu').addClass('currentTab');
        }
    });
</script>


