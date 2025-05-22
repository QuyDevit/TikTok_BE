using Microsoft.EntityFrameworkCore;
using Nest;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;
using BCryptHelper = BCrypt.Net.BCrypt;


namespace TiktokBackend.Infrastructure.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<User> AddUserAsync(User entity)
        {
            entity.Id = Guid.NewGuid();
            entity.Password = BCryptHelper.HashPassword(entity.Password);
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            entity.FullName = $"user{timestamp}";
            entity.Nickname = $"user{timestamp}";
            await _context.Users.AddAsync(entity);
            return entity;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var result = await _context.Users.FindAsync(id);
            if (result is null)
            {
                return null;
            }
            return result;
        }
        public async Task<List<User>> GetListUserByNameAsync(string query)
        {
            var result = await _context.Users.AsNoTracking()
                .Where(u => u.FullName
                .Contains(query)).ToListAsync();
            return result;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var result = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return result;
        }

        public async Task<User?> GetUserByPhoneAsync(string phone)
        {
            var result = await _context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phone);
            return result;
        }

        public async Task<User?> UpdateUserNameByIdAsync(Guid id, string nickname)
        {
            var result = await _context.Users.FindAsync(id);
            if(result is null)
            {
                return null;
            }
            result.Nickname = nickname;
            return result;
        }

        public async Task<bool> CheckUserNameByIdAsync(Guid id,string nickname)
        {
            var result = await _context.Users.AnyAsync(s=>s.Nickname == nickname && s.Id != id);
            return result;
        }

        public async Task<User?> ValidateUserCaseEmailAsync(string username, string password)
        {
            var  user = await _context.Users.FirstOrDefaultAsync(n => (n.Email ==username || n.Nickname == username));

            if (user != null && BCryptHelper.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<User?> ValidateUserCasePhoneAsync(string phone, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.PhoneNumber == phone);

            if (user != null && BCryptHelper.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<User?> GetUserByNickNameAsync(string nickname)
        {
            var result = await _context.Users.SingleOrDefaultAsync(u => u.Nickname == nickname);
            return result;
        }

        public async Task<User?> UpdateUserProfileByIdAsync(Guid id, string nickname, string fullname, string bio, string avatar)
        {
            var result = await _context.Users.FindAsync(id);
            if (result is null)
            {
                return null;
            }
            string[] path = fullname.Split(' ');

            if (path.Length > 1)
            {
                result.FirstName = string.Join(" ", path, 0, path.Length - 1); 
                result.LastName = path[path.Length - 1]; 
            }
            else
            {
                result.FirstName = path[0]; 
                result.LastName = string.Empty; 
            }
            result.Nickname = nickname;
            result.FullName = fullname;
            result.Bio = bio;
            if (!string.IsNullOrEmpty(avatar))
            {
                result.Avatar = avatar;
            }
            return result;
        }

        public async Task<User> UpdateFollowingsCount(Guid userId,bool isFollow)
        {
            var result = await _context.Users.FindAsync(userId);
            if (result == null)
            {
                throw new ArgumentException("User not found.");
            }
            result.FollowingsCount += isFollow ? 1 : -1;
            return result;
        }

        public async Task<User> UpdateFollowersCount(Guid userId, bool isFollow)
        {
            var result = await _context.Users.FindAsync(userId);
            if (result == null)
            {
                throw new ArgumentException("User not found.");
            }
            result.FollowersCount += isFollow ? 1 : -1;

            return result;
        }
    }
}
