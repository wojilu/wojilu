<style>
.menu-demo-wrap {padding:30px;margin:20px; background:#f2f2f2; box-shadow:0px 0px 16px #aaa; }
.menu-demo-wrap h3 { font-weight1:normal; margin-bottom:10px; font-size:14px;}
.code-title {margin:15px 0px 10px 0px;}
</style>

<div class="menu-demo-wrap">

    <div class="alert alert-warning">说明：以下菜单是 bootstrap 自带的菜单。所以前提是：需要保证页面引入了 bootstrap.css </div>

    <h3>菜单1：悬停菜单(鼠标移上去出现菜单)</h3>

    <div style="border:1px solid #aaa; padding:10px 10px;">
        <div style=" float:left; margin-right:10px; padding-top:5px;">按钮悬停：</div>

        <div class="dropdown" style="float:left;">                
		    <span class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">
                <button class="btn btn-primary"><i class="icon-plus icon-white"></i> 发布文章</button>
		        <span class="caret" style="margin-top:12px;"></span>
            </span>
		    <ul class="dropdown-menu hide" style="min-width:120px;">
                <li style="margin-top:10px;"><a href="">菜单1</a></li>
                <li class="divider"></li>
			    <li><a href="">菜单2</a></li>
			    <li style="margin-bottom:10px;"><a href="">菜单3</a></li>
		    </ul>
        </div>

        <div style=" float:left; padding-top:5px; margin-left:100px;">链接悬停：</div>

        <div style="float:left;">

            <style>
            .multi-menu li.dropdown { float:left; list-style:none; margin:5px 10px 5px 10px; }
            .multi-menu .dropdown-menu a { padding:6px 10px;}
            </style>
    
            <ul class="multi-menu" style="margin-left:0px;">
                <li class="dropdown">
                    <a href="" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">菜单1</a>
	                <ul class="dropdown-menu">
                        <li><a href="#">子菜单1.1</a></li>
                        <li><a href="#">子菜单1.2</a></li>
                        <li><a href="#">子菜单1.3</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">菜单2</a>
	                <ul class="dropdown-menu">
                        <li><a href="#">子菜单2.1</a></li>
                        <li><a href="#">子菜单2.2</a></li>
                        <li><a href="#">子菜单2.3</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">菜单2</a>
	                <ul class="dropdown-menu">
                        <li><a href="#">子菜单3.1</a></li>
                        <li><a href="#">子菜单3.2</a></li>
                        <li><a href="#">子菜单3.3</a></li>
                    </ul>
                </li>
            </ul>

    
        </div>
        <div style=" clear:both;"></div>

    </div>

    <div style=" clear:both;"></div>

    <div class="code-title strong">按钮悬停代码</div>
    <pre class="brush: html;">
        
        <div class="dropdown">                
		    <span class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">
                <button class="btn btn-primary"><i class="icon-plus icon-white"></i> 发布文章</button>
		        <span class="caret" style="margin-top:12px;"></span>
            </span>
		    <ul class="dropdown-menu hide" style="min-width:120px;">
                <li style="margin-top:10px;"><a href="">菜单1</a></li>
                <li class="divider"></li>
			    <li><a href="">菜单2</a></li>
			    <li style="margin-bottom:10px;"><a href="">菜单3</a></li>
		    </ul>
        </div>
            
    </pre>

    <div class="code-title strong">链接悬停代码</div>
    <pre class="brush: html;">


            <style>
            .multi-menu li.dropdown { float:left; list-style:none; margin:5px 10px 5px 10px; }
            .multi-menu .dropdown-menu a { padding:6px 10px;}
            </style>
    
            <ul class="multi-menu" style="margin-left:0px;">
                <li class="dropdown">
                    <a href="" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">菜单1</a>
	                <ul class="dropdown-menu">
                        <li><a href="#">子菜单1.1</a></li>
                        <li><a href="#">子菜单1.2</a></li>
                        <li><a href="#">子菜单1.3</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">菜单2</a>
	                <ul class="dropdown-menu">
                        <li><a href="#">子菜单2.1</a></li>
                        <li><a href="#">子菜单2.2</a></li>
                        <li><a href="#">子菜单2.3</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown">菜单2</a>
	                <ul class="dropdown-menu">
                        <li><a href="#">子菜单3.1</a></li>
                        <li><a href="#">子菜单3.2</a></li>
                        <li><a href="#">子菜单3.3</a></li>
                    </ul>
                </li>
            </ul>
    </pre>

</div>

<div class="menu-demo-wrap">

    <h3>菜单2：链接点击菜单(需要点击才出现菜单)</h3>

    <div class="dropdown">
	    <a href="#" class="dropdown-toggle" data-toggle="dropdown">
	        Account <b class="caret"></b>
	    </a>
	    <ul class="dropdown-menu">
        <li><a href="#">Action</a></li>
        <li><a href="#">Another action</a></li>
        <li><a href="#">Something else here</a></li>
        <li class="divider"></li>
        <li><a href="#">Separated link</a></li>
	    </ul>
    </div>


    
    <div style="margin-top:10px;">“悬停菜单”和“点击菜单”的区别：“悬停菜单”多了一个 <span style="color:Blue;">data-hover="dropdown"</span> 属性；“点击菜单”没有这个属性。</div>
    <div class="code-title strong">前端代码</div>
    <pre class="brush: html;">
        <div class="dropdown">
	        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
	            Account <b class="caret"></b>
	        </a>
	        <ul class="dropdown-menu">
                <li><a href="#">Action</a></li>
                <li><a href="#">Another action</a></li>
                <li><a href="#">Something else here</a></li>
                <li class="divider"></li>
                <li><a href="#">Separated link</a></li>
	        </ul>
        </div>

    </pre>

</div>



<div class="menu-demo-wrap">

    <h3>菜单3：按钮点击菜单(需要点击才出现菜单)</h3>

    <div class="btn-group">
        <a class="btn btn-primary dropdown-toggle" data-toggle="dropdown" href="#">
	        Action <span class="caret"></span>
        </a>
	
	    <ul class="dropdown-menu">
            <li><a href="#">Action</a></li>
            <li><a href="#">Another action</a></li>
            <li><a href="#">Something else here</a></li>
            <li class="divider"></li>
            <li><a href="#">Separated link</a></li>
        </ul>
    </div>

    <div class="code-title strong">前端代码</div>
    <pre class="brush: html;">
        <div class="btn-group">
            <a class="btn btn-primary dropdown-toggle" data-toggle="dropdown" href="#">
	            Action <span class="caret"></span>
            </a>
	
	        <ul class="dropdown-menu">
                <li><a href="#">Action</a></li>
                <li><a href="#">Another action</a></li>
                <li><a href="#">Something else here</a></li>
                <li class="divider"></li>
                <li><a href="#">Separated link</a></li>
            </ul>
        </div>
    </pre>

</div>









<div style="padding:30px;margin:20px; background:#f2f2f2; display:none; ">

    <div style="position:relative; width:100px; " id="myLink">
        <div style="border:1px #aaa solid; width:100px; height:28px; background:#fff; cursor:pointer;" id=""><div style="padding:5px 10px;">菜单原理1 <img src="~img/down.gif" /></div></div>
        <div id="dropPanel" style=" position:absolute; display:none; top:28px; left:0px; border:1px #aaa solid; width:200px; background:#fff ">
            <div style="border-top:0px; margin-top:-1px; width:100px; height:1px; background:#fff;"></div>
            <div style="padding:10px;">这是一段文字</div>
        </div>
    </div>
    
    <div>这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的
    </div>

</div>

<div style="padding:30px;margin:20px; background:#f2f2f2;  display:none; ">

    <div class="wMenu" list="myMenuList" style="border:1px #aaa solid; width:100px;">
        <div style="padding:5px 10px;">菜单原理2 <img src="~img/down.gif" /></div>
        <ul id="myMenuList" class="menuItems">
        <li>菜单1</li>
        <li>菜单2</li>
        <li>菜单3</li>
        <li>菜单4</li>
        <li>菜单5</li>
        </ul>
    </div>

    <div>这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的文字这里是大量的
    </div>


</div>

<script>
_run( function() {

    // 菜单原理1
    $('#myLink').hover( 
        function() {
            $('#dropPanel').show();
        },
        function() {
            $('#dropPanel').hide();
        }
        
    );

    // 菜单原理2
    $('.wMenu').hover(
        function() {
            var mx = $(this).height();
            $(this).css( 'position', 'relative' );
            $( '#'+ $(this).attr( 'list' ) ).css( 'position', 'absolute' ).css( 'top', mx+'px' ).css( 'left', 0 ).slideDown('fast');
        },
        function() {
            $( '#'+ $(this).attr( 'list' ) ).slideUp();
        }    
    );

    
});

</script>
