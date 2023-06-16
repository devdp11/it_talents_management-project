using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Entities;
using BusinessLogic.databaseContext;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public ExperienceController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Experience>> GetExperiences()
        {
            var experiences = _dbContext.Experiences.ToList();
            return Ok(experiences);
        }

        [HttpPost]
        public ActionResult<Experience> CreateExperience(Experience experience)
        {
            _dbContext.Experiences.Add(experience);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetExperienceById), new { id = experience.Experienceid }, experience);
        }

        [HttpGet("{id}")]
        public ActionResult<Experience> GetExperienceById(int id)
        {
            var experience = _dbContext.Experiences.FirstOrDefault(e => e.Experienceid == id);
            if (experience == null)
            {
                return NotFound();
            }
            return Ok(experience);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExperience(int id, Experience updatedExperience)
        {
            var experience = _dbContext.Experiences.FirstOrDefault(e => e.Experienceid == id);
            if (experience == null)
            {
                return NotFound();
            }
            experience.Title = updatedExperience.Title;
            experience.Company = updatedExperience.Company;
            experience.Startyear = updatedExperience.Startyear;
            experience.Endyear = updatedExperience.Endyear;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExperience(int id)
        {
            var experience = _dbContext.Experiences.FirstOrDefault(e => e.Experienceid == id);
            if (experience == null)
            {
                return NotFound();
            }
            _dbContext.Experiences.Remove(experience);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("{experienceId}/professional/{professionalId}")]
        public IActionResult AssignExperienceToProfessional(int experienceId, int professionalId)
        {
            var experience = _dbContext.Experiences.FirstOrDefault(e => e.Experienceid == experienceId);
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == professionalId);

            if (experience == null || professional == null)
            {
                return NotFound();
            }

            experience.Professional = professional;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
