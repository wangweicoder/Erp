using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Utility.CacheUtility
{
    /// <summary>
    /// 缓存处理类
    /// </summary>
    public static class Caching
    {
        /// <summary>
        /// 获取一个缓存对象
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <returns>缓存对象</returns>
        public static object Get(string source, string key)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();
            return MemoryCache.Default.Get(key);
        }

        /// <summary>
        /// 设置一个没有过期时间的缓存对象
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <param name="value">缓存对象</param>
        public static void Set(string source, string key, object value)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            MemoryCache.Default.Set(key, value, policy);
        }

        /// <summary>
        /// 获取一个缓存泛型值
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <returns>强类型缓存对象</returns>
        public static T Get<T>(string source, string key)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();

            object obj = MemoryCache.Default.Get(key);

            if (obj == null)
                return default(T);

            return (T)obj;
        }

        /// <summary>
        /// 获取一个缓存泛型值，如果不存在则用函数返回的值，自动加入缓存。失效时间为获取缓存对象后的<paramref name="timeOut"/>分钟。
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <param name="func">委托函数，用于获取缓存对象为空时新建缓存对象</param>
        /// <param name="timeOut">失效时间，单位为分钟，小于1时自动设置为1</param>
        /// <returns>强类型缓存对象</returns>
        public static T Get<T>(string source, string key, Func<T> func, int timeOut = 20)
        {
            T t;
            object o = Get(source, key);
            if (o is T)
            {
                t = (T)o;
            }
            else
            {
                t = func();
                //edit by coffee 2014-12-31
                //如果t为null，在插入缓存的时候会出异常
                if (t == null)
                    return default(T);
            }

            Set(source, key, t, timeOut);
            return t;
        }

        /// <summary>
        /// 设置一个没有过期时间的缓存对象
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <param name="value">缓存对象</param>
        public static void Set<T>(string source, string key, T value)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            MemoryCache.Default.Set(key, value, policy);
        }

        /// <summary>
        /// 设置一个缓存对象
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="dateExpiration">截止时间</param>
        public static void Set<T>(string source, string key, T value, DateTime dateExpiration)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();

            MemoryCache.Default.Set(key, value, dateExpiration);
        }

        /// <summary>
        /// 设置一个缓存对象， <paramref name="timeOut"/>分钟后失效
        /// </summary>
        /// <typeparam name="T">缓存对象的类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="timeOut">失效时间，单位为分钟，小于1时自动设置为1</param>
        public static void Set<T>(string source, string key, T value, int timeOut)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();

            if (timeOut < 1)
                timeOut = 1;
            MemoryCache.Default.Set(key, value, DateTime.Now.AddMinutes(timeOut));
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public static bool Contains(string source, string key)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();

            return MemoryCache.Default.Contains(key);
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public static bool Remove(string source, string key)
        {
            key = string.Format("{0}@{1}", key, source).Trim().ToLower();

            var v = MemoryCache.Default.Remove(key);
            if (v != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            MemoryCache.Default.Dispose();
        }

        /// <summary>
        /// 添加缓存，并计数
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <returns>计数+1</returns>
        public static int AddCount(string source, string key)
        {
            int hit = (int)(Get(source, key) ?? 0);

            Set(source, key, ++hit);

            return hit;
        }

        /// <summary>
        /// 添加缓存，并计数
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="key">键</param>
        /// <param name="timeOut">超时，分钟</param>
        /// <returns>计数+1</returns>
        public static int AddCount(string source, string key, int timeOut)
        {
            int hit = (int)(Get(source, key) ?? 0);

            Set(source, key, ++hit, DateTime.Now.AddMinutes(timeOut));

            return hit;
        }
    }
}
