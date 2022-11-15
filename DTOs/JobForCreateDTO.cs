
using JobsAPI.Models;

namespace JobsAPI.DTOs
{
    public class JobForCreateDTO
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }

    }
}
