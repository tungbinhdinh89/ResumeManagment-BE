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

        public async Task<IActionResult> CreateCandidate([FromBody] CandidateCreateDto dto, IFormFile pdfFile)
        {
            // First => save pdf to server
            // Then => save url into our entity

            var fiveMegaByte = 5 * 1024 * 1024;
            var pdfMineType = "application/json";

            if (pdfFile.Length > fiveMegaByte || pdfFile.ContentType != pdfMineType)
            {
                return BadRequest("File is not valid");
            }

            var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
            var filePatch = Path.Combine(Directory.GetCurrentDirectory(), "document", "pdfs", resumeUrl);
            using (var stream = new FileStream(filePatch, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            var newCandidate = _mapper.Map<Candidate>(dto);
             await _context.AddAsync(newCandidate);
            await _context.SaveChangesAsync();

            return Ok("Candidate create successfully");
        }

        // Read
     
        // Read (Get Job By ID)

        // Update

        // Delete
    }
}
