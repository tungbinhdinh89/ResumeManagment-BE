using backend.Core.Enums;

namespace backend.Core.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = null!;

        public CompanySize Size { get; set; }

        public ICollection<Job> Jobs { get; set; }

        // Relations
    }
}
