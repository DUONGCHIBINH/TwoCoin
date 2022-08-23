using TwoCoinApi.Models;

namespace TwoCoinApi.Repos.Interface
{
    public interface ITokenRepository
    {
        bool Add(RedisModel redisModel);

        bool AddAndCheckToken(UserModel user, TokenModel token, string freshTokenOld);
        IEnumerable<RedisModel> GetAll();
        RedisModel? GetByKey(string key);
        bool Delete(string key);
    }
}