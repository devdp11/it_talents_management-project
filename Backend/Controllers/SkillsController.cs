using BusinessLogic.Entities;
using BusinessLogic.databaseContext;
using BusinessLogic.Models;
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
        public ActionResult<IEnumerable<SkillModel>> GetSkills()
        {
            var skills = _dbContext.Skills
                .Select(s => new SkillModel 
                { 
                    SkillID = s.Skillid, 
                    Name = s.Name, 
                    ProfessionalArea = s.Professionalarea 
                })
                .ToList();

            return Ok(skills);
        }

        [HttpPost]
        public ActionResult<SkillModel> CreateSkill(SkillModel skillModel)
        {
            // Verifique se a skill já existe
            var existingSkill = _dbContext.Skills.FirstOrDefault(s => s.Name == skillModel.Name && s.Professionalarea == skillModel.ProfessionalArea);

            if (existingSkill != null)
            {
                return BadRequest("Skill with the same name and professional area already exists");
            }

            var skill = new Skill 
            { 
                Name = skillModel.Name, 
                Professionalarea = skillModel.ProfessionalArea
            };
    
            _dbContext.Skills.Add(skill);
            _dbContext.SaveChanges();

            skillModel.SkillID = skill.Skillid;
            return CreatedAtAction(nameof(GetSkillById), new { id = skillModel.SkillID }, skillModel);
        }

        [HttpGet("{id}")]
        public ActionResult<SkillModel> GetSkillById(int id)
        {
            var skill = _dbContext.Skills
                .Where(s => s.Skillid == id)
                .Select(s => new SkillModel 
                { 
                    SkillID = s.Skillid, 
                    Name = s.Name, 
                    ProfessionalArea = s.Professionalarea 
                })
                .FirstOrDefault();

            if (skill == null)
            {
                return NotFound();
            }

            return Ok(skill);
        }
        
        [HttpGet("name/{name}")]
        public ActionResult<SkillModel> GetSkillByName(string name)
        {
            var skill = _dbContext.Skills
                .Where(s => s.Name == name)
                .Select(s => new SkillModel 
                { 
                    SkillID = s.Skillid, 
                    Name = s.Name, 
                    ProfessionalArea = s.Professionalarea 
                })
                .ToList();

            if (skill == null)
            {
                return NotFound();
            }

            return Ok(skill);
        }
        
        [HttpGet("area/{professionalArea}")]
        public ActionResult<IEnumerable<SkillModel>> GetSkillsByProfessionalArea(string professionalArea)
        {
            var skills = _dbContext.Skills
                .Where(s => s.Professionalarea == professionalArea)
                .Select(s => new SkillModel 
                { 
                    SkillID = s.Skillid, 
                    Name = s.Name, 
                    ProfessionalArea = s.Professionalarea 
                })
                .ToList();

            if (skills.Count == 0)
            {
                return NotFound();
            }

            return Ok(skills);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSkill(int id, SkillModel updatedSkill)
        {
            var skill = _dbContext.Skills.FirstOrDefault(s => s.Skillid == id);
            if (skill == null)
            {
                return NotFound();
            }
    
            // Verifica se a nova combinação de nome e área profissional já existe em outra skill
            var existingSkill = _dbContext.Skills.FirstOrDefault(s => s.Name == updatedSkill.Name && s.Professionalarea == updatedSkill.ProfessionalArea && s.Skillid != id);
            if (existingSkill != null)
            {
                return BadRequest("A skill with this name and professional area already exists.");
            }

            skill.Name = updatedSkill.Name;
            skill.Professionalarea = updatedSkill.ProfessionalArea;
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

