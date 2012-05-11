using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using wojilu.Caching;

namespace wojilu.AOP
{
    /// <summary>
    /// 完全是.net2.0的妥协,这个3.5以后都系统直接自带
    /// </summary>
    public class AspectFDelegate
    {
        public delegate T Func<T>();

        public delegate TReturnType Func<T1, TReturnType>(T1 t);

        public delegate void Action();
    }

    public class AspectF
    {
        static IApplicationCache cacheResolver = CacheManager.GetApplicationCache();

        /// <summary>
        /// Chain of this to invoke
        /// </summary>

        internal Action<AspectFDelegate.Action> Chain = null;

        /// <summary>
        /// The acrual work delegate that is finally called
        /// </summary>
        internal Delegate WorkDelegate;

        /// <summary>
        /// Create a composition of function e.g. f(g(x))
        /// </summary>
        /// <param name="newAspectDelegate">A delegate that offers an aspect's behavior. 
        /// It's added into the aspect chain</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public AspectF Combine(Action<AspectFDelegate.Action> newAspectDelegate) {
            if (this.Chain == null) {
                this.Chain = newAspectDelegate;
            }
            else {
                Action<AspectFDelegate.Action> existingChain = this.Chain;
                Action<AspectFDelegate.Action> callAnother = (work) => existingChain(() => newAspectDelegate(work));
                this.Chain = callAnother;
            }
            return this;
        }

        /// <summary>
        /// Execute your real code applying the this over it
        /// </summary>
        /// <param name="work">The actual code that needs to be run</param>
        [DebuggerStepThrough]
        public void Do(AspectFDelegate.Action work) {
            if (this.Chain == null) {
                work();
            }
            else {
                this.Chain(work);
            }
        }

        /// <summary>
        /// Execute your real code applying this over it.
        /// </summary>
        /// <typeparam name="TReturnType"></typeparam>
        /// <param name="work">The actual code that needs to be run</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public TReturnType Return<TReturnType>(AspectFDelegate.Func<TReturnType> work) {
            this.WorkDelegate = work;

