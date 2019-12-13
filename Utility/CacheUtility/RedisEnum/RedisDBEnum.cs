using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.CacheUtility.RedisEnum{
   
        /// <summary>
        /// Redis缓存数据库
        /// </summary>
        public enum RedisDBEnum
        {
            /// <summary>
            /// 第一个库
            /// </summary>
            One,

            /// <summary>
            /// 第二个库
            /// </summary>
            Two,

            /// <summary>
            /// 第三个库
            /// </summary>
            Three,

            /// <summary>
            /// 第四个库
            /// </summary>
            Four,

            /// <summary>
            /// 第五个库
            /// </summary>
            /// 
            Five,

            /// <summary>
            /// 第六个库
            /// </summary>
            Six,

            /// <summary>
            /// 第七个库
            /// </summary>
            Seven,

            /// <summary>
            /// 第八个库
            /// </summary>
            Eight,

            /// <summary>
            /// 第九个库
            /// </summary>
            Nine,

            /// <summary>
            /// 第十个库
            /// </summary>
            Ten,

            /// <summary>
            /// 第十一个库
            /// </summary>
            Eleven,

            /// <summary>
            /// 第十二个库
            /// </summary>
            Twelve,

            /// <summary>
            /// 第十三个库
            /// </summary>
            Thirteen,

            /// <summary>
            /// 第十四个库
            /// </summary>
            Fourteen,

            /// <summary>
            /// 第十五个库
            /// </summary>
            Fifteen,

            /// <summary>
            /// 第十六个库
            /// </summary>
            Sixteen,

            /// <summary>
            /// 配置文件指定的库
            /// </summary>
            Default
        }

    /// <summary>
    /// Redis缓存文件夹，多层嵌套示例：[Description("一级目录:二级目录:三级目录")]
    /// </summary>
    public enum RedisFolderEnum
    {
        /// <summary>
        /// 根目录
        /// </summary>
        [Description("")]
        Root,

        /// <summary>
        /// 测试目录1
        /// </summary>
        [Description("fd1")]
        Folder1,

        /// <summary>
        /// 测试目录2
        /// </summary>
        [Description("fd2")]
        Folder2,

        /// <summary>
        /// 测试目录3
        /// </summary>
        [Description("fd3")]
        Folder3,

        /// <summary>
        /// 测试目录4
        /// </summary>
        [Description("fd4")]
        Folder4,

        /// <summary>
        /// 测试目录5
        /// </summary>
        [Description("fd5")]
        Folder5,

        /// <summary>
        /// 测试目录6
        /// </summary>
        [Description("fd6")]
        Folder6,

    }
}
