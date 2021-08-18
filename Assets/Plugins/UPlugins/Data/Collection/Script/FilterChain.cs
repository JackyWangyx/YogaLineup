/////////////////////////////////////////////////////////////////////////////
//
//  Script   : FilterChain.cs
//  Info     : 过滤器链
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Tip		 : 过滤器链可以创建一组对数据进行处理的处理器，然后流水线式的对数据进行处理，返回最终处理完的结果。
//
//  Copyright : Aya Game Studio 2017
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Aya.Data
{
	public class FilterChain<T>
	{
		/// <summary>
		/// 过滤器列表
		/// </summary>
		private readonly List<Func<T, T>> _dataFilters = new List<Func<T, T>>();

		/// <summary>
		/// 添加一个过滤器
		/// </summary>
		/// <param name="filter">过滤器</param>
		public void Add(Func<T, T> filter)
		{
            if (filter == null)
            {
				throw new NullReferenceException("Filter is NULL");
            }

			_dataFilters.Add(filter);
		}

		/// <summary>
		/// 执行过滤器
		/// </summary>
		/// <param name="data">初始数据</param>
		public T Execute(T data)
		{
			var result = data;
			for (var i = 0; i < _dataFilters.Count; i++)
			{
				var filter = _dataFilters[i];
				result = filter(result);
			}
			return result;
		}

		/// <summary>
		/// 清空过滤器
		/// </summary>
		public void Clear()
		{
			_dataFilters.Clear();
		}

		/// <summary>
		/// 获取所有过滤器
		/// </summary>
		/// <returns>过滤器集合</returns>
		public List<Func<T, T>> GetFilters()
		{
			return _dataFilters;
		}
	}
}