            if (this.Chain == null) {
                return work();
            }
            else {
                TReturnType returnValue = default(TReturnType);
                this.Chain(() => {
                    AspectFDelegate.Func<TReturnType> workDelegate = WorkDelegate as AspectFDelegate.Func<TReturnType>;
                    returnValue = workDelegate();
                });
                return returnValue;
            }
        }

        /// <summary>
        /// Handy property to start writing this using fluent style
        /// </summary>
        public static AspectF Define {
            [DebuggerStepThrough]
            get {
                return new AspectF();
            }
        }

        [DebuggerStepThrough]
        public void DoNothing() {
        }

        [DebuggerStepThrough]
        public void DoNothing(params object[] whatever) {
        }

        [DebuggerStepThrough]
        public AspectF Retry(ILog logger) {
            return this.Combine((work) =>
                Retry(1000, 1, (error) => DoNothing(error), x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public AspectF Retry(Action<IEnumerable<Exception>> failHandler, ILog logger) {
            return this.Combine((work) =>
                Retry(1000, 1, (error) => DoNothing(error), x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public AspectF Retry(int retryDuration, ILog logger) {
            return this.Combine((work) =>
                Retry(retryDuration, 1, (error) => DoNothing(error), x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public AspectF Retry(int retryDuration, Action<Exception> errorHandler, ILog logger) {
            return this.Combine((work) =>
                Retry(retryDuration, 1, errorHandler, x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public AspectF Retry(int retryDuration, int retryCount, Action<Exception> errorHandler, ILog logger) {
            return this.Combine((work) =>
                Retry(retryDuration, retryCount, errorHandler, x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public AspectF Retry(int retryDuration, int retryCount, Action<Exception> errorHandler, Action<IEnumerable<Exception>> retryFailed, ILog logger) {
            return this.Combine((work) =>
                Retry(retryDuration, retryCount, errorHandler, retryFailed, work, logger));
        }

        [DebuggerStepThrough]
        public void Retry(int retryDuration, int retryCount, Action<Exception> errorHandler, Action<IEnumerable<Exception>> retryFailed, AspectFDelegate.Action work, ILog logger) {
            List<Exception> errors = null;
            do {
                try {
                    work();
                    return;
                }
                catch (Exception x) {
                    if (null == errors)
                        errors = new List<Exception>();
                    errors.Add(x);
                    logger.Error(x.ToString());
                    errorHandler(x);
                    System.Threading.Thread.Sleep(retryDuration);
                }
            } while (retryCount-- > 0);
            retryFailed(errors);
        }

        [DebuggerStepThrough]
        public AspectF Delay( int milliseconds) {
            return Combine((work) => {
                System.Threading.Thread.Sleep(milliseconds);
                work();
            });
        }

        [DebuggerStepThrough]
        public AspectF MustBeNonDefault<T>( params T[] args) where T : IComparable {
            return Combine((work) => {
                T defaultvalue = default(T);
                for (int i = 0; i < args.Length; i++) {
                    T arg = args[i];
                    if (arg == null || arg.Equals(defaultvalue))
                        throw new ArgumentException(
                            string.Format("Parameter at index {0} is null", i));
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public AspectF MustBeNonDefault<T>(Action<Exception> errorHandler,params T[] args) where T : IComparable {
            bool error = false;
            return Combine((work) => {
                T defaultvalue = default(T);
                for (int i = 0; i < args.Length; i++) {
                    T arg = args[i];
                    if (arg == null || arg.Equals(defaultvalue)) {
                        error = true;
                        errorHandler(new ArgumentException(string.Format("Parameter at index {0} is null or default", i)));
                        break;
                    }
                }
                if(!error)
                   work();
            });
        }

        [DebuggerStepThrough]
        public AspectF MustBeNonNull( params object[] args) {
            return Combine((work) => {
                for (int i = 0; i < args.Length; i++) {
                    object arg = args[i];
                    if (arg == null)
                        throw new ArgumentException(
                            string.Format("Parameter at index {0} is null", i));
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public AspectF Until(AspectFDelegate.Func<bool> test) {
            return Combine((work) => {
                while (!test()) ;

                work();
            });
        }

        [DebuggerStepThrough]
        public AspectF While( AspectFDelegate.Func<bool> test) {
            return Combine((work) => {
                while (test())
                    work();
            });
        }

        [DebuggerStepThrough]
        public AspectF WhenTrue( params AspectFDelegate.Func<bool>[] conditions) {
            return Combine((work) => {
                foreach (AspectFDelegate.Func<bool> condition in conditions)
                    if (!condition())
                        return;

                work();
            });
        }

        [DebuggerStepThrough]
        public AspectF Log( ILog logger,string beforeMessage, string afterMessage) {
            return Combine((work) => {
                logger.Info(beforeMessage);

                work();

                logger.Info(afterMessage);
            });
        }

        [DebuggerStepThrough]
        public AspectF HowLong( ILog logger, string startMessage, string endMessage) {
            return Combine((work) => {
                DateTime start = DateTime.Now;
                logger.Info(startMessage);

                work();

                DateTime end = DateTime.Now.ToUniversalTime();
                TimeSpan duration = end - start;

                logger.Info(string.Format(endMessage, duration.TotalMilliseconds,
                    duration.TotalSeconds, duration.TotalMinutes, duration.TotalHours,
                    duration.TotalDays));
            });
        }

        [DebuggerStepThrough]
        public AspectF TrapLog( ILog logger) {
            return Combine((work) => {
                try {
                    work();
                }
                catch (Exception x) {
                    logger.Error(x.ToString());
                }
            });
        }

        [DebuggerStepThrough]
        public AspectF TrapLogThrow( ILog logger) {
            return Combine((work) => {
                try {
                    work();
                }
                catch (Exception x) {
                    logger.Error(x.ToString());
                    throw;
                }
            });
        }

        [DebuggerStepThrough]
        public AspectF RunAsync(AspectFDelegate.Action completeCallback) {
            return Combine((work) => work.BeginInvoke(asyncresult => {
                work.EndInvoke(asyncresult); completeCallback();
            }, null));
        }

        [DebuggerStepThrough]
        public AspectF RunAsync(AspectF aspect) {
            return Combine((work) => work.BeginInvoke(asyncresult => {
                work.EndInvoke(asyncresult);
            }, null));
        }

        [DebuggerStepThrough]
        public AspectF Cache<TReturnType>( string key) {
            return Combine((work) => {
                Cache<TReturnType>(key, work, cached => cached);
            });
        }

        [DebuggerStepThrough]
        public AspectF CacheList<TItemType, TListType>( string listCacheKey,AspectFDelegate.Func<TItemType, string> getItemKey)
            where TListType : IList<TItemType>, new() {
            return Combine((work) => {
                AspectFDelegate.Func<TListType> workDelegate = WorkDelegate as AspectFDelegate.Func<TListType>;

                // Replace the actual work delegate with a new delegate so that
                // when the actual work delegate returns a collection, each item
                // in the collection is stored in cache individually.
                AspectFDelegate.Func<TListType> newWorkDelegate = () => {
                    TListType collection = workDelegate();
                    foreach (TItemType item in collection) {
                        string key = getItemKey(item);
                        cacheResolver.Put(key, item);
                    }
                    return collection;
                };
                WorkDelegate = newWorkDelegate;

                // Get the collection from cache or real source. If collection is returned
                // from cache, resolve each item in the collection from cache
                Cache<TListType>(listCacheKey, work,
                    cached => {
                        // Get each item from cache. If any of the item is not in cache
                        // then discard the whole collection from cache and reload the 
                        // collection from source.
                        TListType itemList = new TListType();
                        foreach (TItemType item in cached) {
                            object cachedItem = cacheResolver.Get(getItemKey(item));
                            if (null != cachedItem) {
                                itemList.Add((TItemType)cachedItem);
                            }
                            else {
                                // One of the item is missing from cache. So, discard the 
                                // cached list.
                                return default(TListType);
                            }
                        }

                        return itemList;
                    });
            });
        }

        [DebuggerStepThrough]
        public AspectF CacheRetry<TReturnType>( ILog logger, string key) {
            return Combine((work) => {
                try {
                    Cache<TReturnType>( key, work, cached => cached);
                }
                catch (Exception x) {
                    logger.Error(x.ToString());
                    System.Threading.Thread.Sleep(1000);

                    //Retry
                    try {
                        Cache<TReturnType>( key, work, cached => cached);
                    }
                    catch (Exception ex) {
                        logger.Error(ex.ToString());
                        throw ex;
                    }
                }
            });
        }

        private  void Cache<TReturnType>( string key, AspectFDelegate.Action work, AspectFDelegate.Func<TReturnType, TReturnType> foundInCache) {
            object cachedData = cacheResolver.Get(key);
            if (cachedData == null) {
                GetListFromSource<TReturnType>( key);
            }
            else {
                // Give caller a chance to shape the cached item before it is returned
                TReturnType cachedType = foundInCache((TReturnType)cachedData);
                if (cachedType == null) {
                    GetListFromSource<TReturnType>( key);
                }
                else {
                    WorkDelegate = new AspectFDelegate.Func<TReturnType>(() => cachedType);
                }
            }

            work();
        }

        private  void GetListFromSource<TReturnType>( string key) {
            AspectFDelegate.Func<TReturnType> workDelegate =WorkDelegate as AspectFDelegate.Func<TReturnType>;
            TReturnType realObject = workDelegate();
            cacheResolver.Put(key, realObject);
            workDelegate = () => realObject;
            WorkDelegate = workDelegate;
        }
    }   
}
