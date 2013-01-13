/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;

namespace wojilu {

    /// <summary>
    /// 分页后的结果集，非泛型对象。泛型请用 DataPage<T>
    /// </summary>
    public class DataPageInfo : IPageList {

        public DataPageInfo() {
        }

        public DataPageInfo( IList list ) {
            this.Results = list;
        }

        private int _current;
        private String _pageBar;
        private int _pageCount;
        private int _recordCount;
        private IList _results;
        private int _size;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Current {
            get { return _current; }
            set { _current = value; }
        }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int Size {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// 已经封装好的html分页栏
        /// </summary>
        public String PageBar {
            get { return _pageBar; }
            set { _pageBar = value; }
        }

        /// <summary>
        /// 总共页数
        /// </summary>
        public int PageCount {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        /// <summary>
        /// 所有记录数
        /// </summary>
        public int RecordCount {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        /// <summary>
        /// 当前页的数据列表
        /// </summary>
        public IList Results {
            get { return _results; }
            set { _results = value; }
        }

        /// <summary>
        /// 返回一个空的分页结果集
        /// </summary>
        /// <returns></returns>
        public static DataPageInfo GetEmpty() {
            DataPageInfo p = new DataPageInfo();
            p.Results = new ArrayList();
            p.Current = 1;
            p.RecordCount = 0;
            return p;
        }


    }
}

