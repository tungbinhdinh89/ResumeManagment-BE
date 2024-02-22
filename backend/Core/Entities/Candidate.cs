namespace backend.Core.Entities
{
    public class Candidate : BaseEntity
    {

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string CoverLetter { get; set; } = null!;

        public string ResumeUrl { get; set; } = null!;

        // Relations

        public long JobId { get; set; }

        public Job Job { get; set; } = null!;
    }
}
