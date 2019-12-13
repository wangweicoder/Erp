using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using Utility.CacheUtility.RedisEnum;
using Utility.config;

namespace Utility.CacheUtility
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public static class RedisHelper
    {
        private static string _conn =BaseComm.GetAppSettings("redis_connection_string", "127.0.0.1:6379");
        private static string _pwd = BaseComm.GetAppSettings("redis_connection_pwd","123456");
        private static int _store_db = BaseComm.GetAppSettings("redis_store_db", -1);

        static ConnectionMultiplexer _redis;
        public static object _locker = new object();
        #region 单例模式
        public static ConnectionMultiplexer Manager
        {
            get
            {
                if (_redis == null)
                {
                    lock (_locker)
                    {
                        if (_redis != null) return _redis;
                        _redis = GetManager();
                        return _redis;
                    }
                }
                return _redis;
            }
        }

        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _conn;
            }
            var options = ConfigurationOptions.Parse(connectionString);
            options.Password = _pwd;
            return ConnectionMultiplexer.Connect(options);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 获取要操作的库
        /// </summary>
        /// <param name="db">库，0和-1都是第一个库，1是第二个库...</param>
        /// <returns></returns>
        private static int GetOperationDB(RedisDBEnum db)
        {
            if (db == RedisDBEnum.Default)
            {
                return _store_db;
            }
            else
            {
                return (int)db;
            }
        }

        /// <summary>
        /// 字符串/对象转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        /// <summary>
        /// 字符串转对象/字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T ConvertObj<T>(RedisValue value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(value.ToString(), typeof(T));
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
            }
        }

        /// <summary>
        /// 批量转换对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        private static List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                if (model != null)
                    result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 获取完成key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static RedisKey[] ConvertRedisKeys(List<string> redisKeys, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
            }
            else
            {
                return redisKeys.Select(redisKey => (RedisKey)(prefix + ":" + redisKey)).ToArray();
            }
        }

        /// <summary>
        /// 获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        /// <returns>枚举的Description</returns>
        private static string GetDescription(this Enum value, Boolean nameInstead = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }
        #endregion

        #region Key（通用）
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        public static bool KeyExists(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(GetOperationDB(db)).KeyExists(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="min">过期时间，单位：分钟，-1表示不过期</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        public static bool KeyExpire(string key, int min = 600, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            try
            {
                string fd = GetDescription(folder);
                if (min == -1)
                {
                    return Manager.GetDatabase(GetOperationDB(db)).KeyExpire(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, (TimeSpan?)null);
                }
                else
                {
                    return Manager.GetDatabase(GetOperationDB(db)).KeyExpire(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, DateTime.Now.AddMinutes(min));
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 修改键
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="newKey">新键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool KeyRename(string key, string newKey, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(GetOperationDB(db)).KeyRename(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, string.IsNullOrEmpty(fd) ? newKey : fd + ":" + newKey);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool KeyDelete(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(GetOperationDB(db)).KeyDelete(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long KeyDelete(List<string> keys, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            try
            {
                string fd = GetDescription(folder);
                return Manager.GetDatabase(GetOperationDB(db)).KeyDelete(ConvertRedisKeys(keys, fd));
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region String
        /// <summary>
        /// 添加单个
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMinutes">过期时间，单位：分钟，-1表示不过期</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool StringSet(string key, string value, int expireMinutes = 600, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            if (expireMinutes == -1)
            {
                return Manager.GetDatabase(GetOperationDB(db)).StringSet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, value);
            }
            else
            {
                return Manager.GetDatabase(GetOperationDB(db)).StringSet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, value, TimeSpan.FromMinutes(expireMinutes));
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="keysStr">键</param>
        /// <param name="valuesStr">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool StringSet(string[] keysStr, string[] valuesStr, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var count = keysStr.Length;
            var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[count];
            for (int i = 0; i < count; i++)
            {
                keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(string.IsNullOrEmpty(fd) ? keysStr[i] : fd + ":" + keysStr[i], valuesStr[i]);
            }
            return Manager.GetDatabase(GetOperationDB(db)).StringSet(keyValuePair);
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="expireMinutes">过期时间，单位：分钟，-1表示不过期</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool StringSet<T>(string key, T obj, int expireMinutes = 600, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            if (expireMinutes == -1)
            {
                return Manager.GetDatabase(GetOperationDB(db)).StringSet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson(obj));
            }
            else
            {
                return Manager.GetDatabase(GetOperationDB(db)).StringSet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson(obj), TimeSpan.FromMinutes(expireMinutes));
            }
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static string StringGet(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).StringGet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
        }

        /// <summary>
        /// 获取多个
        /// </summary>
        /// <param name="keys">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static RedisValue[] StringGet(List<string> keys, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).StringGet(ConvertRedisKeys(keys, fd));
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static T StringGet<T>(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            string value = Manager.GetDatabase(GetOperationDB(db)).StringGet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            return ConvertObj<T>(value);
        }
        #endregion

        #region List
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="start">索引开始</param>
        /// <param name="stop">索引结束</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static List<T> ListRange<T>(string key, long start = 0, long stop = -1, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var value = Manager.GetDatabase(GetOperationDB(db)).ListRange(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, start, stop);
            return ConvetList<T>(value);
        }

        /// <summary>
        /// 获取指定
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static T ListGetByIndex<T>(string key, long index, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var value = Manager.GetDatabase(GetOperationDB(db)).ListGetByIndex(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, index);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 替换指定
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        public static void ListSetByIndex<T>(string key, long index, T value, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            Manager.GetDatabase(GetOperationDB(db)).ListSetByIndex(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, index, ConvertJson(value));
        }

        /// <summary>
        /// 删除指定
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="count">count > 0: Remove elements equal to value moving from head to tail.count 小于 0: Remove elements equal to value moving from tail to head.count = 0: Remove all elements equal to value.</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long ListRemove<T>(string key, T value, long count = 0, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).ListRemove(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson(value), count);
        }

        /// <summary>
        /// 指定位置之后插入
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="pivot">位置</param>
        /// <param name="value">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long ListInsertAfter<T>(string key, T pivot, T value, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).ListInsertAfter(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson(pivot), ConvertJson(value));
        }

        /// <summary>
        /// 指定位置之前插入
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="pivot">位置</param>
        /// <param name="value">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long ListInsertBefore<T>(string key, T pivot, T value, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).ListInsertBefore(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson(pivot), ConvertJson(value));
        }

        /// <summary>
        /// 入栈（后插入的在List前面）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        public static long ListLeftPush<T>(string key, T value, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).ListLeftPush(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson(value));
        }

        /// <summary>
        /// 批量入栈（后插入的在List前面）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long ListLeftPush<T>(string key, List<T> values, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var redisValues = values.Select(m => (RedisValue)ConvertJson(m)).ToArray();
            return Manager.GetDatabase(GetOperationDB(db)).ListLeftPush(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, redisValues);
        }

        /// <summary>
        /// 出栈（删除最前面的一个元素并返回）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static T ListLeftPop<T>(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var value = Manager.GetDatabase(GetOperationDB(db)).ListLeftPop(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入队（后插入的在List后面）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        public static long ListRightPush<T>(string key, T value, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).ListRightPush(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson(value));
        }

        /// <summary>
        /// 批量入队（后插入的在List后面）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="values">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long ListRightPush<T>(string key, List<T> values, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var redisValues = values.Select(m => (RedisValue)ConvertJson(m)).ToArray();
            return Manager.GetDatabase(GetOperationDB(db)).ListRightPush(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, redisValues);
        }

        /// <summary>
        /// 出队（删除最后面的一个元素并返回）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static T ListRightPop<T>(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var value = Manager.GetDatabase(GetOperationDB(db)).ListRightPop(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 获取个数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long ListLength(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).ListLength(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
        }
        #endregion

        #region Hash
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">元素的键</param>
        /// <param name="t">实体</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool HashSet<T>(string key, string dataKey, T t, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).HashSet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, dataKey, ConvertJson(t));
        }

        /// <summary>
        /// 获取特定
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">元素的键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static T HashGet<T>(string key, string dataKey, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            string value = Manager.GetDatabase(GetOperationDB(db)).HashGet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, dataKey);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKeys">元素的键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static List<T> HashGet<T>(string key, RedisValue[] dataKeys, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var value = Manager.GetDatabase(GetOperationDB(db)).HashGet(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, dataKeys);
            return ConvetList<T>(value);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static HashEntry[] HashGetAll<T>(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).HashGetAll(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
        }

        /// <summary>
        /// 删除特定
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">元素的键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool HashDelete(string key, string dataKey, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).HashDelete(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, dataKey);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKeys">元素的键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long HashDelete(string key, List<RedisValue> dataKeys, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).HashDelete(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, dataKeys.ToArray());
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">元素的键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool HashExists(string key, string dataKey, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).HashExists(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, dataKey);
        }
        #endregion

        #region Zset
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="score">排序列</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool SortedSetAdd<T>(string key, T value, double score, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).SortedSetAdd(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson<T>(value), score);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="start">索引开始</param>
        /// <param name="stop">索引结束</param>
        /// <param name="order">排序方式</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1, Order order = Order.Ascending, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var values = Manager.GetDatabase(GetOperationDB(db)).SortedSetRangeByRank(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, start, stop, order);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static bool SortedSetRemove<T>(string key, T value, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).SortedSetRemove(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, ConvertJson<T>(value));
        }

        /// <summary>
        /// 批量删除（根据对象）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">对象</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long SortedSetRemove<T>(string key, List<T> values, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            var redisValues = values.Select(m => (RedisValue)ConvertJson(m)).ToArray();
            return Manager.GetDatabase(GetOperationDB(db)).SortedSetRemove(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, redisValues);
        }

        /// <summary>
        /// 批量删除（根据score删除）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">开始</param>
        /// <param name="stop">结束</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long SortedSetRemoveRangeByScore(string key, int start, int stop, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).SortedSetRemoveRangeByScore(string.IsNullOrEmpty(fd) ? key : fd + ":" + key, start, stop);
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="folder">目录，默认根目录</param>
        /// <param name="db">库，默认读取配置文件</param>
        /// <returns></returns>
        public static long SortedSetLength(string key, RedisFolderEnum folder = RedisFolderEnum.Root, RedisDBEnum db = RedisDBEnum.Default)
        {
            string fd = GetDescription(folder);
            return Manager.GetDatabase(GetOperationDB(db)).SortedSetLength(string.IsNullOrEmpty(fd) ? key : fd + ":" + key);
        }
        #endregion

    }
}