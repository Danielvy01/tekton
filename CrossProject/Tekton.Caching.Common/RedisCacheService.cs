using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Tekton.RedisCaching
{
    public class RedisCacheService : IRedisCacheService
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IDistributedCache _distributedCache;


        /// <summary>
        /// RedisCacheService
        /// </summary>
        /// <param name="distributedCache"></param>
        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetData<T>(string key)
        {
            var value = _distributedCache.GetString(key);

            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        /// <summary>
        /// SetData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lifeTimeMinutes"></param>
        public void SetData<T>(string key, T value, int lifeTimeMinutes)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(lifeTimeMinutes)
            };
            _distributedCache.SetString(key, JsonConvert.SerializeObject(value), options);
        }

    }
}
