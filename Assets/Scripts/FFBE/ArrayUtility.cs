/**************************************************************************/
/*! @file   ArrayUtility.cs
    @brief  配列ユーティリティ
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;


    //=========================================================================
    //. 配列ユーティリティ
    //=========================================================================
    public static class ArrayUtility
    {
        /// ***********************************************************************
        /// <summary>
        /// 安全な配列アクセス
        /// </summary>
        /// ***********************************************************************
        public static T GetElementSafe<T>( T[] list, int index )
        {
            if( list != null & index >= 0 && index < list.Length )
            {
                return list[ index ];
            }
            return default(T);
        }

        /// <summary>Array範囲外ならnullを返す</summary>
        [return: MaybeNull]
        public static T GetOrNull<T>( this T[] array, int index ) where T : class
        {
            if ( array == null || (uint)index >= (uint)array.Length )
            {
                return null;
            }
            return array[index];
        }

        /// ***********************************************************************
        /// <summary>
        /// 検索
        /// </summary>
        /// ***********************************************************************
        public static int FindIndex<T>( T[] list, System.Func<T,bool> equals )
        {
            if( list != null )
            {
                for( int i = 0; i < list.Length; ++i )
                {
                    if( list[ i ] != null )
                    {
                        if( equals( list[ i ] ) )
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
        
        /// ***********************************************************************
        /// <summary>
        /// 安定ソート
        /// </summary>
        /// ***********************************************************************
        public static void StableSort<T>( T[] list, System.Comparison<T> comparison )
        {
            List<KeyValuePair<int,T>> wrapped = new List<KeyValuePair<int,T>>(list.Length);
            for( int i = 0; i < list.Length; i++ )
            {
                wrapped.Add( new KeyValuePair<int,T>( i, list[i] ) );
            }
            
            wrapped.Sort( delegate( KeyValuePair<int,T> x, KeyValuePair<int,T> y )
                {
                    int result = comparison( x.Value, y.Value );
                    if( result == 0 )
                    {
                        result = x.Key.CompareTo( y.Key );
                    }
                    return result;
                });
            
            for( int i = 0; i < list.Length; i++ )
            {
                list[i] = wrapped[i].Value;
            }
        }
        
        /// ***********************************************************************
        /// <summary>
        /// 安定ソート(逆順)
        /// </summary>
        /// ***********************************************************************
        public static void StableSortReverse<T>( T[] list, System.Comparison<T> comparison )
        {
            List<KeyValuePair<int,T>> wrapped = new List<KeyValuePair<int,T>>(list.Length);
            for( int i = 0; i < list.Length; i++ )
            {
                wrapped.Add( new KeyValuePair<int,T>( i, list[i] ) );
            }
            
            wrapped.Sort( delegate( KeyValuePair<int,T> x, KeyValuePair<int,T> y )
                {
                    int result = comparison( y.Value, x.Value );
                    if( result == 0 )
                    {
                        result = x.Key.CompareTo( y.Key );
                    }
                    return result;
                });
            
            for( int i = 0; i < list.Length; i++ )
            {
                list[i] = wrapped[i].Value;
            }
        }
        
        /// ***********************************************************************
        /// <summary>
        /// マージ
        /// </summary>
        /// ***********************************************************************
        [return: MaybeNull]
        public static T[] Merge<T>( T[] list1, T[] list2, bool isSameEnable = true /*同一データの許可フラグ*/ )
        {
            T[] result = list1;
            if( list2 != null )
            {
                if( result == null )
                {
                    result = list2;
                }
                else
                {
                    List<T> tmp = new List<T>( result );
                    for( int i = 0; i < list2.Length; ++i )
                    {
                        if( isSameEnable || tmp.FindIndex( ( p ) => p.Equals( list2[ i ] ) ) == -1 )
                        {
                            tmp.Add( list2[ i ] );
                        }
                    }
                    result = tmp.Count > 0 ? tmp.ToArray( ): null;
                }
            }
            return result;
        }

        /// ***********************************************************************
        /// <summary>
        /// デリゲートの戻り値が引数と一致する要素を検索.
        /// デリゲート変数キャプチャを防ぐことで内部的なstaticキャッシュを促し
        /// コールのたびにデリゲートのアロケーションが発生するのを防ぐための、Array.Find,Linq.Firstの変形.
        /// </summary>
        /// ***********************************************************************
        [return: MaybeNull]
        public static TItem FindEquals<TItem, TKey>( this TItem[] array, Func<TItem, TKey> func, TKey key )
        {
            if (array == null)
            {
                return default(TItem);
            }

            var comparer = EqualityComparer<TKey>.Default;

            // array.Lengthはキャッシュせずforの第2句に素直に書く
            // こうするとブロック内でのarray[i]の表現で内部のnullチェックが消える
            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (comparer.Equals(func(item), key))
                {
                    return item;
                }
            }

            return default(TItem);
        }

        /// ***********************************************************************
        /// <summary>
        /// FindEquals string特化
        /// </summary>
        /// ***********************************************************************
        [return: MaybeNull]
        public static TItem FindEquals<TItem>( this TItem[] array, Func<TItem, string> func, string key )
        {
            if (array == null)
            {
                return default;
            }

            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (func(item) == key)
                {
                    return item;
                }
            }

            return default;
        }

        /// ***********************************************************************
        /// <summary>
        /// デリゲートの戻り値が引数と一致する要素を検索.
        /// デリゲート変数キャプチャを防ぐことで内部的なstaticキャッシュを促し
        /// コールのたびにデリゲートのアロケーションが発生するのを防ぐための、Linq.Firstの変形.
        /// </summary>
        /// ***********************************************************************
        [return: MaybeNull]
        public static int FindIndexEquals<TItem, TKey>( this TItem[] array, Func<TItem, TKey> func, TKey key )
        {
            if (array == null)
            {
                return -1;
            }

            var comparer = EqualityComparer<TKey>.Default;

            int n = 0;
            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (comparer.Equals(func(item), key))
                {
                    return n;
                }

                ++ n;
            }

            return -1;
        }

        public static bool Any<TItem>(this TItem[] array, Func<TItem, bool> func)
        {
            if (array == null)
            {
                return false;
            }

            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (func(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AnyEquals<TItem, TKey>(this TItem[] array, Func<TItem, TKey> func, TKey key)
        {
            if (array == null)
            {
                return false;
            }

            var comparer = EqualityComparer<TKey>.Default;
            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (comparer.Equals(func(item), key))
                {
                    return true;
                }
            }

            return false;
        }

        /// ***********************************************************************
        /// <summary>
        /// nullなら空配列を返す
        /// </summary>
        /// ***********************************************************************
        [return: NotNull]
        public static T[] OrEmpty<T>( [AllowNull] this T[] array )
        {
            if ( array ==null ) return Array.Empty<T>();
            return array;
        }

        public static void ZeroClear<T>(this T[] array)
        {
            Array.Clear(array, 0, array.Length);
        }

        public static void DisposeAll<T>(this T[] array) where T : IDisposable
        {
            if (array == null)
                return;

            for (var i = 0; i < array.Length; ++i)
            {
                var target = array[i];
                if (target == null)
                    continue;
                target.Dispose();
                array[i] = default;
            }
        }

        public static T[] WhereToArray<T>([AllowNull] this List<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                return Array.Empty<T>();

            var sourceCount = list.Count;
            if (sourceCount == 0)
                return Array.Empty<T>();

            var temp = ArrayPool<T>.Shared.Rent(sourceCount);

            var count = 0;
            for (var i = 0; i < sourceCount; ++i)
            {
                var item = list[i];
                if (predicate(item))
                {
                    temp[count++] = item;
                }
            }
            if (count == 0)
            {
                ArrayPool<T>.Shared.Return(temp, true);
                return Array.Empty<T>();
            }
            if (count == temp.Length)
                return temp;
            var dest = new T[count];
            Array.Copy(temp, dest, count);
            ArrayPool<T>.Shared.Return(temp, true);
            return dest;
        }

        /// <summary>二分検索</summary>
        [return: MaybeNull]
        public static T BinarySearch<T, TKey>(this T[] array, Func<T, TKey> key, TKey value)
        {
            var left = -1;
            var right = array.Length;
            var comparer = Comparer<TKey>.Default;

            while (right - left > 1)
            {
                var mid = (left + right) / 2;
                var target = array[mid];
                var cmp = comparer.Compare(key(target), value);

                if (cmp > 0) { right = mid; }
                else if (cmp < 0) { left = mid; }
                else { return target; }
            }

            return default;
        }

        /// <summary>二分検索.string特化.</summary>
        [return: MaybeNull]
        public static T BinarySearch<T>(this T[] array, Func<T, string> key, string value)
        {
            if (array == null)
                Utility.Throw(new ArgumentNullException(nameof(array)));

            var left = -1;
            var right = array.Length;

            while (right - left > 1)
            {
                var mid = (left + right) / 2;
                var target = array[mid];
                var cmp = key(target).CompareToFast(value);

                if (cmp > 0) { right = mid; }
                else if (cmp < 0) { left = mid; }
                else { return target; }
            }

            return default;
        }

        [return: MaybeNull]
        public static int BinarySearchIndex<T, TKey>(this T[] array, Func<T, TKey> key, TKey value)
        {
            var left = -1;
            var right = array.Length;
            var comparer = Comparer<TKey>.Default;

            while (right - left > 1)
            {
                var mid = (left + right) / 2;
                var target = array[mid];
                var cmp = comparer.Compare(key(target), value);

                if (cmp > 0) { right = mid; }
                else if (cmp < 0) { left = mid; }
                else { return mid; }
            }

            // 見つからない場合は、以上かつ最小のindexをビット反転して返す(必ず負の数).
            return ~right;
        }

        public static T[] SortBy<T, TKey>(this T[] array, Func<T, TKey> keyFunc)
        {
            var count = array.Length;
            var keys = ArrayPool<TKey>.Shared.Rent(count);
            var items = ArrayPool<T>.Shared.Rent(count);
            for (var i = 0; i < count; ++i)
            {
                items[i] = array[i];
                keys[i] = keyFunc(array[i]);
            }

            Array.Sort(keys, items, 0, count);
            
            for (var i = 0; i < count; ++i)
            {
                array[i] = items[i];
            }
            ArrayPool<TKey>.Shared.Return(keys);
            ArrayPool<T>.Shared.Return(items, true);

            return array;
        }

        public static void SortBy<T>(this T[] array, Func<T, string> keyFunc)
        {
            var count = array.Length;
            var keys = ArrayPool<string>.Shared.Rent(count);
            var items = ArrayPool<T>.Shared.Rent(count);
            for (var i = 0; i < count; ++i)
            {
                var item = array[i];
                items[i] = item;
                keys[i] = keyFunc(item);
            }

            Array.Sort(keys, items, 0, count, StringUtility.StringOrdinalComparer.Shared);
            for (var i = 0; i < count; ++i)
            {
                array[i] = items[i];
            }
            ArrayPool<string>.Shared.Return(keys);
            ArrayPool<T>.Shared.Return(items, true);
        }

        public static TItem[] StableSortBy<TItem,TKey>(this TItem[] array, Func<TItem, TKey> func)
        {
            var tmp = ListPool<KeyValuePair<int, TItem>>.Rent();
            var count = array.Length;
            for (var i = 0; i < count; ++i)
            {
                tmp.Add(new KeyValuePair<int, TItem>(i, array[i]));
            }

            var pool = InstancePool<StableFuncComaparer<TItem, TKey>>.Shared;
            var comparer = pool.Rent() ?? new StableFuncComaparer<TItem, TKey>();
            comparer.Func = func;
            tmp.Sort(comparer);
            comparer.Func = null;
            pool.Return(comparer);

            for (var i = 0; i < count; ++i)
            {
                array[i] = tmp[i].Value;
            }
            ListPool.Return(ref tmp);

            return array;
        }

        sealed class StableFuncComaparer<TItem, TKey> : IComparer<KeyValuePair<int, TItem>>
        {
            readonly Comparer<TKey> keyComparer = Comparer<TKey>.Default;
            public Func<TItem, TKey> Func;

            public int Compare(KeyValuePair<int, TItem> x, KeyValuePair<int, TItem> y)
            {
                var cmp = keyComparer.Compare(Func(x.Value), Func(y.Value));
                if (cmp != 0)
                {
                    return cmp;
                }
                return x.Key - y.Key;
            }
        }


       
        public static T Find<T>(this T[] array, Predicate<T> predicate)
        {
            if (array == null || predicate == null)
                return default;

            return Array.Find(array, predicate);
        }

        
        public static T Find<T, TState>(this T[] array, Func<T, TState, bool> predicate, TState state)
        {
            if (array == null || predicate == null)
                return default;

            for (var i = 0; i < array.Length; ++i)
            {
                var item = array[i];
                if (predicate(item, state))
                {
                    return item;
                }
            }
            return default;
        }

        public static T First<T>(this T[] array, Predicate<T> predicate)
        {
            if (array == null || predicate == null)
                throw new ArgumentNullException();

            var index = Array.FindIndex(array, predicate);
            if (index < 0)
                throw new InvalidOperationException("unmatched");

            return array[index];
        }

        public static T[] ToArrayOrEmpty<T>( this List<T> list)
        {
            if (list == null || list.Count == 0)
                return Array.Empty<T>();  // Array.Empty<T>()はアロケーション起きない

            return list.ToArray();
        }

        /// <summary>Linq.Contains(IEnumerable)のキャストアロケーションを防ぐ</summary>
        public static bool Contains<T>(this T[] array, T value)
        {
            if (array == null)
                Utility.Throw(new ArgumentNullException(nameof(array)));

            return Array.IndexOf(array, value) != -1;
        }
    }

