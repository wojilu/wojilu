using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Domain;
using System.Collections;
using wojilu.DI;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Common.Feeds
{
    /// <summary>
    /// 微博同步管理器,该类管理所有注册到系统中的微博同步类
    /// </summary>
    public class MicroblogSyncManager
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MicroblogSyncManager));
        private static IList<IMicroblogSync> items = new List<IMicroblogSync>();

        private static readonly Object objlock = new object();

        private static MicroblogSyncManager _instance;

        private MicroblogSyncManager() { }

        public static MicroblogSyncManager Instance
        {
            get
            {

                lock (objlock)
                {

                    if (_instance == null)
                    {
                        initSyncs();
                        _instance = new MicroblogSyncManager();
                    }

                }
                return _instance;
            }
        }

        private static void initSyncs()
        {
            IList configs = new MicroblogSyncConfig().findAll();
            foreach (MicroblogSyncConfig config in configs)
            {
                IMicroblogSync obj = ObjectContext.GetByType(config.Type) as IMicroblogSync;
                if (obj != null)
                {
                    items.Add(obj);
                }
            }
        }

        /// <summary>
        /// 向已注册到系统中的微博同步类同步微博并且上传一张图片
        /// </summary>
        /// <param name="user">同步的用户</param>
        /// <param name="text">微博内容 格式与新浪微博，QQ微博等一样，140个字以内，不要加html标签</param>
        /// <param name="picUrl">上传图片的本地地址，如c:\1.jpg这样的</param>
        public void SyncWithPic(IUser user, string text, string picUrl)
        {
            foreach (IMicroblogSync item in items)
            {
                item.Sync(user, text, picUrl);
            }
        }
        /// <summary>
        /// 向已注册到系统中的微博同步类同步微博
        /// </summary>
        /// <param name="user">同步的用户</param>
        /// <param name="text">微博内容 格式与新浪微博，QQ微博等一样，140个字以内，不要加html标签</param>
        public void Sync(IUser user,string text)
        {
            SyncWithPic(user, text, string.Empty);
        }
    }
}
