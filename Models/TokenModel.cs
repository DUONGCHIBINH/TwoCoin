namespace TwoCoinApi.Models
{
    public class TokenModel
    {
        public string accesstoken { get; set; } = String.Empty;
        public string freshtoken { get; set; } = String.Empty;
        public DateTime expired { get; set; }
        public DateTime createAt { get; set; } = DateTime.Now;
        public string ipAddress { get; set; }
    }

    public class TokenParam
    {
        public string accesstoken { get; set; }
        public string freshtoken { get; set; }

    }
}
