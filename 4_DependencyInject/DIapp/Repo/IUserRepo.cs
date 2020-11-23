using System.Collections.Generic;
using DIapp.Model;

namespace DIapp.Repo
{
    public interface IUserRepo
    {
        List<User> GetUser();
        void AddUser(User user);

    }
}