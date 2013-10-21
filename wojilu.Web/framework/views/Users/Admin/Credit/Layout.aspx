<style>
.tabList {border-bottom: 1px solid #B8D5FF;}
.tabList li{ width:130px;}
</style>

<div>
	
    <div style="font-weight:bold; font-size:14px; margin:20px 10px 15px 25px;"><img src="~img/s/credit.png" /> _{myIncome}</div>        
	
    <div style="margin:20px 20px 0px 20px; width:95%;">    


        <ul class="tabList">
        <li class="firstTab"><a href="#{keyIncomeLink}" class="frmLink" loadTo="tabMain" nolayout=4>_{myIncome}</a><span></span></li>
        <li><a href="#{incomeLink}" class="frmLink" loadTo="tabMain" nolayout=4>积分明细</a><span></span></li>
        <li><a href="#{ruleLink}" class="frmLink" loadTo="tabMain" nolayout=4>_{incomeRule}</a><span></span></li>
        <li><a href="#{rankLink}" class="frmLink" loadTo="tabMain" nolayout=4>_{userRank}</a><span></span></li>
        
        <div class="clear"></div>
        </ul>

    </div>
</div>

<script>
_run( function() {
    wojilu.tool.makeTab( '.tabList', 'currentTab', '' );
});
</script>
	
<div class="tabMain" style="width:96%">
<div id="tabMain" style="padding:10px;">#{layout_content}</div>

</div>
