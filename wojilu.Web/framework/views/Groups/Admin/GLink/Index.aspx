

<style>
.formPanel h2 {font-size:18px;font-weight:bold;margin:20px 0px;}
.addMenu {margin-left:10px;}
.mcmd {margin-left:2px;}

#linkTitle {font-size:23px; font-weight:bold; margin-bottom:5px;}
#linkMain {margin:20px 10px;}
#linkMain h2 { font-size:14px; font-weight:bold; margin:20px 10px 10px 0px;}

</style>

<script>
_run(function () {
	$('.mcmd').text( '_{addToGroupMenu}' );
});

</script>

<div style="padding:20px;">

    <div id="linkTitle"><img src="~img/app/m/link.png" /> _{adminSiteUrl}</div>
    <div class="note left10">_{addMenuUrlTip}</div>

    <div id="linkMain">


    		
		    <h2>_{adminSiteUrl}</h2>
		    <ul>
			    <li>_{recentGroupMember}: <a href="#{recentMember}">#{recentMember}</a> <a href="#{addMenu}?url=#{recentMember}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
			    <li>_{linkGroup}: <a href="#{friends}">#{friends}</a> <a href="#{addMenu}?url=#{friends}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li>
		    </ul>
    		
		    <h2>_{appUrl}</h2>
		    <ul>
			    <!-- BEGIN list -->
			    <li>#{app.Name}: <a href="#{app.Link}">#{app.Link}</a> <a href="#{addMenu}?url=#{app.Link}" class="addMenu"><img src="~img/add.gif"/><span class="mcmd"></span></a></li><!-- END list -->
		    </ul>
		
		

	</div>
</div>