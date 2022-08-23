using TwoCoinApi.Models;

namespace TwoCoinApi.Repos.Interface
{
    public interface IUserRepository
    {
        public List<UserModel> GetAll();
        public UserModel GetById(string Username);
        public UserModel Add(UserModel user);
        public void Update(UserModel user);
        public void Delete(string Username);
    }
}
