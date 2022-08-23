using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace TwoCoinApi.Models
{
    public class RedisModel
    {
        [Required]
        public string Key { get; set; } = $"redis:{Guid.NewGuid().ToString()}";

        [Required]
        public string Value { get; set; } = String.Empty;

        public List<TokenModel>? Tokens
        {
            get
            {
                return string.IsNullOrEmpty(Value)
                    ? null
                    : JsonConvert.DeserializeObject<List<TokenModel>>(Value);
            }
            set
            {
                Value = value == null
                   ? String.Empty
                   : JsonConvert.SerializeObject(value);
            }
        }
    }
}
