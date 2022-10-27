using Microsoft.AspNetCore.Mvc;

namespace UserMicroservice.Repository
{
    public interface IUserRepository
    {
        IActionResult GetUsers();
        UserRepositoryReturnCodes InsertUser(string userName);
        UserRepositoryReturnCodes DeleteUser(int userId);
        UserRepositoryReturnCodes UpdateUser(int userId, string newUsername);
        void Save();
    }
}
