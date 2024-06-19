namespace Tekton.RedisCaching
{
    public interface IRedisCacheService
    {
        /// <summary>
        /// GetData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetData<T>(string key);

        /// <summary>
        /// SetData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lifeTimeMinutes"></param>
        void SetData<T>(string key, T value, int lifeTimeMinutes);
    }
}
