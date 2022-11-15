namespace JobsAPI.DTOs
{
    public class JobForReturnDTO
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }
        public int UserId { get; set; }
    }
}
