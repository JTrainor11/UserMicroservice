using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Transactions;
using UserMicroservice.Models;
using UserMicroservice.Repository;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("users")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets all users from the User database.
        /// </summary>
        /// <returns>All users in the User database</returns>
        /// <response code="200">Returns all users.</response>
        /// <response code="404">The database is empty - no users returned</response>
        [HttpGet]
        public IActionResult Get()
        {
            IActionResult result = _userRepository.GetUsers();

            if(result.GetType() == typeof(NotFoundResult))
            {
                return Problem("There are no users in the database.", null, 404, "Database is empty");
            }

            return result;
        }

        /// <summary>
        /// Inserts a user into the User database.
        /// </summary>
        /// <param name="userName"></param>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     POST /users
        ///     {
        ///         "username": "New User"
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">The user was successfully created.</response>
        /// <response code="409">There is already a user with the specified username.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(string userName)
        {
            UserRepositoryReturnCodes insertReturnCode = _userRepository.InsertUser(userName);

            if(insertReturnCode == UserRepositoryReturnCodes.UsernameTaken)
            {
                return Problem("Username is already in use.", null, 409);
            }

            return new CreatedResult("", null);
        }

        /// <summary>
        /// Updates an existing user in the User database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newUsername"></param>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     PUT /users
        ///     {
        ///         "userId": 1
        ///         "username": "Changed User"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">The user was successfully updated.</response>
        /// <response code="404">The user doesn't exist in the database.</response>
        /// <response code="409">There is already a user with the specified username.</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, string newUsername)
        {
            UserRepositoryReturnCodes updateReturnCode = _userRepository.UpdateUser(id, newUsername);

            if(updateReturnCode == UserRepositoryReturnCodes.UsernameTaken)
            {
                return Problem("Username is already in use.", null, 409);
            }
            else if(updateReturnCode == UserRepositoryReturnCodes.UserNotFound)
            {
                return Problem("User not found.", null, 404);
            }

            return new OkResult();
        }

        /// <summary>
        /// Deletes a user from the User database.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// 
        /// Sample request:
        /// 
        ///     DELETE /users
        ///     {
        ///         "userId": 1
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">The user was successfully deleted.</response>
        /// <response code="404">The user doesn't exist in the database.</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UserRepositoryReturnCodes deleteReturnCode = _userRepository.DeleteUser(id);

            if (deleteReturnCode == UserRepositoryReturnCodes.UserNotFound)
            {
                return Problem("User not found.", null, 404);
            }

            return new OkResult();
        }
    }
}
