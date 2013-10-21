<div style="padding:20px; background:#fff;">

<style>
.formPanel {margin:10px; padding:10px; background:#eee; line-height:200%;}
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}
</style>
    <div></div>
    
    <div>

    <h3 style="margin-top:30px;">没有前缀 <span class="note">ctx.PostObject&lt;TPObject&gt;()</span></h3>
    <form method="post" action="#{objectLink}" class="ajaxPostForm">
        <div>年龄：<input name="Age" type="text" style="width:80px;" /></div>
        <div>名字：<input name="Name" type="text" /></div>
        <div><input type="submit" value="提交" class="btn btn-primary" /></div>
    </form>


    <h3 style="margin-top:30px;">有前缀 <span class="note">ctx.PostObject&lt;TPObject&gt;("x1")</span></h3>
    <form method="post" action="#{objectLabelLink}" class="ajaxPostForm">
        <div>年龄：<input name="x1.Age" type="text" style="width:80px;" /></div>
        <div>名字：<input name="x1.Name" type="text" /></div>
        <div><input type="submit" value="提交" class="btn btn-primary" /></div>
    </form>


    <h3 style="margin-top:30px;">传统：没有前缀 <span class="note">ctx.PostValue&lt;TPObject&gt;()</span></h3>
    <form method="post" action="#{valueLink}" class="ajaxPostForm">
        <div>年龄：<input name="tPObject.Age" type="text" style="width:80px;" /></div>
        <div>名字：<input name="tPObject.Name" type="text" /></div>
        <div><input type="submit" value="提交" class="btn btn-primary" /></div>
    </form>


    <h3 style="margin-top:30px;">传统：有缀 <span class="note">ctx.PostValue&lt;TPObject&gt;("x2")</span></h3>
    <form method="post" action="#{valueLabelLink}" class="ajaxPostForm">
        <div>年龄：<input name="x2.Age" type="text" style="width:80px;" /></div>
        <div>名字：<input name="x2.Name" type="text" /></div>
        <div><input type="submit" value="提交" class="btn btn-primary" /></div>
    </form>




    </div>


    
</div>
