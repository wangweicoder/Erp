using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.CacheUtility
{
    public abstract class BaseCache
    {
        /// <summary>
        /// 设置缓存，使用默认过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public abstract bool Set(string key, string value);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="min">分钟</param>
        /// <returns></returns>
        public abstract bool Set(string key, string value, int min);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public abstract string Get(string key);
        /// <summary>
        /// 设置缓存，使用默认过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public abstract bool Set<T>(string key, T value);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="min">分钟</param>
        /// <returns></returns>
        public abstract bool Set<T>(string key, T value, int min);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <returns></returns>
        public abstract T Get<T>(string key) where T : class;
        /// <summary>
        /// 判断key是否存在（缓存是否存在）
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public abstract bool KeyExists(string key);
        /// <summary>
        /// 设置key过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">分钟</param>
        /// <returns></returns>
        public abstract bool KeyExpire(string key, int min);
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public abstract bool KeyDelete(string key);
        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        public abstract long KeyDelete(string[] keys);
    }
    public class ApiRedisCache : BaseCache
    {
        public override bool Set(string key, string value)
        {
            return RedisHelper.StringSet(key, value, 20);
        }

        public override bool Set(string key, string value, int min)
        {
            return RedisHelper.StringSet(key, value, min);
        }

        public override string Get(string key)
        {
            return RedisHelper.StringGet(key);
        }

        public override bool Set<T>(string key, T value)
        {
            return RedisHelper.StringSet<T>(key, value, 20);
        }

        public override bool Set<T>(string key, T value, int min)
        {
            return RedisHelper.StringSet<T>(key, value, min);

        }

        public override T Get<T>(string key)
        {
            return RedisHelper.StringGet<T>(key);

        }

        public override bool KeyExists(string key)
        {
            return RedisHelper.KeyExists(key);
        }

        public override bool KeyExpire(string key, int min)
        {
            return RedisHelper.KeyExpire(key, min);
        }

        public override bool KeyDelete(string key)
        {
            try
            {
                RedisHelper.KeyDelete(key);
            }
            catch { }
            return true;
        }

        public override long KeyDelete(string[] keys)
        {
            try
            {
                return RedisHelper.KeyDelete(keys.ToList());
            }
            catch { return 0; }
        }
    }
    public class ApiMemoryCache : BaseCache
    {
        private string source = "ApiMemoryCache";
        public override bool Set(string key, string value)
        {
            try
            {
                Caching.Set(source, key, value, 20);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override bool Set(string key, string value, int min)
        {
            try
            {
                Caching.Set(source, key, value, min);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override string Get(string key)
        {
            return Caching.Get(source, key)?.ToString();
        }

        public override bool Set<T>(string key, T value)
        {
            try
            {
                Caching.Set(source, key, value, 20);
            }
            catch { return false; }
            return true;
        }

        public override bool Set<T>(string key, T value, int min)
        {
            try
            {
                Caching.Set(source, key, value, min);
            }
            catch { return false; }
            return true;
        }

        public override T Get<T>(string key)
        {
            return Caching.Get<T>(source, key);
        }

        public override bool KeyExists(string key)
        {
            return Caching.Contains(source, key);
        }

        public override bool KeyExpire(string key, int min)
        {
            try
            {
                if (Caching.Contains(source, key))
                {
                    Caching.Set(source, key, Caching.Get(source, key), min);
                }
            }
            catch { return false; }
            return true;
        }

        public override bool KeyDelete(string key)
        {
            return Caching.Remove(source, key);
        }

        public override long KeyDelete(string[] keys)
        {
            long count = 0;
            foreach (string key in keys)
            {
                if (Caching.Remove(source, key))
                    count++;
            }
            return count;
        }
    }

}
