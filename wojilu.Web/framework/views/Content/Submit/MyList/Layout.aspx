<style>
#dataAdminList {margin-top:0px;font-size:12px;  border-collapse:collapse;}
.adminBar td{padding:0px 0px 0px 5px; border-bottom:1px #6699cc solid; background:url("~img/admin/gridtoolbar-back.gif");  height:26px; }
.cmd,.ajaxCmd,.btnCmd {color:#15428b;cursor:pointer;text-decoration:; margin-right:5px; font-size:12px;}
tr.tableHeader { background: #f0f0f0 url("~img/admin/grid-hrow.gif"); color:#000;}								
.tableHeader td{border-bottom:0px #999999 solid;padding:0px 5px 0px 5px; height:26px;}

td.adminPage {background:#efefef;}

.tabList li {width:150px;}
.tabBody { padding-left:10px;}

</style>

<div>
	<div></div>



<ul class="tabList" style="margin-left:10px; margin:10px;">
    <li class="firstTab"><a href="#{currentPostLink}"><i class="icon-file"></i> 审核中的投递</a><span></span></li>
    <li><a href="#{approvedPostLink}"><i class="icon-ok"></i> 已成功的投递</a><span></span></li>
</ul>    


	
	<div class="tabBody" style="width:96%">#{layout_content}
	</div>
</div>


<script>
_run( function() {
    wojilu.tool.makeTab('.tabList', 'currentTab', '');
});
</script>
