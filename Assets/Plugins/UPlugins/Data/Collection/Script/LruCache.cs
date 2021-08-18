/////////////////////////////////////////////////////////////////////////////
//
//  Script   : LruCache.cs
//  Info     : 最近最少使用缓存
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Tip		 : 用键值对的形式缓存一定数量的数据，最新添加和被使用的，会移动到链表头部。
//			   当链表超过限制缓存数时，链表末端的对象会被移除。
//			   使用该结构缓存少量数据时，命中率会很高，但性能会随着数据量的增加而急剧下降。
//
//  Copyright : Aya Game Studio 2017
//
/////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace Aya.Data
{
	public class LruCache<TKey, TValue>
	{
		private readonly LinkedList<KeyValuePair<TKey, TValue>> _cache = new LinkedList<KeyValuePair<TKey, TValue>>();
		private readonly Dictionary<TKey, KeyValuePair<TKey, TValue>> _cacheDic = new Dictionary<TKey, KeyValuePair<TKey, TValue>>();

		private readonly int _maxCount;

		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="maxCount">最大缓存数量</param>
		public LruCache(int maxCount = 128)
		{
			_maxCount = maxCount;
		} 

		/// <summary>
		/// 获取元素
		/// </summary>
		/// <param name="key">键</param>
		/// <returns>值</returns>
		public TValue Get(TKey key)
		{
			// 从字典取出，并移动到链表头部
			if (!_cacheDic.ContainsKey(key)) return default(TValue);
			var pair = _cacheDic[key];
			_cache.Remove(pair);
			_cache.AddFirst(pair);
			return pair.Value;
		}

		/// <summary>
		/// 添加元素
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		public void Add(TKey key, TValue value)
		{
			// 如果存在则移动到头部，同时移除链表末端的元素，不存在则添加到头部
			if (_cacheDic.ContainsKey(key))
			{
				var pair = _cacheDic[key];
				_cache.Remove(pair);
				_cache.AddFirst(pair);
			    if (_cache.Count <= _maxCount) return;
			    var temp = _cache.Last;
			    _cacheDic.Remove(temp.Value.Key);
			    _cache.Remove(temp);
			}
			else
			{
				var pair = new KeyValuePair<TKey, TValue>(key, value);
				_cacheDic.Add(key, pair);
				_cache.AddFirst(pair);
			}
		}
	}
}
