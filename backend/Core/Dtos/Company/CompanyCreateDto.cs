using backend.Core.Enums;

namespace backend.Core.Dtos.Company
{
    public class CompanyCreateDto
    {
        public string Name { get; set; } = null!;

        public CompanySize Size { get; set; }
    }
}
