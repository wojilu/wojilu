<%@ Page Language="C#" %>

<%@ Import Namespace="wojilu" %>
<%@ Import Namespace="wojilu.View" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="wojilu.Apps.Photo.Domain" %>


<script runat="server">
    public List<wojilu.Apps.Forum.Domain.ForumBoard> getSubList( List<wojilu.Apps.Forum.Domain.ForumBoard> boards, long parentId) {
        List<wojilu.Apps.Forum.Domain.ForumBoard> subList = new List<wojilu.Apps.Forum.Domain.ForumBoard>();
        for (int x = 0; x < boards.Count; x++) {
            if (boards[x].ParentId == parentId) subList.Add( boards[x] );
        }
        return subList;
    }
</script>

<div style="margin:20px;">

    <div>
        <%
        

            if (ctx.viewer.IsLogin) {
                wojilu.Members.Users.Domain.User user = ctx.viewer.obj as wojilu.Members.Users.Domain.User;
                html.show( "欢迎您，name=" + user.Name + "; email="+user.Email  );
            }
            else {
                html.show( "没有登录" );
            }
        
        
    
        %>
    </div>
    <div>
        欢迎信息：<%= v.data("welcomeMsg") %>
    </div>

    <div style="margin:20px auto;">
        <h2>相册</h2>
        <%
            List<PhotoPost> photos = PhotoPost
                .find( "OwnerId="+ctx.viewer.Id + " order by Id desc" )
                .list( 10 );
        %>
        <ul>
        <% foreach( PhotoPost x in photos ) { %>
            <li style=" float:left; display:block; width:150px; height:150px; background:#ccc; padding:5px;">
                <div><a href="<%= link.data(x) %>"><img src="<%= path.pic( x.DataUrl, "m" ) %>" /></a></div>
                <div><%= x.Created.ToString("yyyy-MM-dd") %></div>                
            </li><%} %>
        </ul>
        <div class="clear"></div>
    </div>

    <div style="margin:20px auto;">
    <div>登录：<%= link.to( new wojilu.Web.Controller.MainController().Login ) %></div>

    <%
        List<wojilu.Apps.Forum.Domain.ForumBoard> boards = wojilu.Apps.Forum.Domain.ForumBoard.find( "OwnerId=0" ).list();
            
    %>
    
    <ul>
        <% for (int i = 0; i < boards.Count; i++) { %>

            <% if( boards[i].ParentId>0 ) continue; %>
            <li class="strong"><a href=""><%= boards[i].Name %></a></li>

            <%
                List<wojilu.Apps.Forum.Domain.ForumBoard> subList = getSubList( boards, boards[i].Id );
                foreach (wojilu.Apps.Forum.Domain.ForumBoard sub in subList) {%>

                
                <li>-----<a href="<%= link.data(sub) %>"><%= sub.Name %></a> 作者：<a href="<%= link.user(sub.Creator) %>"><%= sub.Creator.Name %></a> </li>
                        
            <%}%>


        <%} %>
    </ul>
    
    </div>

    <div style="margin-top:20px;">当前ctx信息：</div>
    <table class="table">
        <tr><td style="width:80px;">ctx.ip</td><td><%= ctx.ip %></td></tr>
        <tr><td>ctx.method</td><td><%= ctx.method %></td></tr>
        <tr><td>ctx.agent</td><td><%= ctx.agent %></td></tr>
        <tr><td>ctx.get("name")</td><td><%= ctx.get("name") %></td></tr>
        <tr><td>ctx.getInt("count")</td><td><%= ctx.getInt("count") %></td></tr>
    </table>

    <div>当前url信息：</div>
    <table class="table">
        <tr><td style="width:80px;">url.text</td><td><%= url.text %></td></tr>
        <tr><td>url.path</td><td><%= url.path %></td></tr>
        <tr><td>url.query</td><td><%= url.query %></td></tr>
        <tr><td>url.from</td><td><%= url.from %></td></tr>
    </table>

</div>