using JobsAPI.DTOs;
using JobsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobsAPI.DBControl
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DBContext _context;
        public AuthRepository(DBContext context)
        {
            _context = context;
        }
        private bool VerifyPasswordHash(string pass, byte[] passHash,byte[] passSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                for(int i=0; i<computedHash.Length; i++)
                {
                    if (computedHash[i] != passHash[i]) return false;
                }
                return true;

            }
        }
        public async Task<User> Login(UserForLoginDTO userForLoginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userForLoginDTO.email);
            if(user == null)
            {
                return null;
            }
            else
            {
                if (!VerifyPasswordHash(userForLoginDTO.password, user.passwordHash, user.passwordSalt))
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }

        }

        private void CreateHashPassword(string pass,out byte[] passHash, out byte[] passSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }

        public async Task<UserForReturnDTO> Register(UserForRegisterDTO userForRegisterDTO)
        {
            byte[] passHash, passSalt;

            CreateHashPassword(userForRegisterDTO.password, out passHash, out passSalt);
            var userObj = new User();
            userObj.Name = userForRegisterDTO.username;
            userObj.Email = userForRegisterDTO.email;
            userObj.passwordHash = passHash;
            userObj.passwordSalt = passSalt;

            await _context.Users.AddAsync(userObj);
            await _context.SaveChangesAsync();

            var userToReturn = new UserForReturnDTO();
            userToReturn.Id = userObj.Id;
            userToReturn.Name = userObj.Name;
            userToReturn.Email = userObj.Email;
            userToReturn.Jobs = userObj.Jobs;

            return userToReturn;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
