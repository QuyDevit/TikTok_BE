using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Domain.Entities
{
    public class VideoMeta
    {
        [Key]
        public Guid VideoId { get; set; } 

        public long FileSize { get; set; }
        public string FileFormat { get; set; } = string.Empty;
        public string PlaytimeString { get; set; } = string.Empty;
        public double PlaytimeSeconds { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        public virtual Video Video { get; set; }
    }
}
