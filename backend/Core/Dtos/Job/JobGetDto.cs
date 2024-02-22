using backend.Core.Entities;
using backend.Core.Enums;

namespace backend.Core.Dtos.Job
{
    public class JobGetDto
    {
        public long ID { get; set; }

        public string Title { get; set; } = null!;

        public JobLevel Level { get; set; }

        public long CompanyId { get; set; }

        public string CompanyName { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
