<style>
#formWrap .control-label { font-size:14px; font-weight:bold;}
#formWrap .controls {font-size:14px;}
</style>


<div style="width:600px; margin:30px auto;">
    <div style="margin-left:80px;">
        <h3 style=" border-bottom:1px solid #ccc; padding:5px 10px;">由于您是第一次登录，请确认</h3>
        <div style="margin:5px 5px 15px 10px;">您访问本网站，比如发帖等，将会用到如下信息——</div>
    </div>

    <form action="#{ActionLink}" method="post" style=" padding:10px;" class="form-horizontal ajaxPostForm">


    <div style=" " id="formWrap">
	    <div class="control-group">
		    <label class="control-label" for="input01">用户名</label>
		    <div class="controls">
			    <input type="text" class="input-xlarge" id="input01" name="userName" value="#{userName}">
		    </div>
	    </div>

	    <div class="control-group">
		    <label class="control-label" for="input02">友好网址</label>
		    <div class="controls">
                <span class="help-inline">#{userUrlPrefix}</span>
			    <input type="text" class="input-xlarge" id="input02" name="userUrl" value="#{userUrl}" style="width:80px;">
			    <span class="help-inline">#{urlExt}</span>
		    </div>
	    </div>

	    <div class="control-group">
		    <label class="control-label">头像</label>
		    <div class="controls">
			    <p class="help-block"><img src="#{userPic}" /></p>
		    </div>
	    </div>

	    <div class="control-group">
		    <label class="control-label" for="input01">&nbsp;</label>
		    <div class="controls">
                <button type="submit" class="btn btn-primary btn-large">
                    <i class="icon-user icon-white"></i>
                    登录
                </button>
                
                <input type="hidden" name="uid" value="#{uid}" />
                <input type="hidden" name="accessToken" value="#{accessToken}" />
                <input type="hidden" name="refreshToken" value="#{refreshToken}" />
                <input type="hidden" name="expiresIn" value="#{expiresIn}" />
                <input type="hidden" name="scope" value="#{scope}" />

		    </div>
	    </div>
        </div>
    </form>

</div>

<script>
    _run(function () {
        wojilu.ui.ajaxPostForm();
    });
</script>