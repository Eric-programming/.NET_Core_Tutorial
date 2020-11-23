using System.Collections.Generic;
using DIapp.Model;

namespace DIapp.Repo
{
    public class UserRepo : IUserRepo
    {
        private List<User> _userList;
        public UserRepo()
        {
            _userList = new List<User>(){
                new User(){id = 1, Name = "Eric"},
                new User(){id = 2, Name = "Frank"},
                new User(){id = 3, Name = "Goat"},
                new User(){id = 4, Name = "Harry"},
            };
        }

        public void AddUser(User user)
        {
            _userList.Add(user);
        }

        public List<User> GetUser()
        {
            return _userList;
        }
    }
}