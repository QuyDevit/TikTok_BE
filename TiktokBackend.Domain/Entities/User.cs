using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TiktokBackend.Domain.Entities 
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Nickname { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Avatar { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string PhoneNumber { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Bio { get; set; } = string.Empty;

        public bool Tick { get; set; } = false;

        public int FollowingsCount { get; set; } = 0;

        public int FollowersCount { get; set; } = 0;

        public int LikesCount { get; set; } = 0;

        [MaxLength(200)]
        public string WebsiteUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string FacebookUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string YoutubeUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string TwitterUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string InstagramUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [MaxLength(6)]
        public string PhoneVerificationCode { get; set; } = string.Empty;
        [MaxLength(6)]
        public string EmailVerificationCode { get; set; } = string.Empty;
        [MaxLength(6)]
        public string ResetPasswordCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

}
