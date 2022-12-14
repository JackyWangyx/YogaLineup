/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ListExtension.cs
//  Info     : List 扩展方法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Aya.Extension
{
    public static class ListExtension
    {
        internal static Random Rand = new Random();

        #region Sort

        /// <summary>
        /// 乱序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>结果</returns>
        public static List<T> RandSort<T>(this List<T> list)
        {
            var count = list.Count * 3;
            for (var i = 0; i < count; i++)
            {
                var index1 = Rand.Next(0, list.Count);
                var item1 = list[index1];
                var index2 = Rand.Next(0, list.Count);
                var item2 = list[index2];
                var temp = item2;
                list[index2] = item1;
                list[index1] = temp;
            }

            return list;
        }

        /// <summary>
        /// 升序排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>结果</returns>
        public static List<T> SortAsc<T>(this List<T> list) where T : IComparable
        {
            list.SortAsc(i => i);
            return list;
        }

        /// <summary>
        /// 按 Key 优先级升序排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="keyGetters">比较值访问器</param>
        /// <returns>结果</returns>
        public static List<T> SortAsc<T>(this List<T> list, params Func<T, IComparable>[] keyGetters)
        {
            list.Sort(ComparisonUtil.GetAscComparison(keyGetters));
            return list;
        }


        /// <summary>
        /// 降序排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>结果</returns>
        public static List<T> SortDesc<T>(this List<T> list) where T : IComparable
        {
            list.SortDesc(i => i);
            return list;
        }

        /// <summary>
        /// 按 Key 优先级降序排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="keyGetters">比较值访问器</param>
        /// <returns>结果</returns>
        public static List<T> SortDesc<T>(this List<T> list, params Func<T, IComparable>[] keyGetters)
        {
            list.Sort(ComparisonUtil.GetDescComparison(keyGetters));
            return list;
        }

        /// <summary>
        /// 按 Key 优先级分别进行制定方式的排序<para/>
        /// getter 返回值 int : 排序数值<para/>
        /// getter 返回值 bool : 排序方式 true 升序 false 降序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="keyGetters">比较值访问器</param>
        /// <returns>结果</returns>
        public static List<T> Sort<T>(this List<T> list, params Func<T, (IComparable, bool)>[] keyGetters)
        {
            list.Sort(ComparisonUtil.GetCustomComparison(keyGetters));
            return list;
        }

        #endregion
    }
}
