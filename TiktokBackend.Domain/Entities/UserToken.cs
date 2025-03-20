using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Domain.Entities
{
    public class UserToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string IpAddress { get; set; } // Địa chỉ IP của thiết bị

        public string UserAgent { get; set; } // Thông tin trình duyệt (nếu trên web)
        public string DeviceId { get; set; } 
    }
}
