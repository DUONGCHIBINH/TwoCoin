using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;

namespace TwoCoinApi.Services
{
    public class UserRepositoryConst : IUserRepository
    {
        public UserModel Add(UserModel user)
        {
            //cần check tồn tại
            Users.Add(user);
            return user.GetInfo();
        }
        public void Delete(string Username)
        {
            Users = Users.FindAll(u => u.Username != Username);
        }
        public List<UserModel> GetAll()
        {
            return Users;
        }
        public UserModel? GetById(string Username)
        {
            return Users.SingleOrDefault(u => u.Username == Username);
        }

        public void Update(UserModel user)
        {
            var cantim = Users.SingleOrDefault(u => u.Username == user.Username);
            if (cantim != null)
            {
                cantim = user;
            }
        }

        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel()
            {
                Username = "admin",
                Password= "1",
                Role = "admin|seller"
            },
            new UserModel()
            {
                Username = "user",
                Password= "1",
                Role = "user"
            }
        };
    }
}
