using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Job_Candidate_API.Models;

namespace Job_Candidate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CandidatesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCandidate(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                var isSaved = await _context.Candidates
                    .FirstOrDefaultAsync(c => c.Email == candidate.Email);

                if (isSaved == null)
                {
                    _context.Candidates.Add(candidate);
                }
                else
                {
                    isSaved.FirstName = candidate.FirstName;
                    isSaved.LastName = candidate.LastName;
                    isSaved.PhoneNumber = candidate.PhoneNumber;
                    isSaved.PreferredContactTime = candidate.PreferredContactTime;
                    isSaved.LinkedIn = candidate.LinkedIn;
                    isSaved.GitHub = candidate.GitHub;
                    isSaved.Comments = candidate.Comments;
                }

                await _context.SaveChangesAsync();
                return Ok(candidate);
            }

            return BadRequest(ModelState);
        }
    }
}

