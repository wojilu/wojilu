//using System;
//using System.Collections.Generic;
//using wojilu;
//using wojilu.Web;
//using wojilu.Web.Mvc.Routes;
//using wojilu.Web.Context;
//using wojilu.Web.Mvc;

//namespace wojilu.Web.Context {

//    public interface IMvcContext {

//        IAppContext app { get; }
//        IOwnerContext owner { get; }
//        IViewerContext viewer { get; }
//        Route route { get; }
//        ControllerBase controller { get; }

//        IWebContext web { get; }
//        MvcContextUtils utils { get; }

//        Result errors { get; }
//        bool HasErrors { get; }
//        MvcException ex( string httpStatus, string msg );
//        Result Validate( IEntity target );

//        bool HasUploadFiles { get; }
//        List<HttpFile> GetFiles();
//        HttpFile GetFileSingle();

//        object GetItem( string key );
//        void SetItem( string key, object val );

//        PageMeta GetPageMeta();
//        string HttpMethod { get; }
//        string Ip { get; }
//        UrlInfo url { get; }

//        string Get( string queryItem );
//        int GetInt( string queryItemName );
//        string GetIdList( string idname );
//        bool GetHas( string key );

//        string Params( string itemName );
//        int ParamInt( string postItem );
//        decimal ParamDecimal( string postItem );

//        string Post( string postItem );
//        int PostInt( string postItem );
//        decimal PostDecimal( string postItem );
//        DateTime PostTime( string postItem );
//        string PostHtml( string postItem );
//        string PostHtml( string postItem, string allowedTags );
//        string PostHtmlAll( string postItem );
//        string PostIdList( string idname );
//        int PostIsCheck( string postItem );
//        T PostValue<T>();
//        object PostValue( object obj );
//        bool PostHas( string key );

//        void RenderJson( string jsonContent );
//        void RenderXml( string xmlContent );

//        string t2( aAction action );
//        string t2( aActionWithId action, int id );
//        string to( aAction action );
//        string to( aActionWithId action, int id );
//        Link GetLink();

//    }

//}
