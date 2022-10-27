using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UserMicroservice.DBContexts;
using UserMicroservice.Models;

namespace UserMicroservice.Repository
{
    public class UserRepository : IUserRepository
    {
        private UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        UserRepositoryReturnCodes IUserRepository.DeleteUser(int userId)
        {
            User userToDelete = null;

            foreach(User user in _userContext.Users)
            {
                if(user.userId == userId)
                {
                    userToDelete = user;
                    break;
                }
            }

            if (userToDelete == null)
            {
                return UserRepositoryReturnCodes.UserNotFound;
            }
            
            _userContext.Users.Remove(userToDelete);
            Save();
            
            return UserRepositoryReturnCodes.Success;
        }

        IActionResult IUserRepository.GetUsers()
        {
            if(_userContext.Users.Count() == 0)
            {
                return new NotFoundResult();
            }

            IEnumerable<User> orderedUsers = _userContext.Users.OrderBy(user => user.userId);
            OkObjectResult result = new OkObjectResult(orderedUsers);

            return result;
        }

        UserRepositoryReturnCodes IUserRepository.InsertUser(string newUsername)
        {
            if (UsernameTaken(newUsername))
            {
                return UserRepositoryReturnCodes.UsernameTaken;
            }

            User newUser = new User();
            newUser.username = newUsername;

            _userContext.Users.Add(newUser);
            Save();

            return UserRepositoryReturnCodes.Success;
        }

        public void Save()
        {
            _userContext.SaveChanges();
        }

        UserRepositoryReturnCodes IUserRepository.UpdateUser(int userId, string newUsername)
        {
            if (UsernameTaken(newUsername))
            {
                return UserRepositoryReturnCodes.UsernameTaken;
            }

            User existingUser = GetUserById(userId);

            if (existingUser == null)
            {
                return UserRepositoryReturnCodes.UserNotFound;
            }
            
            existingUser.username = newUsername;
            Save();
            
            return UserRepositoryReturnCodes.Success;
        }

        private bool UsernameTaken(string newUsername)
        {
            foreach (User existingUser in _userContext.Users)
            {
                if (existingUser.username.ToLower().Trim() == newUsername.ToLower().Trim())
                {
                    return true;
                }
            }

            return false;
        }

        private User GetUserById(int userId)
        {
            foreach (User existingUser in _userContext.Users)
            {
                if (existingUser.userId == userId)
                {
                    return existingUser;
                }
            }

            return null;
        }
    }
}
