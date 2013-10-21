	<div id="spaceHeaderWrap">
		<div id="spaceHeader">		
			<div id="spaceName"><a href="#{spaceUrl}" class="blogLogoText">#{spaceName}</a></div>
			<div id="spaceUrl"><a href="#{spaceUrl}">#{spaceUrl}</a></div>
		</div>
	</div>
	
	<div id="spaceNavWrap">
		<div id="spaceNav">
			<ul class="spaceNavUL">
			<!-- BEGIN navLink -->
			<li class="rootLi">
			    <!-- BEGIN rootNav --><a href="#{menu.Link}" style="#{menu.Style}" #{menu.LinkTarget}>#{menu.Name}</a><!-- END rootNav -->
			    
			    <!-- BEGIN subNav -->
			        <span class="menuMore" list="subMenus#{menu.Id}"><a href="#{menu.Link}" style="#{menu.Style}" #{menu.LinkTarget}>#{menu.Name}</a>
			        <ul id="subMenus#{menu.Id}" class="menuItems" style="width:120px;margin:0px;padding:10px 0px;">
			        <!-- BEGIN subMenu -->
			            <li style="width:100px;margin:0px;padding:5px 10px;"><a href="#{menu.Link}" style="#{menu.Style}" #{menu.LinkTarget}>#{menu.Name}</a></li>
			        <!-- END subMenu -->
			        </ul></span>
			    <!-- END subNav -->
			</li>
			<!-- END navLink -->
			</ul>
			<div class="clear"></div>
		</div>
	</div>