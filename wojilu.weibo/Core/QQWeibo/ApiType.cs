using System;
using System.Collections.Generic;
using System.Text;


namespace wojilu.weibo.Core.QQWeibo
{

    /// <summary>
    /// 枚举类型，标识post 或者 get
    /// </summary>
    /// <remarks></remarks>
    public enum HttpMethod
    {
        httpMethod_get = 0,
        httpMethod_post,
        httpMethod_unknown
    };

    /// <summary>
    /// api的类型
    /// </summary>
    /// <remarks>命名根据api的uri进行的</remarks>
    public enum TypeOption : int{
            TXWB_BASE=0,

	        //时间线TXWB_STATUSES_
	        TXWB_STATUSES_HOME_TL,
	        TXWB_STATUSES_PUBLIC_TL,
	        TXWB_STATUSES_USER_TL,
	        TXWB_STATUSES_MENTIONS_TL,
	        TXWB_STATUSES_HT_TL,
	        TXWB_STATUSES_BROADCAST_TL,
	        TXWB_STATUSES_SPECIAL_TL,
	        TXWB_STATUSES_AREA_TL,
	        TXWB_STATUSES_HOME_TL_IDS,
	        TXWB_STATUSES_USER_TL_IDS,
	        TXWB_STATUSES_BROADCAST_TL_IDS,
	        TXWB_STATUSES_MENTIONS_TL_IDS,
	        TXWB_STATUSES_USERS_TL,
	        TXWB_STATUSES_USERS_TL_IDS,
	        //=14

	        //微博相关  TXWB_T_
	        TXWB_T_SHOW,
	        TXWB_T_ADD,
	        TXWB_T_DEL,
	        TXWB_T_RE_ADD,
	        TXWB_T_REPLY,
	        TXWB_T_ADD_PIC,
	        TXWB_T_RE_COUNT,
	        TXWB_T_RE_LIST,
	        TXWB_T_COMMENT,
	        TXWB_T_ADD_MUSIC,
	        TXWB_T_ADD_VIDEO,
	        TXWB_T_GETVIDEOINFO,
	        TXWB_T_LIST,
	        //=27

	        //账户相关TXWB_USER_
	        TXWB_USER_INFO,
	        TXWB_USER_UPDATE,
	        TXWB_USER_UPDATE_HEAD,
            TXWB_USER_UPDATE_EDU,
	        TXWB_USER_OTHER_INFO,
            TXWB_USER_INFOS,
	        //=31

	        //关系链相关 TXWB_FRIENDS_
	        TXWB_FRIENDS_FANSLIST,
	        TXWB_FRIENDS_IDOLLIST,
	        TXWB_FRIENDS_BLACKLIST,
	        TXWB_FRIENDS_SPECIALLIST,
	        TXWB_FRIENDS_ADD,
	        TXWB_FRIENDS_DEL,
	        TXWB_FRIENDS_ADDSPECIAL,
	        TXWB_FRIENDS_DELSPECIAL,
	        TXWB_FRIENDS_ADDBLACKLIST,
	        TXWB_FRIENDS_DELBLACKLIST,
	        TXWB_FRIENDS_CHECK,
	        TXWB_FRIENDS_USER_FANSLIST,
	        TXWB_FRIENDS_USER_IDOLLIST,
	        TXWB_FRIENDS_USER_SPECIALLIST,
	        //45

	        //私信相关TXWB_PRIVATE_
	        TXWB_PRIVATE_ADD,
	        TXWB_PRIVATE_DEL,
	        TXWB_PRIVATE_RECV,
	        TXWB_PRIVATE_SEND,
	        //49


	        //搜索相关 TXWB_SEARCH_
	        TXWB_SEARCH_USER,
	        TXWB_SEARCH_T,
	        TXWB_SEARCH_USERBYTAG,  //通过标签搜索用户
	        //52


	        //热度，趋势相关  TXWB_TRENDS_
	        TXWB_TRENDS_HT,
	        TXWB_TRENDS_T,
	        //=54

	        //数据更新相关TXWB_INFO_
	        TXWB_INFO_UPDATE,
	        //=55



	        //数据收藏TXWB_FAV_
	        TXWB_FAV_ADDT,
	        TXWB_FAV_DELT,
	        TXWB_FAV_LIST_T,
	        TXWB_FAV_ADDHT,
	        TXWB_FAV_DELHT,
	        TXWB_FAV_LIST_HT,
	        //=61


	        //话题相关TXWB_HT_
	        TXWB_HT_IDS,
	        TXWB_HT_INFO,
	        //=63

	

	        //标签相关TXWB_TAG_
	        TXWB_TAG_ADD,
	        TXWB_TAG_DEL,
	        //=65



	        //其他TXWB_OTHER_
	        TXWB_OTHER_KNOWNPERSON,
	        TXWB_OTHER_SHORTURL,
	        TXWB_OTHER_VIDEOKEY,
	        //=68


	        TXWB_MAX
    };

    

