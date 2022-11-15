using System.ComponentModel.DataAnnotations.Schema;

namespace JobsAPI.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }

        
        public User User { get; set; }

        public int UserId { get; set; }

    }
}
