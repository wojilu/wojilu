<div style="padding:20px; background:#fff;">

<style>
.formPanel {margin:10px; padding:10px; background:#eee; line-height:200%;}
.formPanel table {width:90%;}
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}

#frmLinkTestMain { border-collapse:collapse;}
#frmLinkTestMain td{ vertical-align:top;background:#fff;}
#frmLinkTestMain #sidebarTest {background:#95a5a6;width:100px; padding:10px; vertical-align:top;}
</style>


    <div class="formPanel">
    
    <table style="width:100%; height:350px;" border="1" id="frmLinkTestMain" cellspacing="0">
        <tr>
            <td id="sidebarTest">
                <a href="#{lnkRight}" class="frmLink" loadto="mainRight" nolayout=2 style="color:#fff;">点击此处1</a><br />右侧局部刷新
            </td>            
            <td id="mainRight"></td>
        </tr>    
    </table>
    


    </div>


    <div style="border-bottom:1px #ccc solid;margin:0px 0px 20px 0px;">&nbsp;</div>
    
    <div>
    
    <div style="margin-top:20px; margin-bottom:10px;"><strong>一、局部刷新_简介</strong></div>
    <div>a）可以将普通的链接轻松转换成局部刷新效果，无须增加任何js；</div>
    <div>b）可以无限级局部刷新，无限级嵌套；</div>
    <div>c）可以在局部刷新的页面中继续完整使用现有的富编辑器、验证控件、上传控件、弹窗等等，原有开发模式不受影响</div>
    
    <div style="margin-top:20px;margin-bottom:10px;"><strong>二、局部刷新_优点</strong></div>
    <div>a）减少网络数据传输量，降低带宽压力；</div>
    <div>b）大幅减少服务端数据计算和查询；</div>
    <div>c）速度更快，视觉体验更佳</div>
    
    <div style="margin-top:20px;"><strong>三、使用语法</strong></div>
    <pre class="brush: html;">
    	&lt;a href="someLink" class="frmLink" loadTo="mainPage" nolayout=2&gt;点击此处，局部刷新&lt;/a&gt;
    </pre>
    
    <div style="margin:5px 10px;">
        <div>增加3个属性，即可将普通的链接转换成局部刷新链接：</div>
        <div>1）增加一个名为 frmLink 的 class</div>
        <div>2）链接的内容加载到(load to)页面的哪个局部？请在loadTo属性中，输入此局部的id</div>
        <div>3）设置 nolayout 的值，必须是数字。</div>
        
        <div style="margin-top:10px; font-weight:bold;">【nolayout说明】</div>
        <div>	禁止呈现的layout层级，从顶部开始，1表示禁止呈现链接页面的第一个layout，2表示禁止呈现1和2这两个layout……n表示从1到n的所有layout都禁止呈现，999表示禁止呈现所有layout，0表示呈现全部layout</div>
        
        <div>---------------<br />
            <p style="margin:0in;margin-left:10px;">
                <span style="font-family:Verdana;color:blue">&lt;a href=&quot;</span><span 
                    style="font-family:Verdana;color:blue">a</span><span 
                    style="font-family:Verdana;color:blue">.aspx&quot; class=&quot;</span><span 
                    style="font-weight:bold;font-family:Verdana;color:red">frmLink</span><span 
                    style="font-family:Verdana;color:blue">&quot; </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">loadTo</span><span 
                    style="font-family:Verdana;color:blue">=&quot;mainPage&quot;</span><span 
                    style="font-family:Verdana;color:blue"> </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">
                nolayout</span><span style="font-family:Verdana;color:blue">=0</span><span 
                    style="font-family:Verdana;color:blue">&gt;pageA&lt;/a&gt;</span><span 
                    style="font-family:宋体;color:blue">——包括所有布局</span></p>
            <p style="margin:0in;margin-left:10px;font-size:9.0pt">
                <span style="font-family:Verdana;color:blue">&lt;a href=&quot;</span><span 
                    style="font-family:Verdana;color:blue">a</span><span 
                    style="font-family:Verdana;color:blue">.aspx&quot; class=&quot;</span><span 
                    style="font-weight:bold;font-family:Verdana;color:red">frmLink</span><span 
                    style="font-family:Verdana;color:blue">&quot; </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">loadTo</span><span 
                    style="font-family:Verdana;color:blue">=&quot;mainPage&quot;</span><span 
                    style="font-family:Verdana;color:blue"> </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">
                nolayout</span><span style="font-family:Verdana;color:blue">=1</span><span 
                    style="font-family:Verdana;color:blue">&gt;pageA&lt;/a&gt;</span><span 
                    style="font-family:宋体;color:blue">——剔除顶层布局</span></p>
            <p style="margin:0in;margin-left:10px;font-size:9.0pt">
                <span style="font-family:Verdana;color:blue">&lt;a href=&quot;</span><span 
                    style="font-family:Verdana;color:blue">a</span><span 
                    style="font-family:Verdana;color:blue">.aspx&quot; class=&quot;</span><span 
                    style="font-weight:bold;font-family:Verdana;color:red">frmLink</span><span 
                    style="font-family:Verdana;color:blue">&quot; </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">loadTo</span><span 
                    style="font-family:Verdana;color:blue">=&quot;mainPage&quot;</span><span 
                    style="font-family:Verdana;color:blue"> </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">
                nolayout</span><span style="font-family:Verdana;color:blue">=2</span><span 
                    style="font-family:Verdana;color:blue">&gt;pageA&lt;/a&gt;</span><span 
                    style="font-family:宋体;color:blue">——剔除从顶层开始的</span><span 
                    style="font-family:Verdana;color:blue">1</span><span 
                    style="font-family:宋体;color:blue">级、</span><span style="font-family:
