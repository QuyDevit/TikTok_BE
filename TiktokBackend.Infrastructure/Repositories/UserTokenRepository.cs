﻿using Microsoft.EntityFrameworkCore;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;
using TiktokBackend.Infrastructure.Persistence;

namespace TiktokBackend.Infrastructure.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public UserTokenRepository(AppDbContext context, IJwtService jwtService)
        {
            _jwtService = jwtService; _context = context;
        }

        public async Task AddOrUpdateUserTokenAsync(Guid userId, string rfToken, string ipAddress, string userAgent,string deviceId)
        {
            var findUserToken = await _context.UserTokens.FirstOrDefaultAsync(x =>x.UserId == userId && x.IpAddress == ipAddress && x.UserAgent == userAgent);
            if (findUserToken == null) { 
                var newUserToken = new UserToken
                {
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    RefreshToken = rfToken,
                    DeviceId = deviceId,
                    ExpiryDate = DateTime.UtcNow.AddDays(7),
                };
                await _context.UserTokens.AddAsync(newUserToken);
            }
            else
            {
                findUserToken.RefreshToken = rfToken;
                findUserToken.DeviceId = deviceId;
            }
        }

        public async Task<UserToken?> RefreshTokenAsync(string token, string deviceId)
        {
            var userRefreshToken = await _context.UserTokens.FirstOrDefaultAsync(t=>t.RefreshToken == token && t.DeviceId == deviceId);
            if(userRefreshToken == null)
                return null;
            if ( userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                _context.UserTokens.Remove(userRefreshToken);
                return null;
            }          
            userRefreshToken.RefreshToken = _jwtService.GenerateRefreshToken();
            userRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
            userRefreshToken.DeviceId = deviceId;
            return userRefreshToken;
        }

        public async Task<bool> RemoveRefreshTokenAsync(Guid userId,string token)
        {
            var userRefreshToken = await _context.UserTokens.FirstOrDefaultAsync(t => t.RefreshToken == token && t.UserId == userId);

            if (userRefreshToken == null) return false;

            _context.UserTokens.Remove(userRefreshToken);
            return true;
        }
    }
}
