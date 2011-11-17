<div style="padding:20px; background:#fff;">

<style>
.formPanel {margin:10px; padding:10px; background:#eee; line-height:200%;}
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}
</style>
    <div></div>

    <div class="formPanel">
    
        <div style="margin-bottom:20px;">
        <div><span class="strong">自定义安全验证cookie(加密的cookie)</span>：</div>
        <p style="margin-top:10px; text-indent:35px;">
        一般处理用户登录问题，我们都使用.net默认的cookie加密验证机制，它<span class="strong" style="color:Blue;">只有一个</span>默认的cookie名称，不能对用户进行多次 cookie 验证（比如前台登录之后，还要进行后台登录这种情况）。因为多次验证需要多个加密cookie。</p>
        <p style="margin-top:10px; text-indent:35px;">对于多次登录验证的情况，最简单的解决办法是：第一个验证仍然使用默认的cookie加密验证，第二个或更多的验证使用session。但session在使用过程中往往有诸多不如意之处，比如在服务器内存紧张的情况下，经常被清空，导致用户验证过期，需要频频重复登录。</p>
        <p style="margin-top:10px; text-indent:35px;">最根本的解决办法，则是：在默认的加密cookie之外，自定义一个加密的cookie。wojilu 1.7 对这个功能进行了封装，只要输入自定义的cookie名称，则自动产生加密的安全cookie，以方便多次登录验证。全套方法和默认的登录验证方法完全相同，只不过相应多了一个参数，比如原先的默认的cookie加密登录方式是 <span style="color:Blue;">ctx.web.UserLogin()/ctx.web.UserId()/ctx.web.UserLogout()</span>，新增加的是 <span style="color:red;">ctx.web.UserLogin(cookieName)/ctx.web.UserId(cookieName)/ctx.web.UserLogout(cookieName)</span></p>
        
        </div>
        <div style="margin-top:10px;margin-bottom:10px;">-----------------以下是demo演示(点击不同命令，下面cookie的值会变化)--------------------</div>
        
        <div style="margin-left:35px;">
            <div><a href="#{addCookieLink}" class="btnBig">添加cookie</a> 添加自定义验证cookie【ctx.web.UserLogin( cookieName, ...】。</div>
            
            <div style="margin:10px 0px 10px 0px;"><a href="#{deleteCookieLink}" class="btnBig btnGreen float">删除cookie</a> 删除自定义验证cookie【
            ctx.web.UserLogout( cookieName )】
            </div>
            
        </div>
        <hr style="margin:20px 0px;" />
        <div style="margin-left:35px;">
            
            <div class="strong">cookie的值：<span style="color:Red; font-size:28px; ">#{userId}</span></div>
            <div>【ctx.web.UserId( cookieName )】</div>
        
        </div>
    
    </div>
    
    
    
    <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
    
    <div>
    <div class="strong">服务端代码</div>
    
    <pre class="brush: c-sharp;">
        private static readonly String cookieName = "__wojilu_demo_cookie_test";

        public void Cookie() {

            set( "addCookieLink", to( CookieAdd ) );
            set( "deleteCookieLink", to( CookieDelete ) );

            // 2) 【获取】根据自定义的cookie名称，获取在客户端加密存储的 userId
            set( "userId", ctx.web.UserId( cookieName ) );

        }

        public void CookieAdd() {

            // 1) 【增加】添加一个名为 cookieName 的自定义加密cookie，同时将cookie的值 888 放入
            ctx.web.UserLogin( cookieName, 888, "zhangsan", wojilu.Common.LoginTime.Never );

            echoRedirect( "操作成功", Cookie );
        }

        public void CookieDelete() {

            // 3) 【删除】删除名为 cookieName 的自定义加密cookie 
            ctx.web.UserLogout( cookieName );

            echoRedirect( "操作成功", Cookie );

        }
    </pre>
    
    </div>
</div>