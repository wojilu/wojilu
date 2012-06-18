#IE6 fixes for Twitter BootStrap v2

This work is based on the initial work of Jonathon Joyce (http://www.jonathonjoyce.co.uk/2012/02/02/twitter-bootstrap2-ie-compatibility-fixes/)

Make sure when using this to use conditional IE statements in your HTML (just BEFORE the &lt;/head&gt; tag), eg:

&lt;!--[if IE 6]&gt;   
 
	<link href="ie6.min.css" rel="stylesheet">
	
&lt;![endif]--&gt;


Again, use a browser conditional statement to include the ie6.min.js if IE or add the following after all the JS on the page, ideally just before the &lt;/body&gt; tag:

&lt;script type="text/javascript"&gt;
$(function(){if($.browser.msie&&parseInt($.browser.version,10)===6){$('.row div[class^="span"]:last-child').addClass("last-child");$('[class*="span"]').addClass("margin-left-20");$(':button[class="btn"], :reset[class="btn"], :submit[class="btn"], input[type="button"]').addClass("button-reset");$(":checkbox").addClass("input-checkbox");$('[class^="icon-"], [class*=" icon-"]').addClass("icon-sprite");$(".pagination li:first-child a").addClass("pagination-first-child")}})
&lt;/script&gt;


There is a sample page - sample-typeahead.html

Sharry @ Empowering Communities

Copyright Twitter 2011
http://twitter.github.com/bootstrap/
