﻿using BusinessLogic;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BursaryManagementAPI.Controllers
{
    /// <summary>
    /// The student fund request controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFundRequestController(StudentFundRequestBLL StudentFundRequestBLL) : ControllerBase
    {
        private readonly StudentFundRequestBLL _StudentFundRequestBLL = StudentFundRequestBLL;

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

        [HttpPost("create")]
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

        [HttpPut("update/{id}")]
        public ActionResult UpdateRequest(int id, [FromBody] UpdateStudentFundRequest updatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _StudentFundRequestBLL.UpdateRequest(id, updatedRequest);
                return Ok("Student fund request updated successfully!");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Student fund request not found!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating student fund request: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.BBDAdmin)]
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

        [Authorize(Roles = Roles.BBDAdmin)]
        [HttpPost("{applicationId}/reject")]
        public ActionResult RejectApplication(int applicationId, string comment)
        {
            try
            {
                _StudentFundRequestBLL.RejectApplication(applicationId, comment);
                return Ok("Application rejected successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error rejecting application: {ex.Message}");
            }
        }
    }
}