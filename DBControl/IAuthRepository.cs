using JobsAPI.DTOs;
using JobsAPI.Models;
namespace JobsAPI.DBControl
{
    public interface IAuthRepository
    {
        Task<UserForReturnDTO> Register(UserForRegisterDTO userForRegisterDTO);
        Task<User> Login(UserForLoginDTO userForLoginDTO);
        Task<bool> UserExists(string email);
    }
}
