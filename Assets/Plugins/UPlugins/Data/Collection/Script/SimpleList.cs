/////////////////////////////////////////////////////////////////////////////
//
//  Script   : SimpleList.cs
//  Info     : 去除GC的 精简版List
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Aya.Data
{
    [Serializable]
    public class SimpleList<T> : IList<T>, IList, ICollection<T>, ICollection, IEnumerable<T>, IEnumerable
    {
        #region Private Field
        // 容量
        internal int Capacity = 10;
        // 内部数组
        internal T[] Items;
        // 存放的单元个数
        internal int Length;
        // 可能空闲的单元下标
        internal int MayIdleIndex;
        // 迭代器器组
        internal IEnumerator<T>[] Enumerators;
        // 迭代器组当前占用状态
        internal bool[] EnumStates; 
        #endregion

        #region Properity
        /// <summary>
        /// 获得长度
        /// </summary>
        public virtual int Count => Length;

        #endregion

        #region Construct
        public SimpleList()
        {
            Init(5);
        }

        public SimpleList(int capacity)
        {
            Init(capacity);
        }
        #endregion

        #region Protect
        protected void Init(int capacity)
        {
            Capacity = Capacity < 10 ? 10 : Capacity;
            capacity = capacity < 5 ? 5 : capacity;
            Items = new T[Capacity];
            if (Enumerators != null) return;
            Enumerators = new IEnumerator<T>[capacity];
            EnumStates = new bool[capacity];
            for (var i = 0; i < Enumerators.Length; i++)
            {
                Enumerators[i] = new SimpleEnumerator<T>(this, i);
            }
        }

        /// <summary>
        /// 增长容量
        /// </summary>
        protected void IncreaseCapacity()
        {
            if (Length < Capacity) return;
            var newCapacity = Capacity;
            if (newCapacity == 0)
            {
                newCapacity++;
            }
            newCapacity *= 2;
            var datasNew = new T[newCapacity];
            Array.Copy(Items, 0, datasNew, 0, Length);
            Items = datasNew;
            Capacity = newCapacity;
        }
        #endregion

        #region Index
        /// <summary>
        /// 索引访问器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>结果</returns>
        public virtual T this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException();
                }
                return Items[index];
            }
            set
            {
                if (index < 0 || index >= Length)
                {
                    throw new IndexOutOfRangeException();
                }
                Items[index] = value;
            }
        }
        #endregion

        #region Add / Insert
        /// <summary>
        /// 增加单元
        /// </summary>
        /// <param name="element">添加的单元</param>
        public void Add(T element)
        {
            IncreaseCapacity();
            // 赋值
            Items[Length] = element;
            Length++;
        }

        /// <summary>
        /// 增加单元
        /// </summary>
        /// <param name="value">添加的单元</param>
        /// <returns>添加位置的索引</returns>
        public int Add(object value)
        {
            try
            {
                IncreaseCapacity();
                // 赋值
                Items[Length] = (T) value;
                Length++;
                return Length - 1;
            }
            catch
            {
                throw new ArrayTypeMismatchException();
            }
        }

        /// <summary>
        /// 添加元素集合
        /// </summary>
        /// <param name="collection">集合</param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// 插入单元
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="element">单元</param>
        /// <returns>操作是否成功</returns>
        public virtual bool Insert(int index, T element)
        {
            if (index < 0)
            {
                return false;
            }
            if (index >= Length)
            {
                Add(element);
                return true;
            }
            IncreaseCapacity();
            // 向后拷贝
            Array.Copy(Items, index, Items, index + 1, Length - index);

            Items[index] = element;

            Length++;
            return true;
        }

        /// <summary>
        /// 插入单元
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">单元</param>
        public void Insert(int index, object value)
        {
            if (value is T)
            {
                Insert(index, (T) value);
            }
            throw new ArrayTypeMismatchException();
        }
        #endregion

        #region Contain / Index Of
        /// <summary>
        /// 是否包含某个单元
        /// </summary>
        /// <param name="element">单元</param>
        /// <returns>是否包含</returns>
        public bool Contains(T element)
        {
            for (var i = 0; i < Length; i++)
            {
                if (Items[i].Equals(element))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否包含某个单元
        /// </summary>
        /// <param name="value">单元</param>
        /// <returns>是否包含</returns>
        public bool Contains(object value)
        {
            for (var i = 0; i < Length; i++)
            {
                if (Items[i].Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定单元在当前列表中的位置，从前向后查找
        /// </summary>
        /// <param name="element">单元</param>
        /// <returns>位置</returns>
        public int IndexOf(T element)
        {
            for (var i = 0; i < Length; i++)
            {
                if (Items[i].Equals(element))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取指定单元在当前列表中的位置，从前向后查找
        /// </summary>
        /// <param name="value">单元</param>
        /// <returns>位置</returns>
        public int IndexOf(object value)
        {
            for (var i = 0; i < Length; i++)
            {
                if (Items[i].Equals(value))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取指定单元在当前列表中的位置，从后往前查找
        /// </summary>
        /// <param name="element">单元</param>
        /// <returns>位置</returns>
        public int LastIndexOf(T element)
        {
            for (var i = Length - 1; i >= 0; i--)
            {
                if (Items[i].Equals(element))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取指定单元在当前列表中的位置，从前往后查找
        /// </summary>
        /// <param name="element">单元</param>
        /// <returns>位置</returns>
        public int FirstIndexOf(T element)
        {
            for (var i = 0; i < Length; i++)
            {
                if (Items[i].Equals(element))
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region Clear / Remove
        /// <summary>
        /// 清空单元数组
        /// </summary>
        public virtual void Clear()
        {
            for (var i = 0; i < Length; i++)
            {
                Items[i] = default(T);
            }
            Length = 0;
        }

        /// <summary>
        /// 移除指定单元，如果单元归属权属于当前列表，则会将其卸载
        /// </summary>
        /// <param name="element">单元</param>
        /// <returns>是否操作成功</returns>
        public virtual bool Remove(T element)
        {
            var index = IndexOf(element);
            if (index < 0)
            {
                return false;
            }
            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// 移除指定单元，如果单元归属权属于当前列表，则会将其卸载
        /// </summary>
        /// <param name="value">单元</param>
        public void Remove(object value)
        {
            var index = IndexOf(value);
            if (index >= 0)
            {
                RemoveAt(index);
            }
        }

        /// <summary>
        /// 移除指定位置的单元，如果单元归属权属于当前列表，则会将其卸载
        /// </summary>
        /// <param name="index">位置索引</param>
        /// <returns>移除掉的单元</returns>
        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= Length)
            {
                return;
            }
            for (var i = index; i <= Length - 2; i++)
            {
                Items[i] = Items[i + 1];
            }
            Length--;
        }

        public bool IsFixedSize { get; private set; }

        /// <summary>
        /// 移除指定尾部单元
        /// </summary>
        /// <returns>移除掉的单元</returns>
        public virtual T RemoveEnd()
        {
            if (Length <= 0)
            {
                return default(T);
            }
            var temp = Items[Length - 1];
            Items[Length - 1] = default(T);
            Length--;
            return temp;
        }

        /// <summary>
        /// 从指定位置开始(包括当前)，移除后续单元，如果单元归属权属于当前列表，则会将其卸载
        /// </summary>
        /// <param name="index">要移除的位置</param>
        /// <returns>被移除的个数，如果index越界，则返回-1</returns>
        public virtual int RemoveAllFrom(int index)
        {
            if (index < 0 || index >= Length)
            {
                return -1;
            }
            var removedNum = 0;
            for (var i = Length - 1; i >= index; i--)
            {
                Items[i] = default(T);
                Length--;
                removedNum++;
            }
            return removedNum;
        }
        #endregion

        #region Sort
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="comparison">比较器</param>
        public void Sort(Comparison<T> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException();
            }
            if (Length <= 0)
            {
                return;
            }
            Array.Sort(Items, comparison);
        }
        #endregion

        #region Find
        /// <summary>
        /// 查找一个满足条件的元素
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>结果</returns>
        public T Find(Predicate<T> predicate)
        {
            for (var i = 0; i < Items.Length; i++)
            {
                var item = Items[i];
                if (predicate(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 查找所有满足条件的元素
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>结果</returns>
        public SimpleList<T> FindAll(Predicate<T> predicate)
        {
            var ret = new SimpleList<T>();
            for (var i = 0; i < Items.Length; i++)
            {
                var item = Items[i];
                if (predicate(item))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }
        #endregion

        #region Array
        /// <summary>
        /// 获取所有数据，注意这里的数据可能包含了很多冗余空数据，长度>=当前数组长度。
        /// </summary>
        /// <returns>所有数据数组</returns>
        public T[] GetAllItems()
        {
            return Items;
        }

        /// <summary>
        /// 转换成定长数组，伴随着内容拷贝。
        /// 如果是值类型数组，将与本动态数组失去关联；
        /// 如果是引用类型数组，将与本动态数组保存相同的引用。
        /// </summary>
        /// <returns>数组</returns>
        public virtual Array ToArray()
        {
            var array = new T[Length];
            for (var i = 0; i < Length; i++)
            {
                array[i] = Items[i];
            }
            return array;
        }
        #endregion

        #region Enumerator
        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<T> GetEnumerator()
        {
            //搜索可用的枚举器
            var idleEnumIndex = -1;
            for (var i = 0; i < EnumStates.Length; i++)
            {
                var tryIndex = i + MayIdleIndex;
                if (!EnumStates[tryIndex])
                {
                    idleEnumIndex = tryIndex;
                    break;
                }
            }
            if (idleEnumIndex < 0)
            {
                UnityEngine.Debug.LogError("use too much enumerators");
            }
            // 标记为已经使用状态
            EnumStates[idleEnumIndex] = true;
            Enumerators[idleEnumIndex].Reset();
            // 向前迁移空闲坐标
            MayIdleIndex = (MayIdleIndex + 1) % EnumStates.Length;
            return Enumerators[idleEnumIndex];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }
        #endregion

        #region IList / ICollection
        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = (T)value;
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), (object)null);
                return _syncRoot;
            }
        }
        [NonSerialized]
        private object _syncRoot;

        bool ICollection<T>.IsReadOnly => false;

        bool IList.IsReadOnly => false;

        void IList<T>.Insert(int index, T item)
        {
            Insert(index, item);
        }

        /// <summary>
        /// 拷贝到数组
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="index">开始位置索引</param>
        public void CopyTo(Array array, int index)
        {
            Array.Copy(Items, 0, array, index, Length);
        }

        /// <summary>
        /// 拷贝到数组
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="index">开始位置索引</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(Items, 0, array, arrayIndex, Length);
        }
        #endregion

        #region Simple Enumerator
        internal struct SimpleEnumerator<TValue> : IDisposable, IEnumerator<TValue>
        {
            private readonly SimpleList<TValue> _list;
            private int _indexNext;
            private TValue _current;
            private readonly int _index;

            public object Current
            {
                get
                {
                    if (this._indexNext <= 0)
                    {
                        throw new InvalidOperationException();
                    }
                    return this._current;
                }
            }
            TValue IEnumerator<TValue>.Current => this._current;

            internal SimpleEnumerator(SimpleList<TValue> list, int id)
            {
                this._list = list;
                this._indexNext = 0;
                this._index = id;
                _current = default(TValue);
            }

            void IEnumerator.Reset()
            {
                this._indexNext = 0;
            }

            public void Dispose()
            {
                // 清除使用标记
                _list.EnumStates[_index] = false;
                _list.MayIdleIndex = _index;
            }


            public bool MoveNext()
            {
                if (_list == null)
                {
                    throw new ObjectDisposedException(base.GetType().FullName);
                }
                if (_indexNext < 0)
                {
                    return false;
                }
                if (_indexNext < _list.Count)
                {
                    _current = _list.Items[_indexNext++];
                    return true;
                }
                _indexNext = -1;
                return false;
            }
        }
        #endregion
    }
}
