using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserInfo> Login(string username, string password)
        {
            try
            {
                var user = await _context.UserInfo.FirstOrDefaultAsync(x => x.UserName == username);
                if(user == null){
                    return null;
                }
                if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
                    return null;
            return user;
            }
            catch (System.Exception)
            {
                throw new System.NotImplementedException();
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            try
            {
                using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if(computedHash[i] != passwordHash[i]) return false;
                    }
                }
                return true;
            }
            catch (System.Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<UserInfo> Register(UserInfo user, string password)
        {
            try
            {
                byte[] passwordHash,passwordSalt;
                CreatePasswordHash(password,out passwordHash,out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _context.UserInfo.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception)
            {
                throw new System.NotImplementedException();
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            try
            {
                using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                }

            }
            catch (System.Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                if(await _context.UserInfo.AnyAsync(x => x.UserName == username))
                    return true;
            
                return false;
                 
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}