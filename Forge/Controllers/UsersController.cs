using System.Diagnostics;
using ErrorOr;
using Forge.Contracts.Users;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services;
using Forge.Services.Products;
using Forge.Services.Supplies;
using Forge.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult CreateUser(CreateUserRequest request)
        {
            ErrorOr<User> requestToUserResult = Models.User.From(request);

            if (requestToUserResult.IsError)
            {
                return Problem(requestToUserResult.Errors);
            }

            var user = requestToUserResult.Value;
            ErrorOr<ErrorOr.Created> createUserResult = _userService.CreateUser(user);

            return createUserResult.Match(
                created => CreatedAtGetUser(user),
                errors => Problem(errors)
            );

        }


        [HttpGet("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUser(int id)
        {
            ErrorOr<User> getUserResult = _userService.GetUser(id);
            return getUserResult.Match(
                Supply => Ok(MapUserResponse(Supply)),
                errors => Problem(errors)
            );
        }

        [HttpGet()]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
            ErrorOr<List<User>> getUsersResult = _userService.GetUsers();
            return getUsersResult.Match(
                Users => Ok(MapUsersResponses(Users)),
                errors => Problem(errors)
            );
        }


        [HttpPut("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult UpsertUser(int id, CreateUserRequest request)
        {
            ErrorOr<User> requestToUserResult =  Models.User.From(id, request);

            if (requestToUserResult.IsError){
                return Problem(requestToUserResult.Errors);
            }

            var user = requestToUserResult.Value;

            ErrorOr<UpsertedRecord> upsertedUserResult = _userService.UpsertUser(user);

            return upsertedUserResult.Match(
                upserted => upserted.isNewlyCreated ? CreatedAtGetUser(user) : NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            ErrorOr<Deleted> deleteUserResult = _userService.DeleteUser(id);
            return deleteUserResult.Match(
                deleted => NoContent(),
                errors => Problem(errors)
            );
        }

        private static List<UserResponse> MapUsersResponses(List<User> users){
            List<UserResponse> responses = new ();
            foreach(var user in users){
                responses.Add(MapUserResponse(user));
            }
            return responses;
        }

        private static UserResponse MapUserResponse(User user)
        {
            return new UserResponse(
                user.Id,
                user.Email,
                user.Role,
                user.RoleId
            );
        }
        private IActionResult CreatedAtGetUser(User user)
        {
            return CreatedAtAction(
                actionName: nameof(GetUser),
                routeValues: new { id = user.Id },
                value: MapUserResponse(user));
        }
    }

}