Verdana;color:blue">2</span><span style="font-family:宋体;color:blue">级布局</span></p>
            <p style="margin:0in;margin-left:10px;font-size:9.0pt">
                <span style="font-family:Verdana;color:blue">&lt;a href=&quot;</span><span 
                    style="font-family:Verdana;color:blue">a</span><span 
                    style="font-family:Verdana;color:blue">.aspx&quot; class=&quot;</span><span 
                    style="font-weight:bold;font-family:Verdana;color:red">frmLink</span><span 
                    style="font-family:Verdana;color:blue">&quot; </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">loadTo</span><span 
                    style="font-family:Verdana;color:blue">=&quot;mainPage&quot;</span><span 
                    style="font-family:Verdana;color:blue"> </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">
                nolayout</span><span style="font-family:Verdana;color:blue">=n</span><span 
                    style="font-family:Verdana;color:blue">&gt;pageA&lt;/a&gt;</span><span 
                    style="font-family:宋体;color:blue">——剔除从顶层开始的</span><span 
                    style="font-family:Verdana;color:blue">n</span><span 
                    style="font-family:宋体;color:blue">级布局</span></p>
            <p style="margin:0in;margin-left:10px;font-size:9.0pt">
                <span style="font-family:Verdana;color:blue">&lt;a href=&quot;</span><span 
                    style="font-family:Verdana;color:blue">a</span><span 
                    style="font-family:Verdana;color:blue">.aspx&quot; class=&quot;</span><span 
                    style="font-weight:bold;font-family:Verdana;color:red">frmLink</span><span 
                    style="font-family:Verdana;color:blue">&quot; </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">loadTo</span><span 
                    style="font-family:Verdana;color:blue">=&quot;mainPage&quot;</span><span 
                    style="font-family:Verdana;color:blue"> </span>
                <span style="font-weight:bold;font-family:Verdana;color:red">
                nolayout</span><span style="font-family:Verdana;color:blue">=999</span><span 
                    style="font-family:Verdana;color:blue">&gt;pageA&lt;/a&gt;</span><span 
                    style="font-family:宋体;color:blue">——剔除所有布局</span></p>
                    --------------</div>
                    
            <div style="margin-top:10px;">现在，你可以在本网站任何一个链接的尾部加上“?nolayout=1”(或其他数字)来查看效果</div>
    
    </div>
    
    <div style="margin-top:20px;"><strong>前端示例代码1</strong></div>
    <pre class="brush: html;">

&lt;table style="width:100%; height:350px;" border="1" id="Table1" cellspacing="0"&gt;
    &lt;tr&gt;
        &lt;td id="sidebarTest"&gt;
            &lt;a href="#{lnkRight}" class="frmLink" loadto="mainRight" nolayout=2&gt;点击此处1&lt;/a&gt;&lt;br /&gt;右侧局部刷新
        &lt;/td&gt;            
        &lt;td id="mainRight"&gt;&lt;/td&gt;
    &lt;/tr&gt;    
&lt;/table&gt;

    </pre>
    
    
    <div style="margin-top:20px;"><strong>前端示例代码2</strong></div>
    <pre class="brush: html;">

&lt;div style="margin:10px; "&gt;
    &lt;div style=" background:#ffd700; padding:10px;"&gt;
    &lt;a href="#{lnkBottom}" class="frmLink" loadto="mainBottom" nolayout=2&gt;点击此处2&lt;/a&gt;，下侧刷新&lt;/div&gt;    
    &lt;div id="mainBottom" style="background:#fff;"&gt;&lt;/div&gt;
&lt;/div&gt;

    </pre>
    
    
    <div style="margin-top:20px;"><strong>前端示例代码1</strong></div>
    <pre class="brush: html;">

&lt;div&gt;&lt;a href="#{lnkBottomBox}" class="frmBox"&gt;在本局部页面中，还可以弹窗&lt;/a&gt;&lt;/div&gt;
&lt;div style="margin-top:10px;"&gt;
    &lt;textarea id="TextArea1"  style="width:280px; height:50px;"&gt;&lt;/textarea&gt;
&lt;/div&gt;


    </pre>
    
    <div style="margin-top:20px;"><strong>服务端代码</strong></div>
    <pre class="brush: csharp;">
        public void Index() {
            set( "lnkRight", to( MainRight ) );
        }

        public void MainRight() {
            set( "lnkBottom", to( MainBottom ) );
        }

        public void MainBottom() {
            set( "lnkBottomBox", to( MainBottomBox ) );
        }

        public void MainBottomBox() {
            set( "lnkRefreshParent", to( RefreshParent ) );
        }

        public void RefreshParent() {
            echoToParentPart( "刷新父页面局部" );
        }
    </pre>
    
    </div>

</div>



