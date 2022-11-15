using JobsAPI.DTOs;
using JobsAPI.Models;
namespace JobsAPI.DBControl
{
    public interface IJobRepository
    {
        public Task<JobForReturnDTO> CreateJob(JobForCreateDTO job,int UserId);
        public Task<JobForReturnDTO> UpdateJob(int jobId,int userId,JobForUpdateDTO job);
        public Task<JobForReturnDTO> GetJob(int jobId);
        public Task<List<JobForReturnDTO>> GetJobs(int userId);

        public Task<string> DeleteJob(int jobId,int userId);

        public Task<bool> JobAlreadyExists(JobForCreateDTO job, int UserId);

    }
}
