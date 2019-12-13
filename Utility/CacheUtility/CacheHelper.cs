
using Utility.config;

namespace Utility.CacheUtility
{
    public class CacheHelper
    {
        private static readonly bool _isReids = BaseComm.GetAppSettings("isReids", "false") == "true";
        public static BaseCache GetInstrance()
        {          
      
            if (_isReids)
            {
                return new ApiMemoryCache();
            }
            else
            {
                return new ApiRedisCache();
            }
        }
    }
    
}