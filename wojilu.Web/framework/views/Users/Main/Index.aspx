<div class="section-panel">
    <div class="pull-right" style="padding-right:40px;padding-top:10px;"><a href="#{lnkOnlineAll}">&rsaquo;&rsaquo;_{all}</a></div>
    <h3><img src="~img/s/online.png" /> _{whoIsOnline} </h3>
    <div class="hero-unit" style="padding:10px 10px 0px 0px; margin:0px 5px;">
    <table style="width: 100%">
	    <tr>
		    <td id="onlinePanel" style="padding:0px 10px 0px 0px;"><div style="text-align:center;"><img src="~img/ajax/big.gif" /> loading...</div></td>
	    </tr>
    </table>
    </div>
</div>

<script>
    _run( function() {
        $.get('#{onlineMemberLink}'.toAjax(), function (users) {

            var html = '<ul class="thumbnails user-list" style="margin:0px;padding:0px;_margin-left:-20px;_margin-bottom:10px;">';
            for (var i = 0; i < users.length; i++) {
                html += '<li><div class="thumbnail">';
                html += '<a href="' + users[i].UserUrl + '"><img src="' + users[i].UserPicUrl + '" class="avs" /></a><div><a href="">' + users[i].UserName + '</a></div>';
                html += '</div></li>';
            }
            html += '</ul>';

            $('#onlinePanel').html(html);
        });

    });

</script>


<div class="section-panel">

    <div class="pull-right" style="padding-right:40px;padding-top:10px;"><a href="#{lnkRank}">&rsaquo;&rsaquo;_{all}</a></div>
    <h3 style=" margin-bottom:5px;"><img src="~img/s/users24.png" /> _{userCharts}</h3>
    <ul class="thumbnails user-list">
		<!-- BEGIN ranks -->
		<li style="width:62px;">
            <div class="thumbnail">
			    <a href="#{u.Link}"><img src="#{u.Face}"/></a>
                <div><a href="#{u.Link}">#{u.Name}</a></div>
            </div>
		</li>
		<!-- END ranks -->    
    </ul>
</div>


<div class="section-panel">

    <div class="pull-right" style="padding-right:40px;padding-top:10px;"><a href="#{linkAll}">&rsaquo;&rsaquo;_{all}</a></div>
    <h3 style="  margin-bottom:5px;"><img src="~img/s/users242.png" /> _{newRegUser} </h3>
    <ul class="thumbnails user-list">
		<!-- BEGIN users -->
		<li style="width:62px;">
            <div class="thumbnail">
			    <a href="#{u.Link}"><img src="#{u.Face}"/></a>
                <div><a href="#{u.Link}">#{u.Name}</a></div>
            </div>
		</li>
		<!-- END users -->    
    </ul>

</div>


