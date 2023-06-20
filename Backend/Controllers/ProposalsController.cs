using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Models;
using BusinessLogic.databaseContext;
using BusinessLogic.Entities;
using Frontend.Models;

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
                .ThenInclude(jp => jp.Skill)
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
        public IActionResult AddProposal(CreateWorkProposal createProposal)
        {
            // Transformar o modelo de entrada em uma entidade que pode ser guardada no banco de dados
            Jobproposal entity = new Jobproposal
            {
                Userid = createProposal.UserID,
                Clientid = createProposal.ClientID,
                Name = createProposal.Name,
                Talentcategory = createProposal.TalentCategory,
                Totalhours = createProposal.TotalHours,
                Jobdescription = createProposal.JobDescription
            };

            // Adicionar a entidade ao banco de dados
            _dbContext.Jobproposals.Add(entity);
            _dbContext.SaveChanges();

            // Retornar a entidade salva (se necessário, pode converter de volta para um modelo)
            return CreatedAtAction(nameof(GetProposal), new { id = entity.Jobproposalid }, entity);
        }

        // PUT: api/proposals/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateProposal(int id, UpdateProposalModel updatedProposal)
        {
            var proposal = _dbContext.Jobproposals
                .Include(p => p.JobproposalSkills)
                .FirstOrDefault(p => p.Jobproposalid == id);

            if (proposal == null)
            {
                return NotFound();
            }

            // Não permitir a atualização do Clientid, Userid e das habilidades (ProposalSkills)
            proposal.Name = updatedProposal.Name;
            proposal.Talentcategory = updatedProposal.TalentCategory;
            proposal.Totalhours = updatedProposal.TotalHours;
            proposal.Jobdescription = updatedProposal.JobDescription;

            _dbContext.SaveChanges();

            return NoContent();
        }

        // DELETE: api/proposals/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProposal(int id)
        {
            var proposal = _dbContext.Jobproposals
                .Include(p => p.JobproposalSkills)
                .FirstOrDefault(p => p.Jobproposalid == id);

            if (proposal == null)
            {
                return NotFound();
            }

            // Remove as associações de habilidades
            _dbContext.JobproposalSkills.RemoveRange(proposal.JobproposalSkills);

            // Remove a proposta
            _dbContext.Jobproposals.Remove(proposal);

            _dbContext.SaveChanges();

            return NoContent();
        }


        // POST: api/proposals/{id}/skills
        [HttpPost("{id}/skills")]
        public IActionResult AddProposalSkills(int id, List<AddProposalSkillModel> skills)
        {
            var proposal = _dbContext.Jobproposals.FirstOrDefault(p => p.Jobproposalid == id);

            if (proposal == null)
            {
                return NotFound();
            }

            // Adicionar as habilidades ao proposal
            foreach (var skill in skills)
            {
                proposal.JobproposalSkills.Add(new JobproposalSkill
                {
                    Skillid = skill.SkillID,
                    Minyearsexperience = skill.YearsExperience
                });
            }

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
