using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiktokBackend.Domain.Entities
{
    public class ApiKey
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Key { get; set; }
        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
