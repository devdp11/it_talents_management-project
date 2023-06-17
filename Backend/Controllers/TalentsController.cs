using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using BusinessLogic.Entities;
using BusinessLogic.databaseContext;
using Microsoft.AspNetCore.Mvc;

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
            var professionals = _dbContext.Professionals.ToList();
            return Ok(professionals);
        }

        [HttpPost]
        public ActionResult<ProfessionalModel> CreateProfessional(ProfessionalModel professionalModel)
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

            // Cria as entidades ProfessionalSkill
            foreach (var professionalSkillModel in professionalModel.ProfessionalSkills)
            {
                var skill = _dbContext.Skills.FirstOrDefault(s => s.Skillid == professionalSkillModel.SkillID);
                if (skill == null)
                {
                    return BadRequest($"Skill with id {professionalSkillModel.SkillID} does not exist");
                }

                var professionalSkill = new ProfessionalSkill 
                {
                    Professionalid = professional.Professionalid,
                    Skillid = professionalSkillModel.SkillID,
                    Yearsexperience = professionalSkillModel.YearsExperience,
                    Professional = professional,
                    Skill = skill
                };

                _dbContext.ProfessionalSkills.Add(professionalSkill);
            }

            _dbContext.SaveChanges();

            // Copia o ID do Professional gerado para o modelo
            professionalModel.ProfessionalID = professional.Professionalid;
    
            return CreatedAtAction(nameof(GetProfessionalById), new { id = professionalModel.ProfessionalID }, professionalModel);
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

        [HttpPut("{id}")]
        public IActionResult UpdateProfessional(int id, Professional updatedProfessional)
        {
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == id);
            if (professional == null)
            {
                return NotFound();
            }
            professional.Name = updatedProfessional.Name;
            professional.Country = updatedProfessional.Country;
            professional.Email = updatedProfessional.Email;
            professional.Hourlyrate = updatedProfessional.Hourlyrate;
            professional.Visibility = updatedProfessional.Visibility;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProfessional(int id)
        {
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == id);
            if (professional == null)
            {
                return NotFound();
            }
            _dbContext.Professionals.Remove(professional);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("{id}/skills")]
        public IActionResult AddSkillToProfessional(int id, ProfessionalSkill professionalSkill)
        {
            var professional = _dbContext.Professionals.FirstOrDefault(p => p.Professionalid == id);
            if (professional == null)
            {
                return NotFound();
            }

            // Verify if the skill already exists for the professional
            var existingSkill = professional.ProfessionalSkills.FirstOrDefault(ps => ps.Skillid == professionalSkill.Skillid);
            if (existingSkill != null)
            {
                return BadRequest("Skill already exists for the professional.");
            }

            professional.ProfessionalSkills.Add(professionalSkill);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
