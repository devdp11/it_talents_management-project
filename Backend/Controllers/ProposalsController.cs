using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Models;
using BusinessLogic.databaseContext;
using BusinessLogic.Entities;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProposalsController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public ProposalsController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/proposals
        [HttpGet]
        public IActionResult GetProposals()
        {
            var proposals = _dbContext.Jobproposals
                .Include(p => p.Client)
                .Include(p => p.JobproposalSkills)
                .ToList();

            return Ok(proposals);
        }

        // GET: api/proposals/{id}
        [HttpGet("{id}")]
        public IActionResult GetProposal(int id)
        {
            var proposal = _dbContext.Jobproposals
                .Include(p => p.Client)
                .Include(p => p.JobproposalSkills)
                .FirstOrDefault(p => p.Jobproposalid == id);

            if (proposal == null)
            {
                return NotFound();
            }

            return Ok(proposal);
        }

        // POST: api/proposals
        [HttpPost]
        public IActionResult AddProposal(JobProposalModel proposal)
        {
            // Transformar o modelo de entrada em uma entidade que pode ser guardada na base de dados
            Jobproposal entity = new Jobproposal
            {
                Userid = proposal.UserID,
                Clientid = proposal.ClientID,
                Name = proposal.Name,
                Talentcategory = proposal.TalentCategory,
                Totalhours = proposal.TotalHours,
                Jobdescription = proposal.JobDescription
            };
    
            // Mapear cada JobProposalSkillModel para uma entidade JobProposalSkill e adicionar à lista de JobProposalSkills da entidade JobProposal
            entity.JobproposalSkills = proposal.JobproposalSkill.Select(p => new JobproposalSkill 
            { 
                Skillid = p.SkillID, 
                Minyearsexperience = p.MinYearsExperience
            }).ToList();

            // Adicionar a entidade a base de dados
            _dbContext.Jobproposals.Add(entity);
            _dbContext.SaveChanges();

            // Retornar a entidade salva (se necessário, pode converter de volta para um modelo)
            return CreatedAtAction(nameof(GetProposal), new { id = entity.Jobproposalid }, entity);
        }

        // PUT: api/proposals/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProposal(int id, JobProposalUpdateModel updatedProposal)
        {
            var proposal = _dbContext.Jobproposals
                .Include(p => p.JobproposalSkills)
                .FirstOrDefault(p => p.Jobproposalid == id);
    
            if (proposal == null)
            {
                return NotFound();
            }

            // Atualizar campos simples
            proposal.Clientid = updatedProposal.ClientID;
            proposal.Name = updatedProposal.Name;
            proposal.Talentcategory = updatedProposal.TalentCategory;
            proposal.Totalhours = updatedProposal.TotalHours;
            proposal.Jobdescription = updatedProposal.JobDescription;

            // Remover todas as habilidades antigas
            _dbContext.JobproposalSkills.RemoveRange(proposal.JobproposalSkills);

            // Adicionar habilidades atualizadas
            proposal.JobproposalSkills = updatedProposal.JobproposalSkill
                .Select(skill => new JobproposalSkill
                {
                    Skillid = skill.SkillID,
                    Minyearsexperience = skill.MinYearsExperience
                })
                .ToList();

            _dbContext.SaveChanges();

            return NoContent();
        }

        // DELETE: api/proposals/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProposal(int id)
        {
            var proposal = _dbContext.Jobproposals.Include(p => p.JobproposalSkills).FirstOrDefault(p => p.Jobproposalid == id);
            if (proposal == null)
            {
                return NotFound();
            }

            _dbContext.JobproposalSkills.RemoveRange(proposal.JobproposalSkills);

            _dbContext.Jobproposals.Remove(proposal);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}