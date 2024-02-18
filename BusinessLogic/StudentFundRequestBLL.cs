using DataAccess.Entity;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Net;

namespace BusinessLogic
{
    public class StudentFundRequestBLL
    {
        private readonly StudentFundRequestDAL _repository;

        public StudentFundRequestBLL(StudentFundRequestDAL repository)
        {
            _repository = repository;
        }

        public IEnumerable<StudentFundRequest> GetAllRequests()
        {
            try
            {
                return _repository.GetAllRequests();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving student fund requests: {ex.Message}");
            }
        }

        public void Create(Models.CreateStudentFundRequestForNewStudent newRequest)
        {
            if (newRequest != null)
                try
                {
                    // Convert the business logic model to the data access model
                    CreateStudentFundRequestForNewStudent dataAccessModel = new()
                    {
                        IDNumber = newRequest.IDNumber,
                        FirstName = newRequest.FirstName,
                        LastName = newRequest.LastName,
                        Email = newRequest.Email,
                        PhoneNumber = newRequest.PhoneNumber,
                        GenderName = newRequest.GenderName,
                        RaceName = newRequest.RaceName,
                        UniversityID = newRequest.UniversityID,
                        BirthDate = newRequest.BirthDate,
                        Grade = newRequest.Grade,
                        Amount = newRequest.Amount
                    };

                    _repository.Create(dataAccessModel);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating student fund request", ex);
                }
            else
                throw new ArgumentNullException(nameof(newRequest));
        }

        public void UpdateRequest(int id, Models.UpdateStudentFundRequest newRequest)
        {
            if (newRequest == null)
                throw new ArgumentNullException(nameof(newRequest));

            try
            {
                UpdateStudentFundRequest updatedRequest = new()
                {
                    Grade = newRequest.Grade,
                    Amount = newRequest.Amount
                };
                _repository.UpdateRequest(id, updatedRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating student fund request: {ex.Message}");
            }
        }

        public void ApproveApplication(int applicationId)
        {
            try
            {
                _repository.UpdateApplicationStatus(applicationId, 1, ""); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error approving application: {ex.Message}");
            }
        }

        public void RejectApplication(int applicationId,string comment)
        {
            try
            {
                _repository.UpdateApplicationStatus(applicationId, 2, comment); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error rejecting application: {ex.Message}");
            }
        }

        
    }
}
