using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Candidate;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }
        public CandidateController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //CRUD

        //Create
        [HttpPost]
        [Route("Create")]

        public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateDto dto, IFormFile pdfFile)
        {
            // First => save pdf to server
            // Then => save url into our entity

            var fiveMegaByte = 5 * 1024 * 1024;
            var pdfMineType = "application/pdf";

            if (pdfFile.Length > fiveMegaByte || pdfFile.ContentType != pdfMineType)
            {
                return BadRequest("File is not valid");
            }

            var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
            var filePatch = Path.Combine(Directory.GetCurrentDirectory(), "documents", "pdfs", resumeUrl);
            using (var stream = new FileStream(filePatch, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            var newCandidate = _mapper.Map<Candidate>(dto);
            newCandidate.ResumeUrl = resumeUrl;
            await _context.AddAsync(newCandidate);
            await _context.SaveChangesAsync();

            return Ok("Candidate create successfully");
        }

        // Read
        [HttpGet]
        [Route("Get")]

        public async Task<ActionResult<IEnumerable<CandidateGetDto>>> GetCandidates()
        {
            var candidates = await _context.Candidates.Include(candidate => candidate.Job).ToListAsync();
            var convertedCandidates = _mapper.Map<IEnumerable<CandidateGetDto>>(candidates);

            return Ok(convertedCandidates);
        }

        // Read (Download Pdf File)
        [HttpGet]
        [Route("download/{url}")]

        public IActionResult DownloadPdfFile(string url)
        {
            var filePatch = Path.Combine(Directory.GetCurrentDirectory(), "documents", "pdfs", url);

            if (!System.IO.File.Exists(filePatch))
            {
                return NotFound("File Not Found");
            }

            var pdfBytes = System.IO.File.ReadAllBytes(filePatch);
            var file = File(pdfBytes, "application/pdf", url);
            return file;
        }

        // Read (Get Job By ID)
        [HttpGet]
        [Route("job/{jobId}")]
        public async Task<IActionResult> GetCandidatesByJobId(int jobId)
        {
            var candidates = await _context.Candidates
                                           .Include(candidate => candidate.Job)
                                           .Where(candidate => candidate.JobId == jobId)
                                           .ToListAsync();

            var convertedCandidates = _mapper.Map<IEnumerable<CandidateGetDto>>(candidates);

            return Ok(convertedCandidates);
        }

        // Update
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateCandidate(long id, [FromForm] CandidateCreateDto dto, IFormFile pdfFile)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound("Candidate not found");
            }

            if (pdfFile != null)
            {
                // Validate and save pdf to server
                // Update resumeUrl if pdf is valid
                var fiveMegaByte = 5 * 1024 * 1024;
                var pdfMineType = "application/pdf";

                if (pdfFile.Length > fiveMegaByte || pdfFile.ContentType != pdfMineType)
                {
                    return BadRequest("File is not valid");
                }

                var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
                var filePatch = Path.Combine(Directory.GetCurrentDirectory(), "documents", "pdfs", resumeUrl);
                using (var stream = new FileStream(filePatch, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }

                candidate.ResumeUrl = resumeUrl;
            }

            // Update other properties if needed
            _mapper.Map(dto, candidate);
            _context.Update(candidate);
            await _context.SaveChangesAsync();

            return Ok("Candidate updated successfully");
        }

        // Delete
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteCandidate(long id)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound("Candidate not found");
            }

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();

            return Ok("Candidate deleted successfully");
        }
    }
}
