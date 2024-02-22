using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Job;
using backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }

        public JobController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD

        // Create
        [HttpPost]
        [Route("Create")]

        public async Task<IActionResult> CreateJob([FromBody] JobCreateDto dto)
        {
            Job newJob = _mapper.Map<Job>(dto);
            await _context.Jobs.AddAsync(newJob);
            await _context.SaveChangesAsync();

            return Ok("Job created successfully");
        }

        // Read
        [HttpGet]
        [Route("Get")]

        public async Task<ActionResult<IEnumerable<JobGetDto>>> GetJob()
        {
            var jobs = await _context.Jobs.Include(job => job.Company).ToListAsync();
            var convertedJob = _mapper.Map<IEnumerable<JobGetDto>>(jobs);

            return Ok(convertedJob);
        }
    }
}
