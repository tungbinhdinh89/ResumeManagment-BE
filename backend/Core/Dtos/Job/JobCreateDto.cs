using backend.Core.Enums;

namespace backend.Core.Dtos.Job
{
    public class JobCreateDto
    {
        public string Title { get; set; } = null!;

        public JobLevel Level { get; set; }

        public long CompanyId { get; set; }
    }
}
