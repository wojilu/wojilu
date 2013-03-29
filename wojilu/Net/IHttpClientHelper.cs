using System;
using System.Collections.Generic;
using wojilu.Web;
namespace wojilu.Net {

    public interface IHttpClientHelper {

        String InvokeApi( String apiUrl, String httpMethod, String strQuery, Dictionary<String, String> headers, String boundary = "", String strFiles = "", String userAgent = "", String strEncoding = "" );

        String Upload( String apiUrl, Dictionary<String, String> parameters, Dictionary<String, String> headers, List<HttpFile> files, String userAgent );

        String ConstructQueryString( Dictionary<String, String> parameters );

    }

}
