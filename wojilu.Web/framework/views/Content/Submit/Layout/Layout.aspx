<style>
table.tabHeader {margin-top:10px; margin-left:10px; }
table.tabHeader td{padding:4px 10px; text-align:center; }
	td.currentTab {background:#fff;border-left:1px #6699cc solid;border-top:1px #6699cc solid;border-right:1px #6699cc solid;border-bottom:0px; font-weight:bold;}
	td.otherTab {border-bottom:1px #6699cc solid;background:url("~img/tabbg.gif"); }

div.tabMain {background:#fff; border:1px #6699cc solid; border-top:0px;margin-left:10px; margin-bottom:20px;}

.submitSiderbar ul {padding:20px 10px;}
.submitSiderbar li { padding:10px 0px 10px 5px; }
.submitSiderbar li a { display:block;border2:1px red solid;margin:0px;padding:0px; }
.submitSiderbar li a:hover { text-decoration:none;}
.submitSiderbar li:hover {background:#fefefe;}
</style>

<table style="width:98%; margin:0px auto; margin-left:10px; " border="0">
    <tr>
        <td style="width:150px; background:#eee; vertical-align:top;" class="submitSiderbar">
        <ul class="unstyled">
            <li><a href="#{submitLink}"><img src="~img/s/upload.png" /> <strong>我要投递</strong></a></li>

            <li><a href="#{mySubmitLink}"><img src="~img/list.gif" /> 我的投递<span class="note">(#{submitCount}/#{approvedCount})</span></a></li>
            <li><a href="#{rankLink}"><img src="~img/star.gif" /> 当前等级</a></li>
           
            
            
            <!-- BEGIN admin -->
            <li>
            <div><a href="#{adminLink}"><img src="~img/setting.gif" /> 投递管理</a></div>
            <div style="margin-left:3px;"><span class="note">(#{postCount} 篇待审核)</span></div>
            
            </li><!-- END admin -->
        </ul>
        </td>
        <td style="vertical-align:top;">#{layout_content}</td>
    </tr>

</table>

<script>
    _run(function () {
        require(['wojilu._admin']);
    });
</script>