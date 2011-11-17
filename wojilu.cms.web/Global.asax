<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        //wojilu.Web.SystemInfo.Init();
        wojilu.Web.Mvc.MvcFilterLoader.Init();
    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        wojilu.Web.GlobalApp.AppGlobalHelper gh = wojilu.Web.GlobalApp.AppGlobalHelper.New( sender );
        gh.LogError( true );
        gh.ClearError();
    }

       
</script>
