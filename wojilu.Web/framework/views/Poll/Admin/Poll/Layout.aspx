	
<div style="font-weight:bold; font-size:14px; margin:20px 10px 15px 20px;"><img src="~img/poll.gif" /> 投票管理</div>        
	
    <div style="margin:20px 20px 0px 20px; width:95%;">    

    <style>
    .pollOptions td {padding:2px 10px; }
    .tabList li { width:120px;}
    </style>



    <ul class="tabList">
        <li class="firstTab"><a href="#{friendsPollLink}" class="frmLink" loadTo="pollMain" nolayout=4><img src="~img/list.gif" /> _{friendPoll}</a></a><span></span></li>
        <li><a href="#{myPollLink}" class="frmLink" loadTo="pollMain" nolayout=4><img src="~img/list.gif" /> _{myPoll}</a></a><span></span></li>
        <li><a href="#{addPollLink}" class="frmLink" loadTo="pollMain" nolayout=4><img src="~img/add.gif" /> _{addPoll}</a></a><span></span></li>
    </ul>    

    <script>
        _run(function () {
            wojilu.tool.makeTab('.tabList', 'currentTab', '');
        });
    </script>

    <div style="margin:20px;" id="pollMain">#{layout_content}</div>
    


</div>
