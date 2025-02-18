﻿using BCryptHelper = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;


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
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            
            var result = await _context.Users.FindAsync(id);
            if(result is null)
            {
                return false;
            }
            _context.Users.Remove(result);
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<bool> UpdateUserAsync(Guid id, User user)
        {
            var result = await _context.Users.FindAsync(id);
            if (result is null)
            {
                return false;
            }
            result.FullName = user.FullName;
            result.Avatar = user.Avatar;
            result.Bio = user.Bio;
            result.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
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
    }
}
