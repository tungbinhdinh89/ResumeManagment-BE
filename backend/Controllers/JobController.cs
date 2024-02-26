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
            var jobs = await _context.Jobs.Include(job => job.Company).OrderByDescending(j => j.CreatedAt).ToListAsync();
            var convertedJob = _mapper.Map<IEnumerable<JobGetDto>>(jobs);

            return Ok(convertedJob);
        }

        // Update

        // Delete
        [HttpDelete]
        [Route("Delete/{id}")]

        public async Task<ActionResult> DeleteJob(long id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if(job == null)
            {
                return NotFound("Job not found");
            }

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return Ok("Job delete successfully");
        }

    }
}
