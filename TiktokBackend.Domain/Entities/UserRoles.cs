using System.ComponentModel.DataAnnotations.Schema;

namespace TiktokBackend.Domain.Entities
{
    public class UserRoles
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
