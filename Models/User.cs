using System.ComponentModel.DataAnnotations;

namespace JobsAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // passwordHash --> it will be hashed password
        public byte[] passwordHash { get; set; }

        // passwordSalt --> it will act as key
        public byte[] passwordSalt { get; set; }

        public ICollection<Job> Jobs { get; set; }

    }
}
