using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;

namespace TwoCoinApi.Services
{
    public class UserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<UserModel> GetAll()
        {
            return _userRepository.GetAll();
        }
        public UserModel GetById(string Username)
        {
            return _userRepository.GetById(Username);
        }
        public UserModel Add(UserModel user)
        {
            return _userRepository.Add(user);
        }
        public void Update(UserModel user)
        {
            _userRepository.Update(user);
        }
        public void Delete(string Username)
        {
            _userRepository.Delete(Username);
        }
        public UserModel CheckLogin(LoginModel loginModel)
        {
            var curUser = _userRepository.GetAll().SingleOrDefault(o => o.Username.ToLower() == loginModel.Username.ToLower() && o.Password == loginModel.Password);
            return curUser == null ? null : curUser.GetInfo();

        }
    }
}
