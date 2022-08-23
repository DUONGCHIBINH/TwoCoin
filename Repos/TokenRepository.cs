using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace TwoCoinApi.Services
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConnectionMultiplexer _redis;
        public TokenRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public bool Add(RedisModel redisModel)
        {
            try
            {
                if (redisModel == null) return false;
                var db = _redis.GetDatabase();
                var serialModel = JsonConvert.SerializeObject(redisModel);
                db.StringSet(redisModel.Key, redisModel.Value);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AddAndCheckToken(UserModel user, TokenModel token, string freshTokenOld)
        {
            try
            {
                if (user == null || token == null || string.IsNullOrEmpty(user.Username)) return false;

                var db = _redis.GetDatabase();
                var redisExit = GetByKey(user.Username);
                if (redisExit == null)
                {
                    RedisModel newRedis = new RedisModel()
                    {
                        Key = user.Username,
                        Tokens = new List<TokenModel>() { token },
                    };
                    Add(newRedis);
                }
                else //add new token to Tokes
                {
                    if (redisExit?.Tokens == null)
                    {
                        redisExit.Tokens = new List<TokenModel>() { token };
                    }
                    else
                    {
                        var lstToken = redisExit.Tokens;
                        if (freshTokenOld != "")
                        {
                            if (!lstToken.Any(o => o.freshtoken == freshTokenOld)) return false;//không chứa token cũ thì không cho cấp
                            lstToken = lstToken.FindAll(o => o.freshtoken != freshTokenOld); //xóa token cũ
                        }
                        lstToken.Add(token);
                        redisExit.Tokens = lstToken;
                    }
                    db.StringSet(user.Username, redisExit.Value);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(string key)
        {
            var db = _redis.GetDatabase();
            return db.KeyDelete(key);
        }

        public IEnumerable<RedisModel> GetAll()
        {
            var a = _redis.GetServer(_redis.GetEndPoints().First()).Keys().ToList();
            var b = _redis.GetServer("host");
            return null;
        }

        public RedisModel? GetByKey(string key)
        {
            var db = _redis.GetDatabase();
            var model = db.StringGet(key);
            if (!string.IsNullOrEmpty(model))
            {
                return new RedisModel()
                {
                    Key = key,
                    Value = model,
                };
            }
            return null;
        }

    }
}