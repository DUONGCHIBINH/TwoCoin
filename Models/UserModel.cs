namespace TwoCoinApi.Models
{
    public class UserModel
    {
        public UserModel() { }

        public UserModel(UserModel userModel)
        {
            Username = userModel.Username;
            Password = userModel.Password;
            Email = userModel.Email;
            Role = userModel.Role;
            Surname = userModel.Surname;
            GivenName = userModel.GivenName;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } = "NoEmain";
        public string Role { get; set; } = "user";
        public string Surname { get; set; } = "Surname";
        public string GivenName { get; set; } = "GivenName";

        public List<string> GetRoles
        {
            get
            {
                if (string.IsNullOrEmpty(Role)) return new List<string>();

                return Role.Split('|').ToList();
            }
        }


        public UserModel GetInfo()
        {
            return new UserModel(this) { Password = null };
        }
        public object GetJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(GetInfo());
        }
    }
}
