using BusinessLogic;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFundRequestController : ControllerBase
    {
        private readonly StudentFundRequestBLL _StudentFundRequestBLL;

        public StudentFundRequestController(StudentFundRequestBLL StudentFundRequestBLL)
        {
            _StudentFundRequestBLL = StudentFundRequestBLL;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StudentFundRequest>> GetAllRequests()
        {
            try
            {
                var requests = _StudentFundRequestBLL.GetAllRequests();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving student fund requests: {ex.Message}");
            }
        }

        [HttpPost("CreateStudentFundRequestForNewStudent")]
        public ActionResult Create([FromBody] CreateStudentFundRequestForNewStudent newRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _StudentFundRequestBLL.Create(newRequest);
                return Ok("Student fund request created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating student fund request: {ex.Message}");
            }
        }



        //[HttpPut("{id}")]
        //public ActionResult UpdateRequest(int id, [FromBody] StudentFundRequest updatedRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        _StudentFundRequestBLL.UpdateRequest(id, updatedRequest);
        //        return Ok("Student fund request updated successfully!");
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound("Student fund request not found!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating student fund request: {ex.Message}");
        //    }
        //}

        [HttpPost("{applicationId}/approve")]
        public ActionResult ApproveApplication(int applicationId)
        {
            try
            {
                _StudentFundRequestBLL.ApproveApplication(applicationId);
                return Ok("Application approved successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error approving application: {ex.Message}");
            }
        }

        [HttpPost("{applicationId}/reject")]
        public ActionResult RejectApplication(int applicationId)
        {
            try
            {
                _StudentFundRequestBLL.RejectApplication(applicationId);
                return Ok("Application rejected successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error rejecting application: {ex.Message}");
            }
        }
    }
}