
using BusinessLogic.Models;
using BusinessLogic.Models.Response;
using DataAccess;
using DataAccess.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogic
{
    public class UserBLL
    {
        private UserManager<IdentityUser> _userManager;
        private UserDAL _userDAL;
        private ContactsDAL _contactsDAL;
        private IConfiguration _configuration;
        public UserBLL(UserManager<IdentityUser> userManager, UserDAL userDAL, ContactsDAL contactsDAL, IConfiguration configuration)
        {
            _configuration = configuration;
            _contactsDAL = contactsDAL;
            _userManager = userManager;
            _userDAL = userDAL;
            _configuration = configuration;
        }

        public async Task<UserManagerResponse> LoginUserAsync(Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email Address",
                    isSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (result == false)
            {
                return new UserManagerResponse
                {
                    Message = "Incorrect Password",
                    isSuccess = false,
                };
            }

            Claim[] claims = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));


            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {

            };
        }

        public async Task<UserManagerResponse> RegisterUserAsync(Register model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if(model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Passwords do not match",
                    isSuccess = false
                };
            }

            var applicationUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);

            if(result.Succeeded)
            {
                ContactDetails contacts = new ContactDetails { 
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };
                int contactId = _contactsDAL.InsertContactsAndGetPrimaryKey(contacts);

                User user = new User
                {
                    ContactID = contactId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                int userId = _userDAL.InsertUserAndGetPrimaryKey(user);

                

                return new UserManagerResponse
                {
                    Message = "User created",
                    isSuccess = true
                };
            }


            return new UserManagerResponse
            {
                Message = "User not created",
                isSuccess = false,
                Errors = result.Errors.Select(error => error.Description)
            };
        }
    }
}
