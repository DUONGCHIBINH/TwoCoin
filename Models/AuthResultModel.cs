namespace TwoCoinApi.Models
{
    public class AuthResultModel
    {
        public UserModel user { get; set; }
        public TokenModel tokens { get; set; }

        public DateTime expires { get; set; }
    }
}
