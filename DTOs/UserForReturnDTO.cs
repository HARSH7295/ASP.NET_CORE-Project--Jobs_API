using JobsAPI.Models;

namespace JobsAPI.DTOs
{
    public class UserForReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}
