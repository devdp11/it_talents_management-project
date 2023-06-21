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
        public async Task<ActionResult<IEnumerable<JobProposalModel>>> GetJobProposals()
        {
            return await _dbContext.Jobproposals
                .Include(jp => jp.JobproposalSkills)
                .Select(jp => new JobProposalModel
                {
                    JobProposalID = jp.Jobproposalid,
                    UserID = jp.Userid ?? 0,
                    ClientID = jp.Clientid ?? 0,
                    Name = jp.Name,
                    TalentCategory = jp.Talentcategory,
                    TotalHours = jp.Totalhours,
                    JobDescription = jp.Jobdescription,
                    JobproposalSkill = jp.JobproposalSkills.Select(s => new JobProposalSkillModel
                    {
                    // Aqui você mapeia a entidade Skill para o DTO SkillDto.
                    // Modifique o código abaixo para corresponder à sua classe SkillDto.
                    SkillID = s.Skillid,
                    MinYearsExperience = s.Minyearsexperience,
                    }).ToList()
                })
                .ToListAsync();
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
        public IActionResult AddProposal(CreateWorkProposalModel createProposalModel)
        {
            // Transformar o modelo de entrada numa entidade
            Jobproposal entity = new Jobproposal
            {
                Userid = createProposalModel.UserID,
                Clientid = createProposalModel.ClientID,
                Name = createProposalModel.Name,
                Talentcategory = createProposalModel.TalentCategory,
                Totalhours = createProposalModel.TotalHours,
                Jobdescription = createProposalModel.JobDescription
            };

            // Adicionar a entidade
            _dbContext.Jobproposals.Add(entity);
            _dbContext.SaveChanges();

            // Retornar a entidade (se necessário, pode converter de volta para um modelo)
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

            // Não permitir a atualização do Clientid, Userid e das skills (ProposalSkills)
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

            // Remove as associações de skills
            _dbContext.JobproposalSkills.RemoveRange(proposal.JobproposalSkills);

            // Remove a proposal
            _dbContext.Jobproposals.Remove(proposal);

            _dbContext.SaveChanges();

            return NoContent();
        }


        // POST: api/proposals/{id}/skills
        [HttpPost("addskill")]
        public IActionResult AddSkillToProposal(AddProposalSkillModel model)
        {
            var proposal = _dbContext.Jobproposals.FirstOrDefault(p => p.Jobproposalid == model.JobProposalID);
            if (proposal == null)
            {
                return NotFound();
            }

            var skill = _dbContext.Skills.FirstOrDefault(s => s.Skillid == model.SkillID);
            if (skill == null)
            {
                return BadRequest($"Skill with id {model.SkillID} does not exist");
            }

            var proposalSkill = new JobproposalSkill 
            {
                Jobproposalid = proposal.Jobproposalid,
                Skillid = model.SkillID,
                Minyearsexperience = model.YearsExperience,
                Jobproposal = proposal,
                Skill = skill
            };

            _dbContext.JobproposalSkills.Add(proposalSkill);
            _dbContext.SaveChanges();
    
            return Ok();
        }
    }
}
