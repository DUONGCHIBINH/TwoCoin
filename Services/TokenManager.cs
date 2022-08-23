using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace TwoCoinApi.Services
{
    public class TokenManager
    {
        private readonly ITokenRepository _repo;

        public TokenManager(ITokenRepository repo)
        {
            _repo = repo;
        }
        public bool Add(RedisModel redisModel)
        {
            return _repo.Add(redisModel);
        }

        public IEnumerable<RedisModel> GetAll()
        {
            return _repo.GetAll();
        }

        public RedisModel? GetByKey(string key)
        {
            return _repo.GetByKey(key);
        }

        public bool AddAndCheckToken(UserModel user, TokenModel token, string freshTokenOld)
        {
            return _repo.AddAndCheckToken(user, token, freshTokenOld);
        }

        public bool IsExitsAccessToken(string userID, string token)
        {
            try
            {
                var tokenModel = JsonConvert.DeserializeObject<List<TokenModel>>(GetByKey(userID)?.Value);
                if (!tokenModel.Any(o => o.accesstoken == token)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsExitsRefreshToken(string userId, string token)
        {
            try
            {
                var tokenModel = JsonConvert.DeserializeObject<List<TokenModel>>(GetByKey(userId)?.Value);
                if (!tokenModel.Any(o => o.accesstoken == token)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal bool Delete(string user)
        {
            return _repo.Delete(user);
        }
    }
}
