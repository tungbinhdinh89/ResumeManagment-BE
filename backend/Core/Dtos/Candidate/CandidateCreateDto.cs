namespace backend.Core.Dtos.Candidate
{
    public class CandidateCreateDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string CoverLetter { get; set; } = null!;

        public long JobId { get; set; }

    }
}
