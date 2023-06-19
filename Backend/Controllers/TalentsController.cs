using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Models;
using BusinessLogic.Entities;
using BusinessLogic.databaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalentsController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public TalentsController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Professional>> GetProfessionals()
        {
            var professionals = _dbContext.Professionals
                .Include(p => p.ProfessionalSkills)
                .ThenInclude(ps => ps.Skill)
                .Include(p => p.Experiences)
                .ToList();

            return Ok(professionals);
        }

        [HttpPost]
        public ActionResult<CreateProfessionalModel> CreateProfessional(CreateProfessionalModel professionalModel)
        {
            // Verifica se o usuário existe
            var user = _dbContext.Users.FirstOrDefault(u => u.Userid == professionalModel.UserID);
            if (user == null)
            {
                return BadRequest("User does not exist");
            }

            // Cria uma nova entidade Professional
            var professional = new Professional 
            { 
                Userid = professionalModel.UserID, 
                Name = professionalModel.Name,
                Country = professionalModel.Country,
                Email = professionalModel.Email,
                Hourlyrate = professionalModel.HourlyRate,
                Visibility = professionalModel.Visibility,
                User = user
            };

            _dbContext.Professionals.Add(professional);
            _dbContext.SaveChanges();

            // Retorna uma resposta com o ID do novo profissional
            return CreatedAtAction(nameof(GetProfessionalById), new { id = professional.Professionalid }, professional);
        }

        [HttpGet("{id}")]
        public ActionResult<Professional> GetProfessionalById(int id)
        {
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == id);
            if (professional == null)
            {
                return NotFound();
            }
            return Ok(professional);
        }
        
        [HttpGet("skill/{skillName}")]
        public ActionResult<IEnumerable<Professional>> GetProfessionalsBySkill(string skillName)
        {
            var professionals = _dbContext.Professionals
                .Where(p => p.ProfessionalSkills.Any(ps => ps.Skill.Name.ToLower() == skillName.ToLower()))
                .ToList();

            if (professionals.Count == 0)
            {
                return NotFound();
            }

            return Ok(professionals);
        }
        
        [HttpGet("skill/{skillId}")]
        public ActionResult<IEnumerable<Professional>> GetProfessionalsBySkill(int skillId)
        {
            var professionals = _dbContext.Professionals
                .Where(p => p.ProfessionalSkills.Any(ps => ps.Skillid == skillId))
                .ToList();

            if (professionals.Count == 0)
            {
                return NotFound();
            }

            return Ok(professionals);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProfessional(int id, UpdateProfessionalModel updatedProfessional)
        {
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == id);
            if (professional == null)
            {
                return NotFound();
            }

            professional.Name = updatedProfessional.Name;
            professional.Country = updatedProfessional.Country;
            professional.Email = updatedProfessional.Email;
            professional.Hourlyrate = updatedProfessional.HourlyRate;
            professional.Visibility = updatedProfessional.Visibility;
    
            _dbContext.SaveChanges();
            return NoContent();
        }       

        [HttpDelete("{id}")]
        public IActionResult DeleteProfessional(int id)
        {
            var professional = _dbContext.Professionals
                .Include(p => p.ProfessionalSkills)
                .Include(p => p.Experiences)
                .FirstOrDefault(p => p.Professionalid == id);

            if (professional == null)
            {
                return NotFound();
            }

            // Remove as associações de habilidades
            _dbContext.ProfessionalSkills.RemoveRange(professional.ProfessionalSkills);

            // Remove as experiências
            _dbContext.Experiences.RemoveRange(professional.Experiences);

            // Remove o profissional
            _dbContext.Professionals.Remove(professional);

            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPost("addskill")]
        public IActionResult AddSkillToProfessional(AddProfessionalSkillModel model)
        {
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == model.ProfessionalID);
            if (professional == null)
            {
                return NotFound();
            }

            var skill = _dbContext.Skills.FirstOrDefault(s => s.Skillid == model.SkillID);
            if (skill == null)
            {
                return BadRequest($"Skill with id {model.SkillID} does not exist");
            }

            var professionalSkill = new ProfessionalSkill 
            {
                Professionalid = professional.Professionalid,
                Skillid = model.SkillID,
                Yearsexperience = model.YearsExperience,
                Professional = professional,
                Skill = skill
            };

            _dbContext.ProfessionalSkills.Add(professionalSkill);
            _dbContext.SaveChanges();
    
            return Ok();
        }
        
        [HttpPost("addexperience")]
        public IActionResult AddExperienceToProfessional(AddExperienceModel model)
        {
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == model.ProfessionalID);
            if (professional == null)
            {
                return NotFound();
            }

            var experience = new Experience
            {
                Professionalid = model.ProfessionalID,
                Title = model.Title,
                Company = model.Company,
                Startyear = model.StartYear,
                Endyear = model.EndYear,
                Professional = professional
            };

            _dbContext.Experiences.Add(experience);
            _dbContext.SaveChanges();
    
            return Ok();
        }
    }
}
