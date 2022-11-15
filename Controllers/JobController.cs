using System.IdentityModel.Tokens.Jwt;
using JobsAPI.DBControl;
using JobsAPI.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobsAPI.Models;
using System.Net;

namespace JobsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _repo;

        public JobController(IJobRepository repo)
        {
            _repo = repo;
        }

        // function to get userid from token
        public int GetUserIdFromToken(string AuthorizationStrFromHeader)
        {
            var token = AuthorizationStrFromHeader.Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(token).Payload;
            var UserId = Convert.ToInt32(data["nameid"]);
            return UserId;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromHeader] string Authorization,JobForCreateDTO jobForCreateDTO)
        {
            var UserId = GetUserIdFromToken(Authorization);

            if (await _repo.JobAlreadyExists(jobForCreateDTO, UserId)) { 
                return StatusCode(409,"Job Already Exists.!!!");
            }
            else
            {
                var job = await _repo.CreateJob(jobForCreateDTO, UserId);
                if (job == null) {
                    return StatusCode(500, "Job Creation Failed.!!");
                }
                return Ok(job);
            }
        }

        [HttpGet("getalljobs")]
        public async Task<IActionResult> GetAllJobs([FromHeader]string Authorization)
        {
            var UserId = GetUserIdFromToken(Authorization);

            var jobs = await _repo.GetJobs(UserId);
            if (jobs == null) {
                return StatusCode(204,"No Jobs Posted.!!!");
            }
            else
            {
                return Ok(jobs);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _repo.GetJob(id);
            if (job == null)
            {
                return StatusCode(400, "No Job by provided Id.!!!");
            }
            else
            {
                return Ok(job);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateJob([FromHeader]string Authorization,int id, JobForUpdateDTO jobForUpdateDTO)
        {
            var userId = GetUserIdFromToken(Authorization);
            var job = await _repo.UpdateJob(id,userId,jobForUpdateDTO);
            if (job == null)
            {
                return StatusCode(401, "You are not authorized to updated this job.!!!");
            }
            else
            {
                return Ok(job);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob([FromHeader]string Authorization,int id)
        {
            var userid = GetUserIdFromToken(Authorization);
            var result = await _repo.DeleteJob(id,userid);
            if(result == "Unauthorized")
            {
                return Unauthorized("You are not authorized to delete this job.!!");
            }
            else if(result == "Job Not Found")
            {
                return BadRequest("No job by provided id.!!!");
            }
            else if(result == "Failed")
            {
                return StatusCode(500, "Job deletion failed.!!");
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
