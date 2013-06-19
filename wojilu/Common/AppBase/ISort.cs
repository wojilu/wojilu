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
using System.Collections.Generic;

namespace wojilu.Common.AppBase {

    /// <summary>
    /// 可排序对象接口
    /// </summary>
    public interface ISort {
        int Id { get; set; }
        int OrderId { get; set; }
        void updateOrderId();
    }

    /// <summary>
    /// 排序工具封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortUtil<T> where T : ISort {

        private T data;
        private List<T> list;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">需要移动的对象</param>
        /// <param name="list">对象列表</param>
        public SortUtil( T data, List<T> list ) {
            this.data = data;
            this.list = list;
        }

        /// <summary>
        /// 获取经过排序的列表
        /// </summary>
        /// <returns></returns>
        public List<T> GetOrderedList() {
            return this.list;
        }

        /// <summary>
        /// 向前移动
        /// </summary>
        public void MoveUp() {

            int dataId = data.Id;

            for (int i = 0; i < list.Count; i++) {

                T t = list[i];
                int orderId = list.Count - i;

                if (t.Id == dataId && i == 0) continue;

                if (isPrevData( i, dataId )) {
                    t.OrderId = orderId - 1;
                    t.updateOrderId();
                }
                else if (t.Id == dataId) {
                    t.OrderId = orderId + 1;
                    t.updateOrderId();
                }
                else if (t.OrderId != orderId) {
                    t.OrderId = orderId;
                    t.updateOrderId();
                }

            }
        }

        /// <summary>
        /// 向后移动
        /// </summary>
        public void MoveDown() {

            int dataId = data.Id;

            for (int i = 0; i < list.Count; i++) {

                T t = list[i];
                int orderId = list.Count - i;

                if (t.Id == dataId && i == list.Count - 1) continue;

                if (isNextData( i, dataId )) {
                    t.OrderId = orderId + 1;
                    t.updateOrderId();
                }
                else if (t.Id == dataId) {
                    t.OrderId = orderId - 1;
                    t.updateOrderId();
                }
                else if (t.OrderId != orderId) {
                    t.OrderId = orderId;
                    t.updateOrderId();
                }

            }

        }

        private Boolean isNextData( int i, int dataId ) {
            if (i == 0) return false;
            if (list[i - 1].Id == dataId) return true;
            return false;
        }

        private Boolean isPrevData( int i, int dataId ) {
            if (i > list.Count - 2) return false;
            if (list[i + 1].Id == dataId) return true;
            return false;
        }


    }




}
