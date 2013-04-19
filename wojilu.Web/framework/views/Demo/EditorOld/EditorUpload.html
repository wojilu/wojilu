<div style="padding:20px; background:#fff;">

<style>
.formPanel {margin:10px; padding:10px; background:#eee; line-height:200%;}
.formTitle {font-weight:bold; padding-left:80px;}
.tdLeft { vertical-align:top;width:80px;}
</style>


   
    <div>
    <div class="strong">服务端代码</div>
    <div class="red strong" style="margin:10px 0px;">注意：“我记录网站综合系统”中已经全部实现了如下功能，请勿在“我记录网站综合系统”中额外添加。</div>
    <div class="red" style="margin-top:10px;">1）设置上传图片的网址。一般在 IMvcFilter 过滤器的 Begin_InitContext 事件中设置此网址</div>
    <div class="note left15">其实不一定要使用 IMvcFilter，只要在使用editor( "", "", "" )方法之前设置ctx.SetItem( "editorUploadUrl", "")即可，这里使用 IMvcFilter 是可以对全站生效，避免每个action中逐个编码。</div>
    <pre class="brush: c-sharp;">
    
    public class ContextInit : IMvcFilter {

        public void Process( wojilu.Web.Mvc.MvcEventPublisher publisher ) {

            publisher.Begin_InitContext += 
                new EventHandler&lt;wojilu.Web.Mvc.MvcEventArgs&gt;( publisher_Begin_InitContext );
        }

        void publisher_Begin_InitContext( object sender, wojilu.Web.Mvc.MvcEventArgs e ) {

            MvcContext ctx = e.ctx;
            
            User user = getUserById( ctx.web.UserId() ); // 此处获取用户信息，请自己实现 getUserById 方法

            // 如果用户已经登录
            if (user.IsLogin) {

                Link lnk = new Link( ctx );
                
                // "editorUploadUrl"和"editorMyPicsUrl"是框架内置的两个网址名称
                
                // 设置接收图片上传的服务端网址
                ctx.SetItem( "editorUploadUrl", lnk.To( user, "Users/Admin/UserUpload", "UploadForm", -1, -1 ) );
                    
                // 用户图片浏览(可选)
                ctx.SetItem( "editorMyPicsUrl", lnk.To( user, "Users/Admin/UserUpload", "MyPics", -1, -1 ) );
                
            }   
    
        }


    }  
    </pre>
    </div>
    
    <div class="red" style="margin-top:20px;">2）完成图片上传的action</div>
    <div class="note left15">
    下面截取了“我记录网站综合系统”的部分代码，它实现了两种上传模式，1)传统上传，2)flash带进度条上传<br />
    更多请参考“我记录网站综合系统”中的实现，<br />
    服务端源码在 wojilu.Controller/Users/Admin/UserUploadController.cs <br />
    视图界面源码在 wojilu.Web/framework/views/Users/Admin/UserUpload/ 目录下(包括两种上传界面)。
    </div>
    <pre class="brush: c-sharp;">


        public void UploadForm() {

            Boolean isFlash = ("normal".Equals( ctx.Get( "type" ) ) == false);

            if (isFlash) { // flash上传界面

                view( "FlashUpload" );
                set( "uploadLink", to( SaveFlash ) );
                set( "authCookieName", ctx.web.CookieAuthName() );
                set( "authCookieValue", ctx.web.CookieAuthValue() );
                
                String editorName = ctx.Get( "editor" );
                set( "editorName", editorName );
                set( "normalLink", to( UploadForm ) + "?type=normal&editor=" + editorName );

                set( "jsPath", sys.Path.DiskJs );

            }
            else {
                target( SavePic );
                String editorName = ctx.Get( "editor" );
                set( "editorName", editorName );
            }
        }

        public void SaveFlash() {

            PhotoPost post = savePicPrivate();

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            // 在编辑器中只允许使用中略图，原始图片可能很大，影响页面效果
            set( "picUrl", post.ImgMediumUrl );
            set( "oPicUrl", post.ImgUrl );

            String json = "{PicUrl:'" + post.ImgMediumUrl + "', OpicUrl:'" + post.ImgUrl + "'}";

            echoText( json );
        }

        public void SavePic() {

            String editorName = ctx.Post( "editor" );
            set( "editorName", editorName );
            String uploadUrl = to( UploadForm ) + "?editor=" + editorName;
            set( "uploadUrl", uploadUrl );

            PhotoPost post = savePicPrivate();

            if (ctx.HasErrors) {
                echoRedirect( errors.ErrorsHtml, uploadUrl );
                return;
            }

            // 在编辑器中只允许使用中略图，原始图片可能很大，影响页面效果
            set( "picUrl", post.ImgMediumUrl );
            set( "oPicUrl", post.ImgUrl );

        }

        private PhotoPost savePicPrivate() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveImg( postedFile ); // 保存上传的图片(包括缩略图)
            if (result.HasErrors) {
                errors.Join( result );
                return null;
            }

            PhotoPost post = new PhotoPost();

            post.DataUrl = result.Info.ToString(); // 上传的图片路径
            post.Title = strUtil.CutString( Path.GetFileNameWithoutExtension( postedFile.FileName ), 20 ); // 图片名称
            post.Ip = ctx.Ip;
            
            // ... 其他代码

            return post;

        }

    </pre>
    
    
</div>
