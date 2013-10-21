<style>
.userAvatar li {margin:5px;}
</style>

<div>

    <div style="float:left; width:600px; background:#fff; border-right:1px #ccc solid; padding-bottom:30px;">
        <div style="margin:10px; padding:5px; font-weight:bold;"><img src="~img/talk.gif" /> 大家正在说</div>
        #{recentList}
        <script>
        _run( function() { 
            setInterval( function() {

                var lastEle = $('#blogList .blogItem:last-child');
                lastEle.hide();
                $('#blogList').prepend( lastEle );
                lastEle.slideDown('slow');
            }, 3000);

        });
        </script>
    
    </div>   
    
    <div style="width:260px; float:right; ">
    
        <div>
        
            <div style="margin:10px; font-weight:bold; border-bottom:1px #ccc solid; padding:5px 10px;"><img src="~img/s/users.png" /> 最受关注排行</div>
         
			<ul class="userAvatar unstyled">
				<!-- BEGIN followers --><li>
				<a href="#{user.Link}"><img src="#{user.Face}" class="avs" /></a>
				<div><a href="#{user.Link}">#{user.Name}</a></div></li>
				<!-- END followers -->		
			</ul>
			<div class="clear"></div>

            
            <div style="margin:10px; font-weight:bold; border-bottom:1px #ccc solid; padding:5px 10px;"><img src="~img/s/users.png" /> 最新注册</div>
        
			<ul class="userAvatar unstyled">
				<!-- BEGIN recent --><li>
				<a href="#{user.Link}"><img src="#{user.Face}" class="avs" /></a>
				<div><a href="#{user.Link}">#{user.Name}</a></div></li>
				<!-- END recent -->		
			</ul>

        </div>
    </div>

</div>
