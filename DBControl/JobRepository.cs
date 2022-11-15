using JobsAPI.DTOs;
using JobsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobsAPI.DBControl
{
    public class JobRepository : IJobRepository
    {
        private readonly DBContext _context;
        public JobRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<bool> JobAlreadyExists(JobForCreateDTO jobForCreateDTO,int userId)
        {
            var jobs = await _context.Jobs.Where(job => job.UserId == userId).ToListAsync();
            foreach(var job in jobs)
            {
                if(job.UserId == userId)
                {
                    if(job.Position == jobForCreateDTO.Position && job.Company == jobForCreateDTO.Company && job.Status == jobForCreateDTO.Status)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public async Task<JobForReturnDTO> CreateJob(JobForCreateDTO job,int UserId)
        {
            if(job.Company != null && job.Position != null && job.Status != null)
            {
                Job job2 = new Job();
                job2.Position = job.Position;
                job2.Status = job.Status;
                job2.Company = job.Company;
                job2.UserId = UserId;

                await _context.Jobs.AddAsync(job2);
                await _context.SaveChangesAsync();

                JobForReturnDTO jobToReturn = new JobForReturnDTO();
                jobToReturn.Id = job2.Id;
                jobToReturn.Position = job.Position;
                jobToReturn.Status = job.Status;
                jobToReturn.Company = job.Company;
                jobToReturn.UserId = UserId;

                return jobToReturn;
            }
            else
            {
                return null;
            }

        }

        public async Task<string> DeleteJob(int jobId,int userId)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);
            if(job.UserId != userId)
            {
                return "Unauthorized";
            }
            if(job == null)
            {
                return "Job Not Found";
            }
            else
            {
                var userid = job.UserId;
                var user = await _context.Users.FindAsync(userid);
                user.Jobs.Remove(job);
                _context.Jobs.Remove(job);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return "Job Deleted Succesfully.!!";
                }
                else
                {
                    return "Failed";
                }
            }
        }

        public async Task<JobForReturnDTO> GetJob(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return null;
            }
            else
            {

                JobForReturnDTO jobToReturn = new JobForReturnDTO();
                jobToReturn.Id = jobId;
                jobToReturn.Company = job.Company;
                jobToReturn.Position = job.Position;
                jobToReturn.Status = job.Status;
                jobToReturn.UserId = job.UserId;
                return jobToReturn;
            }
        }

        public async Task<List<JobForReturnDTO>> GetJobs(int userId)
        {
            var user = await _context.Users.Include(j => j.Jobs).FirstOrDefaultAsync(x => x.Id == userId);
            var jobs = new List<JobForReturnDTO>();
            foreach(var job in user.Jobs)
            {
                var jobToReturn = new JobForReturnDTO();
                jobToReturn.Id = job.Id;
                jobToReturn.Company = job.Company;
                jobToReturn.Position = job.Position;
                jobToReturn.Status = job.Status;
                jobToReturn.UserId = job.UserId;
                jobs.Add(jobToReturn);
            }
            if (jobs.Count == 0)
            {
                return null;
            }
            else
            {
                return jobs;
            }
        }

        public async Task<JobForReturnDTO> UpdateJob(int jobId, int userId,JobForUpdateDTO jobForUpdate)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if(job == null || job.UserId != userId)
            {
                return null;
            }
            else
            {

                job.Status = jobForUpdate.Status;
                job.Position = jobForUpdate.Position;
                job.Company = jobForUpdate.Company;

                await _context.SaveChangesAsync();
                var jobToReturn = new JobForReturnDTO();
                jobToReturn.Status = job.Status;
                jobToReturn.Id = job.Id;
                jobToReturn.Position = job.Position;
                jobToReturn.Company = job.Company;
                jobToReturn.UserId = job.UserId;
                return jobToReturn;
            }
        }
    }
}
