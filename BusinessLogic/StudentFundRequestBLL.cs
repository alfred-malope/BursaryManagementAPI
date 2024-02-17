using DataAccess.Entity;
using DataAccess;
using System;
using System.Collections.Generic;
using BursaryManagementAPI;

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

        public void Create(StudentFundRequest newRequest)
        {
            if (newRequest == null)
                throw new ArgumentNullException(nameof(newRequest));

            try
            {
                _repository.Create(newRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating student fund request: {ex.Message}");
            }
        }

        public void UpdateRequest(int id, StudentFundRequest updatedRequest)
        {
            if (updatedRequest == null)
                throw new ArgumentNullException(nameof(updatedRequest));

            try
            {
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
