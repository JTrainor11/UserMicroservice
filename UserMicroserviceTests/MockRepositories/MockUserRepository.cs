using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UserMicroservice.DBContexts;
using UserMicroservice.Models;

namespace UserMicroservice.Repository
{
    public class MockUserRepository : IUserRepository
    {
        private readonly List<User> _mockUsers;

        public MockUserRepository()
        {
            _mockUsers = new List<User>()
            {
                new User() { userId = 1, username = "User One" },
                new User() { userId = 2, username = "User Two" },
                new User() { userId = 3, username = "User Three" }
            };
        }

        UserRepositoryReturnCodes IUserRepository.DeleteUser(int userId)
        {
            User? userToDelete = null;

            foreach(User user in _mockUsers)
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
            
            _mockUsers.Remove(userToDelete);
            
            return UserRepositoryReturnCodes.Success;
        }

        IActionResult IUserRepository.GetUsers()
        {
            if(_mockUsers.Count() == 0)
            {
                return new NotFoundResult();
            }

            IEnumerable<User> orderedUsers = _mockUsers.OrderBy(user => user.userId);
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

            _mockUsers.Add(newUser);

            return UserRepositoryReturnCodes.Success;
        }

        UserRepositoryReturnCodes IUserRepository.UpdateUser(int userId, string newUsername)
        {
            if (UsernameTaken(newUsername))
            {
                return UserRepositoryReturnCodes.UsernameTaken;
            }

            User? existingUser = GetUserById(userId);

            if (existingUser == null)
            {
                return UserRepositoryReturnCodes.UserNotFound;
            }
            
            existingUser.username = newUsername;
            
            return UserRepositoryReturnCodes.Success;
        }

        private bool UsernameTaken(string newUsername)
        {
            foreach (User existingUser in _mockUsers)
            {
                if (existingUser.username.ToLower().Trim() == newUsername.ToLower().Trim())
                {
                    return true;
                }
            }

            return false;
        }

        private User? GetUserById(int userId)
        {
            foreach (User existingUser in _mockUsers)
            {
                if (existingUser.userId == userId)
                {
                    return existingUser;
                }
            }

            return null;
        }

        public void Save() { } // not implemented
    }
}
