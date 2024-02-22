using backend.Core.Enums;

namespace backend.Core.Dtos.Company
{
    public class CompanyGetDto
    {
        public long ID { get; set; }

        public string Name { get; set; } = null!;

        public CompanySize Size { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