    class ApiType
    {
        private const string baseurl = "http://open.t.qq.com";
        /// <summary>
        /// 各个api对应的uri ，与 上面的type option 对应
        /// </summary>
        private static string[] uri = 
        {
            "base",

		
		    //时间线
		    "/api/statuses/home_timeline",
		    "/api/statuses/public_timeline",
		    "/api/statuses/user_timeline",
		    "/api/statuses/mentions_timeline",
		    "/api/statuses/ht_timeline",
		    "/api/statuses/broadcast_timeline",
		    "/api/statuses/special_timeline",
		    "/api/statuses/area_timeline",
		    "/api/statuses/home_timeline_ids",
		    "/api/statuses/user_timeline_ids",
		    "/api/statuses/broadcast_timeline_ids",
		    "/api/statuses/mentions_timeline_ids",
		    "/api/statuses/users_timeline",
		    "/api/statuses/users_timeline_ids",


		    //微博相关
		    "/api/t/show",
		    "/api/t/add",
		    "/api/t/del",
		    "/api/t/re_add",
		    "/api/t/reply",
		    "/api/t/add_pic",
		    "/api/t/re_count",
		    "/api/t/re_list",
		    "/api/t/comment",
		    "/api/t/add_music",
		    "/api/t/add_video",
		    "/api/t/getvideoinfo",
		    "/api/t/list",


		    //账户相关
		    "/api/user/info",
		    "/api/user/update",
		    "/api/user/update_head",
            "/api/user/update_edu",
		    "/api/user/other_info",
            "/api/user/infos",

		    //关系链相关
		
		    "/api/friends/fanslist",
		    "/api/friends/idollist",
		    "/api/friends/blacklist",


		    "/api/friends/speciallist",
		    "/api/friends/add",
		    "/api/friends/del",


		    "/api/friends/addspecial",
		    "/api/friends/delspecial",
		    "/api/friends/addblacklist",


		    "/api/friends/delblacklist",
		    "/api/friends/check",
		    "/api/friends/user_fanslist",

		    "/api/friends/user_idollist",
		    "/api/friends/user_speciallist",


		    //私信相关
		    "/api/private/add",
		    "/api/private/del",
		    "/api/private/recv",
		    "/api/private/send",


		    //搜索相关
		    "/api/search/user",
		    "/api/search/t",
		    "/api/search/userbytag",


		    //热度相关
		    "/api/trends/ht",
		    "/api/trends/t",


		    //数据更新相关
		    "/api/info/update",

		    //数据收藏
		    "/api/fav/addt",
		    "/api/fav/delt",
		    "/api/fav/list_t",
		    "/api/fav/addht",
		    "/api/fav/delht",
		    "/api/fav/list_ht",


		    //话题相关
		    "/api/ht/ids",
		    "/api/ht/info",


		    //标签相关
		    "/api/tag/add",
		    "/api/tag/del",


		    //其他other
		    "/api/other/knownperson",
		    "/api/other/shorturl",

		    "/api/other/videokey"
		
		
        }; 

       //根据api的类型获取该api对应的url
       public static string GetUrl(TypeOption option)
          {
                string ret = "";
                if (option <= TypeOption.TXWB_BASE || option >= TypeOption.TXWB_MAX)
                {
                    return ret;
                }
            
                ret = baseurl + uri[(int)option];
                return ret;
            }

        
        // 各类api的方法列表
       private static HttpMethod [] hmethod = {
            HttpMethod.httpMethod_unknown,  // base , means nothing


			//时间线
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			// = 14


			//微博相关
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_get,

			//=27


			//账户相关
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
            HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_get,
            HttpMethod.httpMethod_get,
			//=31


			//关系链相关

			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			


			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			

			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			

			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,

			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			//=45

			//私信相关
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			//=50

			//搜索相关
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			//=53


			//热度相关
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			//=55


			//数据更新相关
			HttpMethod.httpMethod_get,
			//=56

			//数据收藏
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_post,
			HttpMethod.httpMethod_get,
			//=61

			//话题相关
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,
			// = 63

			//标签相关
			HttpMethod.httpMethod_post,
		    HttpMethod.httpMethod_post,
			// = 65


			//其他other
			HttpMethod.httpMethod_get,
			HttpMethod.httpMethod_get,

			HttpMethod.httpMethod_get
			// = 68
        };
    
        //获取http 方法， GET or POST
        public static string GetHttpMethod(TypeOption option)
        {   
            return hmethod[(int)option] == HttpMethod.httpMethod_get?"GET":"POST";
        }

        
    }
   

   

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class ApiTypeInfo
    {
        private const string baseurl = "http://open.t.qq.com/api/";

        //api类型的数量
        public static int apiCount{get; private set;}


        //api的类型
        public int option { get; private set; }

        //api的描述
        public string desc { get; private set; }

        //对应uri
        public string uri {get; private set;}

        
        //默认参数列表
        public List<Parameter> paralist { get; private set; }


        //http 方法， get or post
        public HttpMethod httpmethod { get; private set; }


        //是否有图片，有图片需要特殊处理
        public bool ispic { get; private set; }


        public string GetUrl()
        {
            string ret = baseurl + uri;
            
            return ret;
        }


        public string GetHttpMethod()
        {
            return httpmethod == HttpMethod.httpMethod_get ? "GET" : "POST";
            
        }
    }



    public class ApiInfoConfig
    {


        public const string ConfileName = "TXWeiboApi.ini"; 
        /// <summary>
        /// Gets the type array.
        /// </summary>
        /// <remarks></remarks>
        public Dictionary<int, ApiTypeInfo>  TypeArray { get; private set; }

        /// <summary>
        /// Gets the type collection.
        /// </summary>
        /// <remarks></remarks>
        public Dictionary<string, ApiTypeInfo>  TypeCollection { get; private set;}


        /// <summary>
        /// Reads the config.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ReadConfig()
        {

            return true;
        }


    }


    
   

}
