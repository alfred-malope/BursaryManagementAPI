using BursaryManagementAPI.DataAccess.DAO;
using BursaryManagementAPI.Models.DataModels;
using BursaryManagementAPI.Models.DTO;
using BursaryManagementAPI.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BursaryManagementAPI.Authentication
{
    public class UserManager
    {
        private UserManager<IdentityUser> _userManager;
        private UserDAO _userDAO;
        private ContactsDAO _contactsDAO;
        public UserManager(UserManager<IdentityUser> userManager, UserDAO userDAO, ContactsDAO contactsDAO) {
            _contactsDAO = contactsDAO;
            _userManager = userManager;
            _userDAO = userDAO;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterDTO model)
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
                int contactId = _contactsDAO.InsertContactsAndGetPrimaryKey(contacts);

                User user = new User
                {
                    ContactID = contactId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                int userId = _userDAO.InsertUserAndGetPrimaryKey(user);

                

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
