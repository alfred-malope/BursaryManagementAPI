﻿using DataAccess.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess
{
    public class UserDAL(SqlConnection connection, UserManager<IdentityUser> userManager)
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SqlConnection _connection = connection;


        //public getRoleIdByName(string name)
        //{
        //    _connection.Open();
        //    string query = "SELECT ID FROM [dbo].[Role] WHERE RoleType=@Role";
        //    using (SqlCommand command = new SqlCommand(query, _connection))
        //    {
        //        try
        //        {
        //            command.Parameters.AddWithValue()
        //        }
        //    }
        //}
        public int GetRoleIdByName(string roleName)
        {
            _connection.Open();
            string query = "SELECT ID FROM [dbo].[Role] WHERE RoleType=@RoleName";
            using(SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@RoleName", roleName);
                    int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                    _connection.Close();
                    return primaryKey;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to retirve Role ID: {ex.Message}");
                    return 0;
                }
            }
        }

        public int InsertToUserRole(int UserId, string RoleName)
        {   
            int roleId = GetRoleIdByName(RoleName);
            _connection.Open();
            string query = "INSERT INTO [dbo].[UserRole] (UserID, RoleID) VALUES (@UserID, @RoleID)";
            using(SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@UserID", UserId);
                    command.Parameters.AddWithValue("@RoleID", roleId);
                    int rowsAffected = command.ExecuteNonQuery();
                    _connection.Close();
                    return rowsAffected;
                }
                catch(Exception ex)
                {
                    _connection.Close();
                    Console.WriteLine($"Unable to add UserRole: {ex.Message}");
                    return 0;
                }
            }

        }

        public Task<IList<System.Security.Claims.Claim>> getUserClaims(IdentityUser user)
        {
            return _userManager.GetClaimsAsync(user);
        }
        
        //Returns null if no user exists
        public Task<IdentityUser?> checkUserByEmail(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<IList<string>> getUserRoles(IdentityUser user) {
            return _userManager.GetRolesAsync(user);
        }

        public async Task<bool> checkUserPassword(IdentityUser user,string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> RegisterIdentityUser(IdentityUser applicationUser, string password, string role)
        {
            var result = await _userManager.CreateAsync(applicationUser, password);
            await _userManager.AddToRoleAsync(applicationUser, role);
            return result;
        }


            public int InsertUserAndGetPrimaryKey(User user)
        {
            _connection.Open();
            string query = "INSERT INTO [dbo].[User] (FirstName, LastName, ContactID) VALUES (@FirstName, @LastName, @ContactID); SELECT SCOPE_IDENTITY()";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@ContactID", user.ContactID);


                    // Execute the INSERT statement and retrieve ID
                    int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"User ID: '{primaryKey}'");
                    _connection.Close();
                    return primaryKey;
                }
                catch(Exception e)
                {   _connection.Close();
                    Console.WriteLine($"Unable to insert to User Table. Details: '{e.Message}'");
                    throw e;
                }
                finally { _connection.Close(); }


            }


        }

        public int InsertContactsAndGetPrimaryKey(ContactDetails contactDetails)
        {
            _connection.Open();
            string query = "INSERT INTO ContactDetails (Email, PhoneNumber) VALUES (@Email, @PhoneNumber); SELECT SCOPE_IDENTITY()";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    Console.WriteLine($"Contacts: {contactDetails.Email} {contactDetails.PhoneNumber}");
                    command.Parameters.AddWithValue("@Email", contactDetails.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", contactDetails.PhoneNumber);
                    Console.WriteLine(command.CommandText);
                    // Execute the INSERT statement and retrieve ID
                    int primaryKey = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Contact ID: '{primaryKey}'");
                    _connection.Close();
                    return primaryKey;
                }
                catch (Exception ex)
                {
                    _connection.Close();
                    Console.WriteLine($"Unable to insert to ContactDetails Table. Details: '{ex.StackTrace}'");
                    throw ex;
                }
                finally { _connection.Close(); }

            }
        }

        public User getUserByEmail(string email)
        {
            _connection.Open();
            User user = new User();

            string query = "SELECT usr.ID, FirstName, LastName,ContactDetails.ID, PhoneNumber, Email FROM [dbo].[User] as usr INNER JOIN ContactDetails ON ContactDetails.Email=@Email AND ContactDetails.ID = usr.ContactID";
            using(SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {  command.Parameters.AddWithValue("@Email", email);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new User
                        {
                            ID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            ContactID = reader.GetInt32(3),
                        };

                    }
                }
                catch (Exception e)
                {
                    _connection.Close();
                    throw e;
                }
            }
            _connection.Close();
            return user;
        }
    }
}
