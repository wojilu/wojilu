<%@ Application Language="C#" %>
<%@ Import Namespace="wojilu.Web" %>
<%@ Import Namespace="wojilu.Web.Mvc" %>
<%@ Import Namespace="wojilu.Web.Jobs" %>
<%@ Import Namespace="wojilu.Web.GlobalApp" %>

<script RunAt="server">

    void Application_Start( object sender, EventArgs e ) {
        
        SystemInfo.Init();
        MvcFilterLoader.Init();
        WebJobStarter.Init();
        
    }

    void Application_Error( object sender, EventArgs e ) {
        AppGlobalHelper gh = AppGlobalHelper.New( sender );
        gh.LogError( true );
        gh.MailError();
        gh.ClearError();
    }

    void Application_BeginRequest( object sender, EventArgs e ) {
        SystemInfo.UpdateSessionId();
    }
    
</script>

