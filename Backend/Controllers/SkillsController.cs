using BusinessLogic.Entities;
using BusinessLogic.databaseContext;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public SkillsController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Skill>> GetSkills()
        {
            var skills = _dbContext.Skills.ToList();
            return Ok(skills);
        }

        [HttpPost]
        public ActionResult<Skill> CreateSkill(Skill skill)
        {
            _dbContext.Skills.Add(skill);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetSkillById), new { id = skill.Skillid }, skill);
        }

        [HttpGet("{id}")]
        public ActionResult<Skill> GetSkillById(int id)
        {
            var skill = _dbContext.Skills.FirstOrDefault(s => s.Skillid == id);
            if (skill == null)
            {
                return NotFound();
            }
            return Ok(skill);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSkill(int id, Skill updatedSkill)
        {
            var skill = _dbContext.Skills.FirstOrDefault(s => s.Skillid == id);
            if (skill == null)
            {
                return NotFound();
            }
            skill.Name = updatedSkill.Name;
            skill.Professionalarea = updatedSkill.Professionalarea;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSkill(int id)
        {
            var skill = _dbContext.Skills.FirstOrDefault(s => s.Skillid == id);
            if (skill == null)
            {
                return NotFound();
            }
            _dbContext.Skills.Remove(skill);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}

