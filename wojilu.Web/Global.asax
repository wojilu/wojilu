<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start( object sender, EventArgs e ) {
        wojilu.Web.SystemInfo.Init();
        wojilu.Web.Mvc.MvcFilterLoader.Init();
        wojilu.Web.Jobs.WebJobStarter.Init();
    }

    void Application_Error( object sender, EventArgs e ) {
        wojilu.Web.GlobalApp.AppGlobalHelper gh = wojilu.Web.GlobalApp.AppGlobalHelper.New( sender );
        gh.LogError( true );
        gh.MailError();
        gh.ClearError();
    }

    void Application_BeginRequest( object sender, EventArgs e ) {
        wojilu.Web.SystemInfo.UpdateSessionId();
    }
    
</script>

