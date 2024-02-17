using DataAccess.Entity;
using DataAccess;
using System;
using System.Collections.Generic;

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
            if (newRequest == null)
                throw new ArgumentNullException(nameof(newRequest));

            try
            {
                // Convert the business logic model to the data access model
                CreateStudentFundRequestForNewStudent dataAccessModel = new CreateStudentFundRequestForNewStudent
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
        }

        //public void UpdateRequest(int id, StudentFundRequest updatedRequest)
        //{
        //    if (updatedRequest == null)
        //        throw new ArgumentNullException(nameof(updatedRequest));

        //    try
        //    {
        //        _repository.UpdateRequest(id, updatedRequest);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error updating student fund request: {ex.Message}");
        //    }
        //}

        public void ApproveApplication(int applicationId)
        {
            try
            {
                _repository.UpdateApplicationStatus(applicationId, 1); // 1 indicates "Approved"
            }
            catch (Exception ex)
            {
                throw new Exception($"Error approving application: {ex.Message}");
            }
        }

        public void RejectApplication(int applicationId)
        {
            try
            {
                _repository.UpdateApplicationStatus(applicationId, 2); // 2 indicates "Rejected"
            }
            catch (Exception ex)
            {
                throw new Exception($"Error rejecting application: {ex.Message}");
            }
        }

        
    }
}
